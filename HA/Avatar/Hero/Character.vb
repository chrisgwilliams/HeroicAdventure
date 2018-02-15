Imports System.Console
Imports HA.Common
Imports DBuild.DunGen3
Imports HA.Common.Helper

Module Character

#Region " Hero Subs and Functions "

	Private Function CombatMessages(ByVal strWeaponName As String, _
									ByVal intEncounter As Integer, _
									ByVal minDam As Integer, ByVal maxDam As Integer, _
									ByVal Critical As Boolean, _
									ByVal intDamage As Integer, _
									ByVal intAttackType As Integer) As String

		Dim strMessage As String = "", _
			strVerb As String = "", _
			strVerb2 As String = ""

		Select Case intAttackType
			Case AttackType.Bludgeoning
				Select Case RND.Next(1, 3)
					Case 1
						strVerb = "crush"
					Case 2
						strVerb = "smash"
					Case 3
						strVerb = "pulverize"
				End Select

				strVerb2 = "crushes the " & m_arrMonster(intEncounter).MonsterRace

			Case AttackType.Piercing
				Select Case RND.Next(1, 3)
					Case 1
						strVerb = "stab"
					Case 2
						strVerb = "gut"
					Case 3
						strVerb = "impale"
				End Select

				strVerb2 = "bites deep"

			Case AttackType.Slashing
				Select Case RND.Next(1, 3)
					Case 1
						strVerb = "cut"
					Case 2
						strVerb = "slash"
					Case 3
						strVerb = "cripple"
				End Select

				strVerb2 = "bites deep"

			Case AttackType.SlashPierce
				Select Case RND.Next(1, 3)
					Case 1
						strVerb = "eviscerate"
					Case 2
						strVerb = "cut"
					Case 3
						strVerb = "impale"
				End Select

				strVerb2 = "guts the " & m_arrMonster(intEncounter).MonsterRace & " like a fish"

		End Select

		If Critical Then
			Select Case RND.Next(1, 2)
				Case 1
					strMessage = "Critical hit! You " & strVerb & " the " & m_arrMonster(intEncounter).MonsterRace & "!"
				Case 2
					strMessage = "Critical hit! Your " & strWeaponName & " strikes true!"
			End Select
		Else
			Select Case intDamage
				Case Is > maxDam * 0.75
					Select Case RND.Next(1, 2)
						Case 1
							strMessage = "A powerful hit! Your " & strWeaponName & " " & strVerb2 & "!"
						Case 2
							strMessage = "Excellent hit! The " & m_arrMonster(intEncounter).MonsterRace & " staggers!"
					End Select
				Case Is > maxDam * 0.5
					Select Case RND.Next(1, 2)
						Case 1
							strMessage = "You hit the " & m_arrMonster(intEncounter).MonsterRace & " squarely."
						Case 2
							strMessage = "You score a solid hit with your " & strWeaponName & "."
					End Select
				Case Else
					Select Case RND.Next(1, 2)
						Case 1
							strMessage = "A glancing blow with your " & strWeaponName & "."
						Case 2
							strMessage = "You graze the " & m_arrMonster(intEncounter).MonsterRace & "."
					End Select
			End Select
		End If

		Dim CritterType As Type = m_arrMonster(intEncounter).GetType()
		Select Case CType(m_arrMonster(intEncounter).currenthp, Integer)
			Case Is <= 0
				If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
					' Undead... say "destroyed"
					strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " is destroyed."
				Else
					strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " is dead."
				End If
			Case 1 To 2	' critical wounds
				If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
					' Undead... say nothing
				Else
					strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " is about to die."
				End If
			Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.5) ' serious wounds
				strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " is seriously wounded."
			Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.9) ' minor wounds
				strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " has minor wounds."
			Case Is > CInt(m_arrMonster(intEncounter).HP * 0.9)	' barely scratched
				strMessage += " The " & m_arrMonster(intEncounter).MonsterRace & " has a few scratches."
		End Select

		CombatMessages = strMessage & " "
	End Function

	Friend Function HeroAttack(ByVal intEncounter As Integer) As String
		Dim AC As Integer, _
			bolHit As Boolean, _
			intDamage As Integer, minDam As Integer, maxDam As Integer, _
			strMessage As String = "", _
			intRoll As Integer = 0, _
			critical As Boolean = False, _
			attacks As Integer = 0, _
			strLeftWpnName As String, _
			strRightWpnName As String

		' Determine monster AC
		AC = m_arrMonster(intEncounter).AC

		' precalculate damage potential and determine # of attacks
		minDam = 0
		maxDam = 0

		' anything in the right hand?
		If Not IsNothing(TheHero.Equipped.RightHand) Then
			' yes, but is it a weapon?
			If TheHero.Equipped.RightHand.type = ItemType.Weapon Then
				minDam += TheHero.Equipped.RightHand.MinDamage
				maxDam += TheHero.Equipped.RightHand.MaxDamage
				attacks += 1
				strRightWpnName = TheHero.Equipped.RightHand.name

				' roll D20 to attack
				intRoll = D20()
				If (intRoll + AbilityMod(TheHero.EStrength) + TheHero.BaseAtkBonus > AC) Or intRoll = 20 Then
					bolHit = True
					' get a number between the min and max damage, and add strength bonus
					intDamage = RND.Next(minDam, maxDam) + AbilityMod(TheHero.EStrength)

					' was it a critical hit? if so roll to hit again and apply crit multiplier if it hits
					If intRoll >= TheHero.Equipped.RightHand.critical Then
						If D20() + AbilityMod(TheHero.EStrength) + TheHero.BaseAtkBonus > AC Then
							intDamage *= TheHero.Equipped.RightHand.critmultiplier
							critical = True
						End If
					End If

					' monster was hit, so subtract the damage from current health
					m_arrMonster(intEncounter).currenthp -= intDamage

					' build the combat mesages
					strMessage += CombatMessages(strRightWpnName, intEncounter, minDam, maxDam, critical, intDamage, TheHero.Equipped.RightHand.mode)

				ElseIf intRoll = 1 Then	' critical miss, this is bad!
					strMessage += "Fumble! You missed horribly! "

				Else ' you missed... you loser
					Dim strVerb As String = ""
					Select Case TheHero.Equipped.RightHand.Mode
						Case AttackType.Bludgeoning
							strVerb = "swing"
						Case AttackType.Piercing
							strVerb = "stab"
						Case AttackType.Slashing
							strVerb = "slash"
						Case AttackType.SlashPierce
							strVerb = "thrash about"
					End Select

					strMessage += "You " & strVerb & " wildly at the " & m_arrMonster(intEncounter).MonsterRace & ", but do not hit! "
				End If
			End If
		End If

		' If monster is still alive, allow other attacks
		If m_arrMonster(intEncounter).currenthp > 0 Then

			' anything in the left hand?
			If Not IsNothing(TheHero.Equipped.LeftHand) Then
				' yes, but is it a weapon?
				If TheHero.Equipped.LeftHand.type = ItemType.Weapon Then
					minDam += TheHero.Equipped.LeftHand.MinDamage
					maxDam += TheHero.Equipped.LeftHand.MaxDamage
					attacks += 1
					strLeftWpnName = TheHero.Equipped.LeftHand.name

					' roll D20 to attack
					intRoll = D20()
					If (intRoll + AbilityMod(TheHero.EStrength) + TheHero.BaseAtkBonus > AC) Or intRoll = 20 Then
						bolHit = True
						' get a number between the min and max damage, and add strength bonus
						intDamage = RND.Next(minDam, maxDam) + AbilityMod(TheHero.EStrength)

						' was it a critical hit? if so roll to hit again and apply crit multiplier if it hits
						If intRoll >= TheHero.Equipped.RightHand.critical Then
							If D20() + AbilityMod(TheHero.EStrength) + TheHero.BaseAtkBonus > AC Then
								intDamage *= TheHero.Equipped.LeftHand.critmultiplier
								critical = True
							End If
						End If

						' monster was hit, so subtract the damage from current health
						m_arrMonster(intEncounter).currenthp -= intDamage

						' build the combat mesages
						strMessage += CombatMessages(strLeftWpnName, intEncounter, minDam, maxDam, critical, intDamage, TheHero.Equipped.LeftHand.mode)

					ElseIf intRoll = 1 Then	' critical miss, this is bad!
						strMessage += "Fumble! You missed horribly! "

					Else ' you missed... you loser
						Dim strVerb As String = ""
						Select Case TheHero.Equipped.RightHand.mode
							Case AttackType.Bludgeoning
								strVerb = "swing"
							Case AttackType.Piercing
								strVerb = "stab"
							Case AttackType.Slashing
								strVerb = "slash"
							Case AttackType.SlashPierce
								strVerb = "thrash about"
						End Select

						strMessage += "You " & strVerb & " wildly at the " & m_arrMonster(intEncounter).MonsterRace & ", but do not hit! "
					End If
				End If
			End If

		End If

		' attacks = 0 means no weapon, so try taking a punch (d6 dam)
		If attacks = 0 Then
			strLeftWpnName = "fist"
			minDam = 1
			maxDam = 6

			' roll D20 to attack
			intRoll = D20()
			If (intRoll + AbilityMod(TheHero.EStrength) + TheHero.BaseAtkBonus > AC) Or intRoll = 20 Then
				bolHit = True
				' get a number between the min and max damage, and add strength bonus
				intDamage = RND.Next(minDam, maxDam) + AbilityMod(TheHero.EStrength)

				' no critical hits for punches

				' monster was hit, so subtract the damage from current health
				m_arrMonster(intEncounter).currenthp -= intDamage

				' build the combat mesages
				strMessage += CombatMessages(strLeftWpnName, intEncounter, minDam, maxDam, critical, intDamage, AttackType.Bludgeoning)

			ElseIf intRoll = 1 Then	' critical miss, this is bad!
				strMessage += "Fumble! You missed horribly! "

			Else ' you missed... you loser
				strMessage += "You swing wildly at the " & m_arrMonster(intEncounter).MonsterRace & ", but do not hit! "
			End If
		End If

		' display the battle message
		HeroAttack = strMessage

		' monster is dead
		If m_arrMonster(intEncounter).currenthp <= 0 Then
			' remove the DEAD monster from the screen
			Level(m_arrMonster(intEncounter).locx, m_arrMonster(intEncounter).locy, TheHero.LocZ).Monster = 0
			FixFloor(m_arrMonster(intEncounter).locx, m_arrMonster(intEncounter).locy, TheHero.LocZ)

			' set the monsters status to dead
			m_arrMonster(intEncounter).dead = True

			' increment kill count
			TheHero.TotalKills += 1

			' assign XP based on CR vs Hero.TotalLevel
			TheHero.XP += AwardXP(m_arrMonster(intEncounter).CR)
			UpdateXPDisplay()

			' debug messages
			Debug.WriteLine("Monster " & intEncounter & " has been killed.")
			Debug.WriteLine("Hero total kills = " & TheHero.TotalKills)
		End If

	End Function

	Friend Sub HeroDisplay()
		Clear()
        'TODO: Add survival time (so far)
        WriteAt(0, 0, "---------------------------------- Your Hero ----------------------------------", ConsoleColor.DarkYellow)
		WriteAt(35, 0, "Your Hero", ConsoleColor.Yellow)

		WriteAt(1, 1, "Name:", ConsoleColor.Gray)
		WriteAt(7, 1, TheHero.Name)
		WriteAt(24, 1, "Class:", ConsoleColor.Gray)
		WriteAt(31, 1, GetClass(TheHero.HeroClass))
		WriteAt(48, 1, "Race:", ConsoleColor.Gray)
		WriteAt(54, 1, GetRace(TheHero.HeroRace))

		WriteAt(1, 2, "Level: ", ConsoleColor.Gray)
		WriteAt(8, 2, TheHero.TotalLevels)
		WriteAt(11, 2, "XP:", ConsoleColor.Gray)
		WriteAt(15, 2, TheHero.XP)
		WriteAt(24, 2, "Next Level:", ConsoleColor.Gray)
		WriteAt(36, 2, LevelLookup(TheHero.TotalLevels + 1) & " XP")

		WriteAt(6, 5, "Base  Eff.  Bonus", ConsoleColor.Gray)

		WriteAt(1, 6, "Str:", ConsoleColor.Gray)
		WriteAt(7, 6, TheHero.Strength)
		If TheHero.EStrength = TheHero.Strength Then
			WriteAt(13, 6, TheHero.EStrength)
		ElseIf TheHero.EStrength > TheHero.Strength Then
			WriteAt(13, 6, TheHero.EStrength, ConsoleColor.Green)
		ElseIf TheHero.EStrength < TheHero.Strength Then
			WriteAt(13, 6, TheHero.EStrength, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.EStrength), 1).ToString = "-" Then
			WriteAt(19, 6, AbilityMod(TheHero.EStrength))
		Else
			WriteAt(19, 6, "+" & AbilityMod(TheHero.EStrength))
		End If

		WriteAt(1, 7, "Int:", ConsoleColor.Gray)
		WriteAt(7, 7, TheHero.Intelligence)
		If TheHero.EIntelligence = TheHero.Intelligence Then
			WriteAt(13, 7, TheHero.EIntelligence)
		ElseIf TheHero.EIntelligence > TheHero.Intelligence Then
			WriteAt(13, 7, TheHero.EIntelligence, ConsoleColor.Green)
		ElseIf TheHero.EIntelligence < TheHero.Intelligence Then
			WriteAt(13, 7, TheHero.EIntelligence, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.EIntelligence), 1).ToString = "-" Then
			WriteAt(19, 7, AbilityMod(TheHero.EIntelligence))
		Else
			WriteAt(19, 7, "+" & AbilityMod(TheHero.EIntelligence))
		End If

		WriteAt(1, 8, "Wis:", ConsoleColor.Gray)
		WriteAt(7, 8, TheHero.Wisdom)
		If TheHero.EWisdom = TheHero.Wisdom Then
			WriteAt(13, 8, TheHero.EWisdom)
		ElseIf TheHero.EWisdom > TheHero.Wisdom Then
			WriteAt(13, 8, TheHero.EWisdom, ConsoleColor.Green)
		ElseIf TheHero.EWisdom < TheHero.Wisdom Then
			WriteAt(13, 8, TheHero.EWisdom, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.EWisdom), 1).ToString = "-" Then
			WriteAt(19, 8, AbilityMod(TheHero.EWisdom))
		Else
			WriteAt(19, 8, "+" & AbilityMod(TheHero.EWisdom))
		End If

		WriteAt(1, 9, "Dex:", ConsoleColor.Gray)
		WriteAt(7, 9, TheHero.Dexterity)
		If TheHero.EDexterity = TheHero.Dexterity Then
			WriteAt(13, 9, TheHero.EDexterity)
		ElseIf TheHero.EDexterity > TheHero.Dexterity Then
			WriteAt(13, 9, TheHero.EDexterity, ConsoleColor.Green)
		ElseIf TheHero.EDexterity < TheHero.Dexterity Then
			WriteAt(13, 9, TheHero.EDexterity, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.EDexterity), 1).ToString = "-" Then
			WriteAt(19, 9, AbilityMod(TheHero.EDexterity))
		Else
			WriteAt(19, 9, "+" & AbilityMod(TheHero.EDexterity))
		End If

		WriteAt(1, 10, "Con:", ConsoleColor.Gray)
		WriteAt(7, 10, TheHero.Constitution)
		If TheHero.EConstitution = TheHero.Constitution Then
			WriteAt(13, 10, TheHero.EConstitution)
		ElseIf TheHero.EConstitution > TheHero.Constitution Then
			WriteAt(13, 10, TheHero.EConstitution, ConsoleColor.Green)
		ElseIf TheHero.EConstitution < TheHero.Constitution Then
			WriteAt(13, 10, TheHero.EConstitution, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.EConstitution), 1).ToString = "-" Then
			WriteAt(19, 10, AbilityMod(TheHero.EConstitution))
		Else
			WriteAt(19, 10, "+" & AbilityMod(TheHero.EConstitution))
		End If

		WriteAt(1, 11, "Cha:", ConsoleColor.Gray)
		WriteAt(7, 11, TheHero.Charisma)
		If TheHero.ECharisma = TheHero.Charisma Then
			WriteAt(13, 11, TheHero.ECharisma)
		ElseIf TheHero.ECharisma > TheHero.Charisma Then
			WriteAt(13, 11, TheHero.ECharisma, ConsoleColor.Green)
		ElseIf TheHero.ECharisma < TheHero.Charisma Then
			WriteAt(13, 11, TheHero.ECharisma, ConsoleColor.Red)
		End If
		If Left(AbilityMod(TheHero.ECharisma), 1).ToString = "-" Then
			WriteAt(19, 11, AbilityMod(TheHero.ECharisma))
		Else
			WriteAt(19, 11, "+" & AbilityMod(TheHero.ECharisma))
		End If

		WriteAt(6, 13, "Curr  Max", ConsoleColor.Gray)
		WriteAt(1, 14, "HP:", ConsoleColor.Gray)
		WriteAt(7, 14, TheHero.CurrentHP)
		WriteAt(13, 14, TheHero.HP)

		WriteAt(1, 15, "MP:", ConsoleColor.Gray)
		WriteAt(7, 15, "0")
		WriteAt(13, 15, "0")

		WriteAt(36, 13, "Equipped          Atk   Damage", ConsoleColor.Gray)
		WriteAt(27, 14, "Melee: ", ConsoleColor.Gray)

		Dim strDamage As String, WeaponYrow As Int16 = 14, _
			strLeftWpn As String, strRightWpn As String
		Dim intRightBAB As Int16 = TheHero.BaseAtkBonus + AbilityMod(TheHero.EStrength), _
			intLeftBAB As Int16 = TheHero.BaseAtkBonus + AbilityMod(TheHero.EStrength), _
			HandACBonus As Int16 = 0, _
			intRangedBAB As Int16 = TheHero.BaseAtkBonus + AbilityMod(TheHero.EDexterity)

		With TheHero
			If Not .Equipped.LeftHand Is Nothing Then
				If .Equipped.LeftHand.type = ItemType.Weapon Then
					strDamage = .Equipped.LeftHand.Damage & "+" & AbilityMod(.EStrength).ToString
					strLeftWpn = .Equipped.LeftHand.name
					intLeftBAB += TheHero.Equipped.LeftHand.atkBonus
					WriteAt(36, WeaponYrow, strLeftWpn)
					WriteAt(54, WeaponYrow, intLeftBAB)
					WriteAt(60, WeaponYrow, strDamage)
					WeaponYrow += 1
				Else
					strLeftWpn = ""
					HandACBonus += TheHero.Equipped.LeftHand.acbonus
				End If
			End If

			If Not .Equipped.RightHand Is Nothing Then
				If .Equipped.RightHand.type = ItemType.Weapon Then
					strDamage = .Equipped.RightHand.Damage & "+" & AbilityMod(.EStrength).ToString
					strRightWpn = .Equipped.RightHand.name
					intRightBAB += TheHero.Equipped.RightHand.atkbonus
					WriteAt(36, WeaponYrow, strRightWpn)
					WriteAt(54, WeaponYrow, intRightBAB)
					WriteAt(60, WeaponYrow, strDamage)
				Else
					strRightWpn = ""
					HandACBonus += TheHero.Equipped.RightHand.acbonus
				End If
			End If

			WriteAt(27, 16, "Ranged: ", ConsoleColor.Gray)
			If Not .Equipped.MissleWeapon Is Nothing Then
				intRangedBAB += .Equipped.MissleWeapon.atkBonus
				WriteAt(36, 16, .Equipped.MissleWeapon.name)
				WriteAt(54, 16, intRangedBAB)
				WriteAt(60, 16, .Equipped.Missles.Damage)
			End If

			WriteAt(32, 5, "AC = 10 + Armor + Shield + Dex + Misc", ConsoleColor.Gray)
			WriteAt(32, 6, .AC)
			WriteAt(37, 6, "10")

			Dim intArmorBonus As Int16
			intArmorBonus += .NaturalArmor
			If Not .Equipped.Helmet Is Nothing Then
				intArmorBonus += .Equipped.Helmet.acbonus
			End If
			If Not .Equipped.Cloak Is Nothing Then
				intArmorBonus += .Equipped.Cloak.acbonus
			End If
			If Not .Equipped.Armor Is Nothing Then
				intArmorBonus += .Equipped.Armor.acbonus
			End If
			If Not .Equipped.Gloves Is Nothing Then
				intArmorBonus += .Equipped.Gloves.acbonus
			End If
			If Not .Equipped.Bracers Is Nothing Then
				intArmorBonus += .Equipped.Bracers.acbonus
			End If
			If Not .Equipped.Boots Is Nothing Then
				intArmorBonus += .Equipped.Boots.acbonus
			End If
			'TODO: BUG! Armor is being added twice, to AC and MiscACMod, must fix
			WriteAt(44, 6, intArmorBonus)
		End With

		WriteAt(52, 6, HandACBonus)
		WriteAt(60, 6, AbilityMod(TheHero.EDexterity))

		Dim intMiscMod As Int16
		intMiscMod += TheHero.MiscACMod
		If Not TheHero.Equipped.Neck Is Nothing Then
			If TheHero.Equipped.Neck.acbonus > 0 Then
				intMiscMod += TheHero.Equipped.Neck.acbonus
			End If
		End If
		If Not TheHero.Equipped.LeftRing Is Nothing Then
			intMiscMod += TheHero.Equipped.LeftRing.acbonus
		End If
		If Not TheHero.Equipped.RightRing Is Nothing Then
			intMiscMod += TheHero.Equipped.RightRing.acbonus
		End If
		WriteAt(66, 6, intMiscMod)


		PressAKey()
		RedrawDisplay()
	End Sub

	Friend Sub HeroLevelUpCheck()
        If TheHero.XP >= LevelLookup(TheHero.TotalLevels + 1) Then

            ' let them know they went up a level
            MessageHandler("You went up a level. You feel more powerful.")
            More(46, 0)
            WriteAt(1, 0, CLEARSPACE)

            ' increase total levels
            TheHero.TotalLevels += 1
            UpdateLvlDisplay()

            ' adjust BaseAttackBonus
            TheHero.BaseAtkBonus = BaseAtkBonus(TheHero.HeroClass)
            UpdateAtkBonus()

            HeroIncreaseStat()
            HeroIncreaseHP()
            'TODO: increase skills at LevelUp
        Else
            ' Sorry, you're not quite there yet... keep killing stuff.
        End If

		UpdateEffectiveStats()
		If Not TheHero.Overland Then DisplayValues()
	End Sub

    Private Sub HeroIncreaseStat()
        ' check for potential stat increase every 4th level
        If TheHero.TotalLevels Mod 4 = 0 And TheHero.HeroClass > 0 Then
            ' Savage Races must have matured (selected a class) 
            ' in order to specify attribute increases
            MessageHandler("You may increase a stat by 1 point.")
            More(37, 0)
            WriteAt(1, 0, CLEARSPACE)
            MessageHandler("Please select 1-6 (Str, Int, Wis, Dex, Con, Cha): ")
            CursorLeft = 51
            CursorTop = 0
            CursorVisible = True

            Dim statIncrease As ConsoleKeyInfo
            Dim ok As Boolean = False

            Do While Not ok
                statIncrease = ReadKey()
                With TheHero
                    Select Case statIncrease.KeyChar.ToString
                        Case PCStats.strength
                            .Strength += 1
                            ok = True
                        Case PCStats.intelligence
                            .Intelligence += 1
                            ok = True
                        Case PCStats.wisdom
                            .Wisdom += 1
                            ok = True
                        Case PCStats.dexterity
                            .Dexterity += 1
                            ok = True
                        Case PCStats.constitution
                            .Constitution += 1
                            ok = True
                        Case PCStats.charisma
                            .Charisma += 1
                            ok = True
                        Case Else
                            ok = False
                    End Select
                End With
            Loop
            UpdateEffectiveStats()
            WriteAt(1, 0, CLEARSPACE)
            CursorVisible = False
        End If

    End Sub
    Private Sub HeroIncreaseHP()
        ' increase HP
        Dim newHP As Int16
        If TheHero.HeroRace = Race.Ogre And TheHero.HeroClass = 0 Then
            newHP = OgreUp()
        ElseIf TheHero.HeroRace = Race.Pixie And TheHero.HeroClass = 0 Then
            newHP = PixieUp()
        Else
            newHP = RND.Next(1, TheHero.HitDieType) + AbilityMod(TheHero.EConstitution)
        End If

        TheHero.HP += newHP
        TheHero.CurrentHP += newHP
        UpdateHPDisplay()

    End Sub

    Private Function OgreUp() As Int16
		Dim newHP As Int16
        'ToDo: Take a look at Ogre HP Progression when leveling up. Make sure it's working properly.
        'ToDo: Ogre needs class levels after ECL 6
        Select Case TheHero.TotalLevels
            Case 2
                ' 2d8 HD
                newHP += (RND.Next(1, TheHero.HitDieType) + AbilityMod(TheHero.EConstitution))
                TheHero.Strength += 2
                TheHero.Constitution += 2
            Case 3
                ' 3D8 HD
                newHP += (RND.Next(1, TheHero.HitDieType) + AbilityMod(TheHero.EConstitution))
                TheHero.NaturalArmor = 4
                CalculateAC()
            Case 4
                ' 3D8 HD
                TheHero.Strength += 2
                TheHero.Constitution += 2
            Case 5
                ' 4D8 HD
                newHP += (RND.Next(1, TheHero.HitDieType) + AbilityMod(TheHero.EConstitution))
                TheHero.Strength += 2
                TheHero.Dexterity -= 2
                CalculateAC()
            Case 6
                '4D8 HD
                TheHero.Strength += 2
                TheHero.NaturalArmor = 5
                CalculateAC()
        End Select

        Return newHP
	End Function
	Private Function PixieUp() As Int16
		Dim newHP As Int16

		Select Case TheHero.TotalLevels
			Case 2
				' D6 HD
				TheHero.Flight = 20
				TheHero.Intelligence += 2
				TheHero.Charisma += 2
			Case 3
				' D6 HD
				TheHero.Flight = 40
				TheHero.NaturalArmor = 1
				TheHero.Wisdom += 2
				TheHero.Dexterity += 2
				CalculateAC()
			Case 4
				' D6 HD
				TheHero.Flight = 60
				TheHero.Intelligence += 2
				TheHero.Charisma += 2
			Case 5
				' D6 HD
				TheHero.SpellResist = 16
				TheHero.Wisdom += 2
				TheHero.Dexterity += 2
				CalculateAC()
		End Select

		newHP += AbilityMod(TheHero.EConstitution)
		Return newHP
	End Function

    <DebuggerStepThrough()>
    Friend Sub UpdateEffectiveStats()
        With TheHero
            .EStrength = .Strength + .StrMods
            .EIntelligence = .Intelligence + .IntMods
            .EWisdom = .Wisdom + .WisMods
            .EDexterity = .Dexterity + .DexMods
            .EConstitution = .Constitution + .ConMods
            .ECharisma = .Charisma + .ChaMods
        End With
    End Sub

    <DebuggerStepThrough()>
    Private Function BaseAtkBonus(ByVal heroClass As Integer) As Integer
		Select Case heroClass
			Case PCClass.Warrior, PCClass.Barbarian, PCClass.Paladin
				BaseAtkBonus = TheHero.TotalLevels

			Case PCClass.Assassin, PCClass.Druid, PCClass.Priest, PCClass.Thief
				BaseAtkBonus = CInt(TheHero.TotalLevels * 0.75)

			Case PCClass.Sorceror, PCClass.Wizard
				BaseAtkBonus = CInt(TheHero.TotalLevels * 0.5)

			Case 0
				Select Case TheHero.HeroRace
					Case Race.Ogre
						Select Case TheHero.TotalLevels
							Case 2
								BaseAtkBonus = 1
							Case 3
								BaseAtkBonus = 2
							Case 4
								BaseAtkBonus = 2
							Case 5
								BaseAtkBonus = 3
							Case 6
								BaseAtkBonus = 3
						End Select

					Case Race.Pixie
						Select Case TheHero.TotalLevels
							Case 2
								BaseAtkBonus = 0
							Case 3
								BaseAtkBonus = 0
							Case 4
								BaseAtkBonus = 0
							Case 5
								BaseAtkBonus = 0
						End Select

				End Select
		End Select

	End Function

    <DebuggerStepThrough()>
    Private Sub UpdateAtkBonus()
		If CInt(TheHero.BaseAtkBonus) + CInt(AbilityMod(TheHero.EStrength)) >= 0 Then
			WriteAt(72, 14, "+" & TheHero.BaseAtkBonus + AbilityMod(TheHero.EStrength) & " ")
		Else
			WriteAt(72, 14, TheHero.BaseAtkBonus + AbilityMod(TheHero.EStrength) & " ")
		End If

	End Sub

	Friend Function AwardXP(ByVal CR As Single) As Integer
		Select Case TheHero.TotalLevels
			Case 1 To 3
				Select Case CR
					Case 0 To 3
						AwardXP = CInt(CR * 300)
					Case 4
						AwardXP = 1350
					Case Is > 4
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 4
				Select Case CR
					Case 0 To 2
						AwardXP = CInt(CR * 300)
					Case 3
						AwardXP = 800
					Case Is > 3
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 5
				Select Case CR
					Case 0 To 1
						AwardXP = CInt(CR * 300)
					Case 2 To 3
						AwardXP = CInt(CR * 250)
					Case Is > 3
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 6
				Select Case CR
					Case 0 To 1
						AwardXP = CInt(CR * 300)
					Case 2
						AwardXP = 450
					Case Is > 2
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 7
				Select Case CR
					Case 0 To 1
						AwardXP = CInt(CR * 263)
					Case 2
						AwardXP = 394
					Case 3
						AwardXP = 525
					Case 4
						AwardXP = 700
					Case Is > 4
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 8
				Select Case CR
					Case 0 To 1
						AwardXP = CInt(CR * 200)
					Case 2
						AwardXP = 300
					Case 3
						AwardXP = 450
					Case 4
						AwardXP = 600
					Case Is > 4
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 9
				Select Case CR
					Case Is < 2
						AwardXP = 0
					Case 2
						AwardXP = 225
					Case 3
						AwardXP = 338
					Case 4
						AwardXP = 506
					Case Is > 4
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 10
				Select Case CR
					Case Is < 3
						AwardXP = 0
					Case 3
						AwardXP = 250
					Case 4
						AwardXP = 375
					Case 5
						AwardXP = 563
					Case Is > 5
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 11
				Select Case CR
					Case Is < 4
						AwardXP = 0
					Case 4
						AwardXP = 250
					Case 5
						AwardXP = 375
					Case 6
						AwardXP = 563
					Case Is > 6
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case 12
				Select Case CR
					Case Is < 5
						AwardXP = 0
					Case 5
						AwardXP = 250
					Case 6
						AwardXP = 375
					Case 7
						AwardXP = 563
					Case Is > 7
						AwardXP = AwardXP(CR - 2) * 2
				End Select
			Case Is > 12
				Select Case CR
					Case Is < TheHero.TotalLevels - 7
						AwardXP = 0
					Case TheHero.TotalLevels - 7
						AwardXP = 250
					Case TheHero.TotalLevels - 6
						AwardXP = 375
					Case TheHero.TotalLevels - 5
						AwardXP = 563
					Case Is > TheHero.TotalLevels - 5
						AwardXP = AwardXP(CR - 2) * 2
				End Select
		End Select
	End Function

    <DebuggerStepThrough()>
    Friend Sub HeroLOS()
		' process visible area
		Dim MinX As Int16, MinY As Int16, _
			MaxX As Int16, MaxY As Int16, _
			intXCtr As Int16, intYCtr As Int16, _
			intCtr As Int16, sight As Int16

		Select Case Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).Illumination
			Case IlluminationStrength.Dark
				sight = 1
			Case IlluminationStrength.Torch
				sight = 2
			Case Else
				sight = TheHero.Sight
		End Select

		MinX = TheHero.LocX - sight
		If MinX < 0 Then MinX = 0
		MaxX = TheHero.LocX + sight
		If MaxX > 60 Then MaxX = 60

		MinY = TheHero.LocY - sight
		If MinY < 0 Then MinY = 0
		MaxY = TheHero.LocY + sight
		If MaxY > 22 Then MaxY = 22

		For intCtr = 1 To 8
			Select Case intCtr
				Case 1
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MinY Step -1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							If intYCtr < TheHero.LocY Then
								Dim xloop As Integer
								For xloop = -1 To 1
									If (intXCtr + xloop = TheHero.LocX And intYCtr = TheHero.LocY) _
									Or Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster > 0 _
									Or Level(intXCtr + xloop, intYCtr, TheHero.LocZ).itemcount > 0 Then
										' do nothing
									Else
										FixFloor(intXCtr + xloop, intYCtr, TheHero.LocZ)
										Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Observed = True
									End If

									' refresh items on floor
									If Level(intXCtr + xloop, intYCtr, TheHero.LocZ).itemcount > 0 Then
										WriteAt(intXCtr + xloop, intYCtr, Level(intXCtr + xloop, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr + xloop, intYCtr, TheHero.LocZ).items(0).Color)
									End If

									' if a monster is standing here we need to refresh it
									If Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster > 0 Then
										Dim intMID As Integer
										intMID = Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster - 1
										WriteAt(intXCtr + xloop, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
									End If
								Next
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If

				Case 2
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MinY + 1 Step -1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							' dont "fix" any tiles with (hero, creature or item) or it will overwrite the symbol
							If (intXCtr = TheHero.LocX And intYCtr = TheHero.LocY) _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								' this is where the hero or a monster is, don't overwrite them
							Else
								FixFloor(intXCtr, intYCtr, TheHero.LocZ)
							End If

							' refresh items on floor
							If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
							End If

							' if a monster is standing here we need to refresh it
							If Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 Then
								Dim intMID As Integer
								intMID = Level(intXCtr, intYCtr, TheHero.LocZ).Monster - 1
								WriteAt(intXCtr, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							 Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
						intXCtr += 1
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If

				Case 3
					intYCtr = TheHero.LocY
					For intXCtr = TheHero.LocX To MaxX

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							If intXCtr > TheHero.LocX Then
								Dim yloop As Integer
								For yloop = -1 To 1

									If (intXCtr = TheHero.LocX And intYCtr + yloop = TheHero.LocY) _
									Or Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster > 0 _
									Or Level(intXCtr, intYCtr + yloop, TheHero.LocZ).itemcount > 0 Then
										' do nothing
									Else
										FixFloor(intXCtr, intYCtr + yloop, TheHero.LocZ)
										Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Observed = True
									End If

									' refresh items on floor
									If Level(intXCtr, intYCtr + yloop, TheHero.LocZ).itemcount > 0 Then
										WriteAt(intXCtr, intYCtr + yloop, Level(intXCtr, intYCtr + yloop, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr + yloop, TheHero.LocZ).items(0).Color)
									End If

									' if a monster is standing here we need to refresh it
									If Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster > 0 Then
										Dim intMID As Integer
										intMID = Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster - 1
										WriteAt(intXCtr, intYCtr + yloop, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
									End If
								Next
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
					Next	' intXCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If


				Case 4
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MaxY - 1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							' dont "fix" any tiles with (hero, creature or item) or it will overwrite the symbol
							If (intXCtr = TheHero.LocX And intYCtr = TheHero.LocY) _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								' this is where the hero or a monster is, don't overwrite them
							Else
								FixFloor(intXCtr, intYCtr, TheHero.LocZ)
							End If

							' refresh items on floor
							If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
							End If

							' if a monster is standing here we need to refresh it
							If Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 Then
								Dim intMID As Integer
								intMID = Level(intXCtr, intYCtr, TheHero.LocZ).Monster - 1
								WriteAt(intXCtr, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
						intXCtr += 1
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If


				Case 5
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MaxY

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							If intYCtr > TheHero.LocY Then
								Dim xloop As Integer
								For xloop = -1 To 1
									If (intXCtr + xloop = TheHero.LocX And intYCtr = TheHero.LocY) _
									Or Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster > 0 _
									Or Level(intXCtr + xloop, intYCtr, TheHero.LocZ).itemcount > 0 Then
										' do nothing
									Else
										FixFloor(intXCtr + xloop, intYCtr, TheHero.LocZ)
										Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Observed = True
									End If

									' refresh items on floor
									If Level(intXCtr + xloop, intYCtr, TheHero.LocZ).itemcount > 0 Then
										WriteAt(intXCtr + xloop, intYCtr, Level(intXCtr + xloop, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr + xloop, intYCtr, TheHero.LocZ).items(0).Color)
									End If

									' if a monster is standing here we need to refresh it
									If Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster > 0 Then
										Dim intMID As Integer
										intMID = Level(intXCtr + xloop, intYCtr, TheHero.LocZ).Monster - 1
										WriteAt(intXCtr + xloop, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
									End If
								Next
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If


				Case 6
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MaxY - 1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							' dont "fix" any tiles with (hero, creature or item) or it will overwrite the symbol
							If (intXCtr = TheHero.LocX And intYCtr = TheHero.LocY) _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								' this is where the hero or a monster is, don't overwrite them
							Else
								FixFloor(intXCtr, intYCtr, TheHero.LocZ)
							End If

							' refresh items on floor
							If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
							End If

							' if a monster is standing here we need to refresh it
							If Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 Then
								Dim intMID As Integer
								intMID = Level(intXCtr, intYCtr, TheHero.LocZ).Monster - 1
								WriteAt(intXCtr, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
						intXCtr -= 1
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If


				Case 7
					intYCtr = TheHero.LocY
					For intXCtr = TheHero.LocX To MinX Step -1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							If intXCtr < TheHero.LocX Then
								Dim yloop As Integer
								For yloop = -1 To 1
									If (intXCtr = TheHero.LocX And intYCtr + yloop = TheHero.LocY) _
									Or Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster > 0 _
									Or Level(intXCtr, intYCtr + yloop, TheHero.LocZ).itemcount > 0 Then
										' do nothing
									Else
										FixFloor(intXCtr, intYCtr + yloop, TheHero.LocZ)
										Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Observed = True
									End If

									' refresh items on floor
									If Level(intXCtr, intYCtr + yloop, TheHero.LocZ).itemcount > 0 Then
										WriteAt(intXCtr, intYCtr + yloop, Level(intXCtr, intYCtr + yloop, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr + yloop, TheHero.LocZ).items(0).Color)
									End If

									' if a monster is standing here we need to refresh it
									If Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster > 0 Then
										Dim intMID As Integer
										intMID = Level(intXCtr, intYCtr + yloop, TheHero.LocZ).Monster - 1
										WriteAt(intXCtr, intYCtr + yloop, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
									End If
								Next
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If


				Case 8
					intXCtr = TheHero.LocX
					For intYCtr = TheHero.LocY To MinY + 1 Step -1

						' check to see if square is lit or dark
						If Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
							' check to see if the square has already been observed, if so skip this section
							' this speeds up the program a little and also reduces flicker
							If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = False Then
								Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True
							End If

							' dont "fix" any tiles with (hero, creature or item) or it will overwrite the symbol
							If (intXCtr = TheHero.LocX And intYCtr = TheHero.LocY) _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								' this is where the hero or a monster is, don't overwrite them
							Else
								FixFloor(intXCtr, intYCtr, TheHero.LocZ)
							End If

							' refresh items on floor
							If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
								WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
							End If

							' if a monster is standing here we need to refresh it
							If Level(intXCtr, intYCtr, TheHero.LocZ).Monster > 0 Then
								Dim intMID As Integer
								intMID = Level(intXCtr, intYCtr, TheHero.LocZ).Monster - 1
								WriteAt(intXCtr, intYCtr, m_arrMonster(intMID).Character, m_arrMonster(intMID).Color)
							End If

							' if LOS hits a wall stop immediately
							If Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Door _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Wall _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.NECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SECorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.SWCorner _
							Or Level(intXCtr, intYCtr, TheHero.LocZ).FloorType = SquareType.Secret Then
								Exit For
							Else
								'do nothing
							End If

						End If
						intXCtr -= 1
					Next	' intYCtr

					' added this section to prevent orphaned monster symbols from lingering on the map
					If Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
						FixFloor(intXCtr, intYCtr, TheHero.LocZ)
					End If

			End Select
		Next

		WriteAt(TheHero.LocX, TheHero.LocY, TheHero.Icon, TheHero.Color)
	End Sub

    <DebuggerStepThrough()>
    Friend Sub UpdateHPDisplay()
		Dim strHPdisplay As String = TheHero.CurrentHP

		If (TheHero.CurrentHP <= CInt(TheHero.HP * 0.75)) _
		And (TheHero.CurrentHP > CInt(TheHero.HP * 0.25)) Then
			WriteAt(65, 10, strHPdisplay, ConsoleColor.Yellow)

		ElseIf TheHero.CurrentHP <= CInt(TheHero.HP * 0.25) Then
			WriteAt(65, 10, strHPdisplay, ConsoleColor.Red)

		Else
			WriteAt(65, 10, strHPdisplay)
		End If

		WriteAt(65 + Len(strHPdisplay), 10, "/" & TheHero.HP & "   ")
	End Sub

	Friend Sub UpdateLvlDisplay()
		WriteAt(68, 17, TheHero.TotalLevels)
	End Sub

    <DebuggerStepThrough()>
    Friend Sub UpdateXPDisplay()
		WriteAt(65, 11, TheHero.XP)
	End Sub

    <DebuggerStepThrough()>
    Friend Sub HeroHealing()
		Dim iHealRate As Integer = (35 - (AbilityMod(TheHero.EConstitution) * 5))
		If iHealRate < 1 Then iHealRate = 1

		If TheHero.TurnCount Mod iHealRate = 0 _
		And TheHero.CurrentHP < TheHero.HP Then
			TheHero.CurrentHP += 1
			UpdateHPDisplay()
		End If
	End Sub

#End Region

End Module
