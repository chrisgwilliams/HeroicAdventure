Imports HA.Common
Imports DBuild.DunGen3
Imports System.Console
Imports System.Math
Imports System.Text
Imports System.Collections.Generic
Imports HA.Common.Helper
Imports HA.My.Resources.English

Module m_MiscRoutines

#Region " Trap-Related Routines "

	Friend Function TrapDescriptor(ByVal myTrapType As TrapType) As String
		Select Case myTrapType
			Case TrapType.sleep
                Return resTrapSleep
            Case TrapType.poison
                Return resTrapPoison
            Case TrapType.explosion
                Return resTrapExplosion
            Case TrapType.pit
                Return ResTrapPit
            Case TrapType.snake
                Return resTrapSnake
            Case TrapType.rock
                Return resTrapRock
            Case TrapType.confusion
                Return resTrapConfusion
            Case TrapType.teleport
                Return resTrapTeleport
            Case Else
				Return ""
		End Select
	End Function

	Friend Function TriggerTrap() As String
		Dim strMessage As String = ""

		Select Case Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType
			Case TrapType.sleep
				strMessage = SleepHero()

			Case TrapType.poison
                strMessage = resTrapPoisonEffect
                ' cloud of poison gas... use showexplosion for the special effects
                ShowExplosion(TheHero.LocX, TheHero.LocY, ConsoleColor.Cyan, ConsoleColor.Green)
				PoisonHero()

			Case TrapType.explosion
				Dim iDamageDice As Int16, iCtr As Int16
				iDamageDice = (D4() * (TheHero.TotalLevels \ 4) + (TheHero.CurrentLevel \ 4) - 2)
				If iDamageDice < 1 Then iDamageDice = 1
				For iCtr = 1 To iDamageDice
					TheHero.CurrentHP -= D6()
				Next

                strMessage = resTrapExplosionMsg
                ShowExplosion(TheHero.LocX, TheHero.LocY, ConsoleColor.Red, ConsoleColor.Yellow)

				'TODO: calculate possible damage to inventory

				'TODO: calculate possible damage to adjacent enemies / allies
				iDamageDice -= 1
				If iDamageDice > 0 Then
					Dim iDam As Int16
					For iCtr = 1 To iDamageDice
						iDam += D6()
					Next

					Dim AC As Int16, intEncounter As Int16
					For iCtr = 1 To 9
						intEncounter = CheckForMonster(TheHero.LocX, TheHero.LocY, TheHero.LocZ, CType(iCtr, Integer))
						If intEncounter > 0 And intEncounter < 9999 Then
							' Determine monster AC
							AC = m_arrMonster(intEncounter).AC
							If D20() >= AC Then
								m_arrMonster(intEncounter).CurrentHP -= iDam
								If m_arrMonster(intEncounter).CurrentHP <= 0 Then
                                    strMessage &= resArticleThe & m_arrMonster(intEncounter).MonsterRace & resTrapExplosionMonsterKilled
                                End If
							End If
						End If
					Next
				End If

			Case TrapType.pit
				Dim iDepth As Integer = D6(), iCtr As Integer
				Dim iDmg As Integer

				' pit depth should not exceed 2X the current dungeon level depth
				If iDepth > TheHero.CurrentLevel * 2 Then iDepth = TheHero.CurrentLevel * 2
                strMessage = resTrapPitFall & iDepth * 10 & resMeasurementFeet
                For iCtr = 1 To iDepth
					iDmg += D6()
				Next

				'TODO: replace with actual Tumble check
				If D20() + AbilityMod(TheHero.EDexterity) > 12 + iDepth Then
					iDmg = iDmg \ 2
				End If

				' assign damage
				TheHero.CurrentHP -= iDmg

			Case TrapType.snake
                strMessage = resTrapSnakeMsg
                SummonMonster("trap")

			Case TrapType.rock
				Dim iDamageDice As Int16, iCtr As Int16
				iDamageDice = (D4() * (TheHero.TotalLevels \ 3) + (TheHero.CurrentLevel \ 4))
				If iDamageDice < 1 Then iDamageDice = 1
				For iCtr = 1 To iDamageDice
					TheHero.CurrentHP -= D6()
				Next
                strMessage = resTrapRockMsg

            Case TrapType.confusion
				ConfuseHero()
                strMessage = resTrapConfusionMsg

            Case TrapType.teleport
				TeleportHero()
                strMessage = resTrapTeleportMsg
        End Select

		' check damage to see if it killed the hero
		If TheHero.CurrentHP <= 0 Then
			TheHero.Dead = True
			TheHero.KilledBy = GetTrap(Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType)
			UpdateHPDisplay()
		End If

		Return strMessage
	End Function

	Friend Function SleepHero() As String
        With TheHero
            If D20() > .SleepResist Then
                .Sleeping = True
                .SleepDuration = 1 + D4() * D4()
                If .Sleeping And .SleepDuration > 0 Then
                    WriteAt(61, 22, resStatusSleeping)
                End If

                Return resTrapSleepResistFail
            Else
                Select Case .HeroRace
                    Case Race.Elf
                        Return resTrapSleepResistElf
                    Case Race.HalfElf
                        Return resTrapSleepResistHalfElf
                End Select
            End If
        End With

        Return ""
    End Function
	Friend Sub PoisonHero()
		With TheHero
			Dim intRoll As Int16
			intRoll = D20()
			If intRoll = 1 Or intRoll + .PoisonResist + AbilityMod(.EConstitution) < 5 + (.CurrentLevel * 2) Then
				.Poisoned = True
				.PoisonDuration += ((10 - AbilityMod(.EConstitution)) * D6()) + (D20() * D12()) * .CurrentLevel
				If .Poisoned And .PoisonDuration > 0 Then
                    WriteAt(61, 20, resStatusPoisoned, ConsoleColor.Green)
                End If
			Else
				' do nothing
			End If
		End With
	End Sub
	Friend Sub ConfuseHero()
		With TheHero
			.Confused = True
			.ConfusionDuration += 6 + D8()
			If .Confused = True And .ConfusionDuration > 0 Then
                WriteAt(61, 21, resStatusConfused)
            End If
		End With
	End Sub
	Friend Sub TeleportHero()
		Dim iX As Int16, iY As Int16, ok As Boolean = False
		Do While Not ok
			iX = RND.Next(0, MAXWIDTH)
			iY = RND.Next(0, MAXHEIGHT)
			If Level(iX, iY, TheHero.LocZ).FloorType = SquareType.Floor Then
				ok = True
				FixFloor(TheHero.LocX, TheHero.LocY, TheHero.LocZ)
				TheHero.LocX = iX
				TheHero.LocY = iY
				WriteAt(TheHero.LocX, TheHero.LocY, TheHero.Icon, TheHero.Color)
				HeroLOS()
			End If
		Loop
	End Sub

	Friend Sub SummonMonster(Optional ByVal source As String = "")
		Select Case source
			Case "trap"
				' must be vipers, since that is the only summon trap we have at the moment
				Dim vipers As Int16 = D4() + 2, intCtr As Int16, placed As Boolean
				For intCtr = 1 To vipers
					Dim v As New Viper
					placed = False
					Do While Not placed
						Dim LocX As Int16 = RND.Next(TheHero.LocX - 4, TheHero.LocX + 4)
						If LocX < 3 Then LocX = 3
						If LocX > 58 Then LocX = 58

						Dim LocY As Int16 = RND.Next(TheHero.LocY - 4, TheHero.LocY + 4)
						If LocY < 4 Then LocY = 4
						If LocY > 20 Then LocY = 20

						' monsters can't share a square with the hero
						If LocX <> TheHero.LocX Or LocY <> TheHero.LocY Then
							Dim LocZ As Int16 = TheHero.LocZ

							' monsters should only appear on a legit square
							If Level(LocX, LocY, LocZ).FloorType = SquareType.Floor _
							Or Level(LocX, LocY, LocZ).FloorType = SquareType.OpenDoor _
							Or Level(LocX, LocY, LocZ).FloorType = SquareType.StairsDn _
							Or Level(LocX, LocY, LocZ).FloorType = SquareType.StairsUp _
							Or Level(LocX, LocY, LocZ).FloorType = SquareType.Trap Then
								' make sure there's no monster in this square already
								If Level(LocX, LocY, LocZ).Monster = 0 Then
									' assign the coords to our viper and add it to the arraylist
									v.LocX = LocX : v.LocY = LocY : v.LocZ = LocZ : m_arrMonster.Add(v)
									Level(LocX, LocY, LocZ).Monster = m_arrMonster.Count
									placed = True
								End If
							End If
						End If
					Loop
				Next
				HeroLOS()
			Case Else

		End Select
	End Sub

    Friend Function CheckForTrap() As String
        Dim DieRoll As Int16

        With Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ)

            ' check to see if hero stepped on a trap
            If .TrapType > 0 Then
                ' if trap was previously discovered, check for chance to bypass
                If .TrapDiscovered Then
                    ' already discovered trap is only DC5 to bypass (DC15 in the dark)
                    Dim DC As Integer = 5
                    ' modify chance to bypass depending on lighting conditions
                    If .Illumination = IlluminationStrength.Dark Then
                        DC += 10
                    Else
                        DC -= .Illumination
                    End If

                    DieRoll = D20()
                    If (DieRoll = 1) Or (DieRoll + AbilityMod(TheHero.EDexterity) < DC) Then
                        ' Hero failed the check, set off the trap again
                        Return TriggerTrap()
                    Else
                        Return resTrapBypass & GetTrap(Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType) & resTrap
                    End If

                Else ' trap not discovered before, it MIGHT be now (1 in 8 chance - modified by luck)
                    If D8() - TheHero.Luck <= 1 Then
                        .TrapDiscovered = True
                        .FloorType = SquareType.Trap

                        ' modify chance to evade depending on lighting conditions
                        ' undiscovered trap is DC10 to detect
                        Dim DC As Integer = 10
                        If .Illumination = IlluminationStrength.Dark Then
                            DC += 10
                        Else
                            DC -= .Illumination
                        End If

                        DieRoll = D20()
                        If (DieRoll = 1) Or (DieRoll + AbilityMod(TheHero.EDexterity)) < (11 + D6() + DC) Then
                            ' hero set off trap and did not evade
                            Return TriggerTrap()
                        Else
                            ' hero set off trap but managed to evade
                            Select Case .TrapType
                                Case TrapType.sleep
                                    Return resTrapSleepEvade
                                Case TrapType.poison
                                    Return resTrapPoisonEvade
                                Case TrapType.explosion
                                    Return resTrapExplosionEvade
                                Case TrapType.pit
                                    Return resTrapPitEvade
                                Case TrapType.snake
                                    Return resTrapSnakeEvade
                                Case TrapType.rock
                                    Dim iPile As Int16, intCtr As Int16
                                    iPile = D8()
                                    For intCtr = 1 To iPile
                                        Dim aRock As New Rock
                                        .items.Add(aRock)
                                        .itemcount += 1
                                        aRock = Nothing
                                    Next
                                    Return resTrapRockEvade
                                Case TrapType.confusion
                                    Return resTrapConfusionEvade
                                Case TrapType.teleport
                                    Return resTrapTeleportEvade
                            End Select
                        End If
                    End If
                End If
            End If
        End With

        Return ""
    End Function

#End Region

#Region " Line of Sight Routines "

    Friend Function TileDescriptor(ByVal tiletype As SquareType) As String
		Select Case tiletype
			Case SquareType.Door
                Return resTileDescriptorDoor
            Case SquareType.OpenDoor
                Return resTileDescriptorOpenDoor
            Case SquareType.Floor, SquareType.Trap
                Return resTileDescriptorFloor
            Case SquareType.NECorner, SquareType.NWCorner, SquareType.SECorner, SquareType.SWCorner, SquareType.Wall, SquareType.Secret
                Return resTileDescriptorStone
            Case SquareType.Rock
                Return resTileDescriptorRock
            Case SquareType.StairsDn
                Return resTileDescriptorStairsDown
            Case SquareType.StairsUp
                Return resTileDescriptorStairsUp
            Case Else
				Return ""
		End Select
	End Function
	Friend Sub FixFloor(ByVal x As Int16, _
						ByVal y As Int16, _
						ByVal z As Int16)
		With Level(x, y, z)
			If .Illumination > 0 Then

				Select Case .FloorType
					Case SquareType.Wall, SquareType.NWCorner, SquareType.NECorner, SquareType.SECorner, _
						 SquareType.SWCorner, SquareType.Secret
						WriteAt(x, y, TileSymbol(.FloorType), ConsoleColor.DarkGray)

					Case SquareType.Floor, SquareType.StairsDn, SquareType.StairsUp
						WriteAt(x, y, TileSymbol(.FloorType), ConsoleColor.Gray)

					Case SquareType.Door, SquareType.OpenDoor
						WriteAt(x, y, TileSymbol(.FloorType), ConsoleColor.DarkYellow)

					Case SquareType.Trap
						WriteAt(x, y, TileSymbol(.FloorType), Level(x, y, z).TrapType)

					Case SquareType.Rock
						WriteAt(x, y, " ")
				End Select

				If Level(x, y, z).itemcount > 0 Then
					WriteAt(x, y, Level(x, y, z).items(0).symbol, Level(x, y, z).items(0).color)
				End If
			End If
		End With
	End Sub

#End Region

#Region " Hero Status Routines "
    Friend Function CheckForSickness(ByVal strMessage As String) As String
        'TODO: process sickness and decrement duration

        Return strMessage
    End Function

    Friend Function CheckForSleeping(ByVal strmessage As String) As String
		' if hero is sleeping, decrement duration counter
		With TheHero
			If .Sleeping And .SleepDuration > 0 Then
                strmessage &= resStatusSleepingAction
                .SleepDuration -= 1
				If .SleepDuration = 0 Then
                    WriteAt(61, 22, resStatusSleeping, ConsoleColor.Black)
                    strmessage &= resStatusSleepingWakeUp
                    .Sleeping = False
				End If
			End If
		End With

		Return strmessage
	End Function

	Friend Function CheckForInvisible(ByVal strMessage As String) As String
		' if hero is sleeping, decrement duration counter
		With TheHero
			If .Invisible And .InvisibilityDuration > 0 Then
				.Icon = "_"
				.InvisibilityDuration -= 1
				If .InvisibilityDuration = 0 Then
                    strMessage &= resStatusVisibleMsg
                    .Invisible = False
					.Icon = "@"
				End If
			End If
		End With

		Return strMessage

	End Function

    'TODO: Convert from TheHero to Avatar so Monsters can be Confused
    Friend Function CheckForConfusion(ByVal strMessage As String) As String
		' if hero is confused, decrement duration counter
		With TheHero
			If .Confused And .ConfusionDuration > 0 Then
                strMessage &= resStatusConfusedMsg
                .ConfusionDuration -= 1
				If .ConfusionDuration = 0 Then
                    WriteAt(61, 21, resStatusConfused, ConsoleColor.Black)
                    strMessage &= resStatusConfusedRecoverMsg
                    .Confused = False
				End If
			End If
		End With

		Return strMessage
	End Function

    'TODO: Convert from TheHero to Avatar so Monsters can be poisoned too.
    Friend Function CheckForPoison(ByVal strMessage As String) As String
		' if hero is poisoned, decrement duration counter
		' then roll d10 and do damage on a 1. Pump out a message and
		' also check for hero death from poisioning
		With TheHero
			If .Poisoned And .PoisonDuration > 0 Then
				If D10() = 1 Then
					.CurrentHP -= D4()
                    strMessage &= resStatusPoisonDamage
                End If

				.PoisonDuration -= 1
				If .PoisonDuration = 0 Then
                    WriteAt(61, 20, resStatusPoisoned, ConsoleColor.Black)
                    If D4() = 1 Then .PoisonResist += 1
					.Poisoned = False
				End If

				If .CurrentHP <= 0 Then
					.Dead = True
                    .KilledBy = resKilledByPoison
                    UpdateHPDisplay()
                    strMessage &= resDeathMsg
                End If
			End If
		End With

		Return strMessage
	End Function

	Friend Function CheckForHungry(ByVal strMessage As String) As String
		'TODO: implement hunger and nutrition

		Return strMessage
	End Function
#End Region

#Region " Miscellaneous Routines "

	Friend Function ElfCheckForSecret(ByVal strMessage As String) As String
		If TheHero.HeroRace = Race.Elf Then

			' if an Elf passes within 1 square of a secret door, roll a d6
			If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Secret _
				OrElse Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Secret Then

				If D6() = 1 Then
                    strMessage &= resSecretDoorElf
                End If
			End If
		End If
		Return strMessage
	End Function

	Friend Function RestartOrQuit() As Boolean
		Dim strAnswer As ConsoleKeyInfo, _
			ok As Boolean

		Do While Not ok
			Clear()

			WriteAt(1, 0, CLEARSPACE)
			WriteAt(1, 1, CLEARSPACE)
            WriteAt(1, 0, resPromptQuitOrRestart)

            strAnswer = ReadKey()

			Select Case strAnswer.KeyChar.ToString
				Case Chr(13)
					WriteAt(1, 0, CLEARSPACE)
					ok = True
					Return True
				Case "q", "Q"
					Return False
				Case "r", "R"
					WriteAt(1, 0, CLEARSPACE)
					ok = True
					Return True
			End Select
		Loop

	End Function

	Friend Sub PlaceItems(ByVal intZ As Integer)
		Debug.WriteLine("Entering PlaceItems routine")

		Dim intCtr As Int16, _
			rndX As Int16, rndY As Int16, _
			maxitems As Int16, _
			anyFailures As Int16, roll As Int16

		maxitems = RND.Next(MINITEMSPERLEVEL, MAXITEMSPERLEVEL)

		Do While intCtr <= maxitems
			rndX = RND.Next(1, MAXWIDTH)
			rndY = RND.Next(1, MAXHEIGHT)

			' make sure we place our random items in a floor tile, not a wall or stone (or door)
			If Level(rndX, rndY, intZ).FloorType = SquareType.Floor Then
				If Level(rndX, rndY, intZ).itemcount = 0 Then
					Level(rndX, rndY, intZ).items = New ArrayList
				End If

				' miscellaneous items table
				roll = D20()
				Select Case roll
					Case ItemType.Potion
						Level(rndX, rndY, intZ).items.Add(RandomPotion)
					Case ItemType.Scroll
						Level(rndX, rndY, intZ).items.Add(RandomScroll)
					Case ItemType.Tool
						Level(rndX, rndY, intZ).items.Add(RandomTool)
					Case ItemType.Armor
						Level(rndX, rndY, intZ).items.Add(RandomArmor)
					Case ItemType.Weapon
						Level(rndX, rndY, intZ).items.Add(RandomWeapon)
					Case ItemType.Helmet
						Level(rndX, rndY, intZ).items.Add(RandomHelmet)
					Case ItemType.Boots
						Level(rndX, rndY, intZ).items.Add(RandomBoots)
					Case ItemType.Shield
						Level(rndX, rndY, intZ).items.Add(RandomShield)
					Case ItemType.Neck
						Level(rndX, rndY, intZ).items.Add(RandomAmulet)

					Case ItemType.Gold
						' lets put some gold here
						Dim GP As New Gold, _
							intGold As Integer

						intGold = RND.Next(1, (20 * Abs(TheHero.CurrentLevel) * RND.Next(1, 3)))
						GP.Quantity = intGold
						Level(rndX, rndY, intZ).items.Add(GP)

					Case ItemType.Girdle
						Level(rndX, rndY, intZ).items.Add(RandomGirdle)
					Case ItemType.Cloak
						Level(rndX, rndY, intZ).items.Add(RandomCloak)
					Case ItemType.Ring
						Level(rndX, rndY, intZ).items.Add(RandomRing)
					Case ItemType.Gloves
						Level(rndX, rndY, intZ).items.Add(RandomGloves)
					Case ItemType.Bracers
						Level(rndX, rndY, intZ).items.Add(RandomBracers)
					Case ItemType.MissleWeapon
						Level(rndX, rndY, intZ).items.Add(RandomMissleWeapon)
                    Case ItemType.Missiles
                        Level(rndX, rndY, intZ).items.Add(RandomMissle)

					Case ItemType.Book
						'TODO: book code - implement later... for now just drop gold
						Dim GP As New Gold
						GP.Quantity = RND.Next(1, (20 * TheHero.CurrentLevel * RND.Next(1, 3)))
						Level(rndX, rndY, intZ).items.Add(GP)

					Case ItemType.Wand
						Level(rndX, rndY, intZ).items.Add(RandomWand)
					Case Enumerations.ItemType.Gem
						Level(rndX, rndY, intZ).items.Add(RandomGem)
				End Select

				Level(rndX, rndY, intZ).itemcount = Level(rndX, rndY, intZ).items.Count
				intCtr += 1

				Debug.WriteLine(Level(rndX, rndY, intZ).items(Level(rndX, rndY, intZ).items.Count - 1).walkover & " placed at " & rndX & "," & rndY & ".")
			Else
				' improper placement - try again
				anyFailures += 1
			End If
		Loop

		If anyFailures > 0 Then Debug.WriteLine("Tried " & anyFailures & " bad locations to place items.")
		Debug.WriteLine("leaving PlaceItems routine")
	End Sub

    <DebuggerStepThrough()>
    Friend Sub More(Optional ByVal intX As Integer = 61, _
					 Optional ByVal intY As Integer = 1)
		Dim strKeyPress As ConsoleKeyInfo
        WriteAt(intX, intY, resPromptMore, ConsoleColor.Yellow)

        strKeyPress = ReadKey(True)
		If Asc(strKeyPress.KeyChar.ToString) <> 32 Then
			More(intX, intY)
		End If
	End Sub

    <DebuggerStepThrough()>
    Friend Function LevelLookup(ByVal level As Integer) As Integer
		Return level * (level - 1) * 500
	End Function

    <DebuggerStepThrough()>
    Friend Sub PressAKey()
		CursorVisible = False
        WriteAt(1, 24, resPromptPressKeyToCont, ConsoleColor.Yellow)
        ReadKey()
	End Sub

    <DebuggerStepThrough()>
    Friend Function Top3of4(ByVal intDie1 As Integer, _
							 ByVal intDie2 As Integer, _
							 ByVal intDie3 As Integer, _
							 ByVal intDie4 As Integer) As Integer

		If (intDie1 <= intDie2) And (intDie1 <= intDie3) And (intDie1 <= intDie4) Then
			Top3of4 = intDie2 + intDie3 + intDie4
		ElseIf (intDie2 <= intDie1) And (intDie2 <= intDie3) And (intDie2 <= intDie4) Then
			Top3of4 = intDie1 + intDie3 + intDie4
		ElseIf (intDie3 <= intDie1) And (intDie3 <= intDie2) And (intDie3 <= intDie4) Then
			Top3of4 = intDie1 + intDie2 + intDie4
		ElseIf (intDie4 <= intDie1) And (intDie4 <= intDie2) And (intDie4 <= intDie3) Then
			Top3of4 = intDie1 + intDie2 + intDie3
		End If

	End Function

	Friend Function TileSymbol(ByVal Tile As SquareType) As String
		Select Case Tile
			Case SquareType.Wall, SquareType.NWCorner, SquareType.NECorner, _
				 SquareType.SECorner, SquareType.SWCorner, SquareType.Secret
				Return "#"

			Case SquareType.Floor
				Return "."

			Case SquareType.Door
				Return "+"

			Case SquareType.OpenDoor
				Return "/"

			Case SquareType.StairsUp
				Return "<"

			Case SquareType.StairsDn
				Return ">"

			Case SquareType.Trap
				Return "^"

			Case Else
				Return ""
		End Select
	End Function

    <DebuggerStepThrough()>
    Friend Sub PlaceAvatar(ByVal intDirection As Integer)
		Debug.WriteLine("Entering PlaceAvatar routine")

		' if no stairs up were generated for whatever reason
		If (m_stairsUp.x = 0 Or m_stairsUp.y = 0) _
		Or Level(m_stairsUp.x, m_stairsUp.y, TheHero.LocZ).FloorType <> SquareType.StairsUp Then
			Debug.WriteLine("Stairs Up missing or equal to 0")
			Debug.Write("Up.x = " & m_stairsUp.x & "  ")
			Debug.WriteLine("Up.y = " & m_stairsUp.y)

			Dim intX As Integer, intY As Integer, intZ As Integer, _
				ok As Boolean = False

			Do While Not ok
				intX = RND.Next(1, MAXWIDTH)
				intY = RND.Next(1, MAXHEIGHT)
				intZ = TheHero.LocZ

				If Level(intX, intY, intZ).FloorType = SquareType.Floor _
				And Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
				And Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
				And Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
				And Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor Then
					Level(intX, intY, intZ).FloorType = SquareType.StairsUp
					m_stairsUp.x = intX
					m_stairsUp.y = intY
					ok = True
					Debug.WriteLine("New Stairs Up placed")
				End If
			Loop
		End If

		' if no stairs down were generated for whatever reason
		If (m_stairsDn.x = 0 Or m_stairsDn.y = 0) _
		Or Level(m_stairsDn.x, m_stairsDn.y, TheHero.LocZ).FloorType <> SquareType.StairsDn Then
			Debug.WriteLine("Stairs Down missing or equal to 0")

			Dim intX As Integer, intY As Integer, intZ As Integer, _
				ok As Boolean = False

			Do While Not ok
				intX = RND.Next(1, MAXWIDTH)
				intY = RND.Next(1, MAXHEIGHT)
				intZ = TheHero.LocZ

				If Level(intX, intY, intZ).FloorType = SquareType.Floor _
				And Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
				And Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
				And Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
				And Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor Then
					Level(intX, intY, intZ).FloorType = SquareType.StairsDn
					m_stairsDn.x = intX
					m_stairsDn.y = intY
					ok = True
					Debug.WriteLine("New Stairs Down placed")
				End If
			Loop
		End If

		' pick the appropriate stairs based on hero's ascent or descent
		If intDirection = Direction.Down Then
			Debug.WriteLine("Hero placed at Up stairs")
			TheHero.LocX = m_stairsUp.x
			TheHero.LocY = m_stairsUp.y
		Else
			Debug.WriteLine("Hero placed at Down Stairs")
			TheHero.LocX = m_stairsDn.x
			TheHero.LocY = m_stairsDn.y
		End If

		' write the hero at that location
		WriteAt(TheHero.LocX, TheHero.LocY, TheHero.Icon, TheHero.Color)

		If Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).Illumination < IlluminationStrength.Torch Then
            WriteAt(1, 0, resPitchBlack)
        End If

		Debug.WriteLine("Exiting PlaceAvatar routine")
	End Sub

	Friend Sub PlaceMonsters(ByVal intZ As Integer, _
					 Optional ByVal MaxMonsters As Integer = 0)
		If RND Is Nothing Then RND = New DBuild.MersenneTwister

		Debug.WriteLine("entering PlaceMonsters routine")

		Dim intCtr As Integer, _
			rndX As Integer, rndY As Integer

		If MaxMonsters = 0 Then
			MaxMonsters = RND.Next(8, 12)

			' create the monster array
			'm_arrMonster = New ArrayList
			m_arrMonster.Clear()

			Debug.WriteLine("m_arrMonster.count = " & m_arrMonster.Count)
		End If
		Debug.WriteLine("MaxMonsters = " & MaxMonsters)

		intCtr = 1
		Do While intCtr <= MaxMonsters
			' randomly generate some coordinates
			rndX = RND.Next(1, MAXWIDTH)
			rndY = RND.Next(1, MAXHEIGHT)

			' monsters must be placed only on floor tiles or in open doorways
			' two monsters can never be placed in the same tile
			' monsters must be spawned 2 squares farther than the Hero can see
			'  -- except during level generation, when they can be 1 tile away
			If (Level(rndX, rndY, intZ).FloorType = SquareType.Floor _
				Or Level(rndX, rndY, intZ).FloorType = SquareType.OpenDoor) _
			And Level(rndX, rndY, intZ).Monster = 0 _
			And ( _
				  (rndX < (TheHero.LocX - TheHero.Sight) - 2 _
				   Or rndX > (TheHero.LocX + TheHero.Sight) + 2) _
				And _
				   (rndY < (TheHero.LocY - TheHero.Sight) - 2 _
				   Or rndY > (TheHero.LocY + TheHero.Sight) + 2) _
				Or _
				   MaxMonsters > 1) _
			Then
				' randomly select a monster to place, tougher monsters are near
				' the bottom of the list and should only appear after the Hero
				' has acquired a few levels...
				RandomMonster(rndX, rndY, intZ)
				Debug.WriteLine("a new " & m_arrMonster(m_arrMonster.Count - 1).monsterrace & " has been spawned")
				Level(rndX, rndY, intZ).Monster = m_arrMonster.Count
				intCtr += 1
			Else
				' improper placement - try again
				If MaxMonsters = 1 Then Debug.WriteLine("A new monster tried to spawn, but couldnt!")
			End If
		Loop

		Debug.WriteLine("leaving PlaceMonsters routine")
		Debug.WriteLine("m_arrMonster.count = " & m_arrMonster.Count)
	End Sub

    <DebuggerStepThrough()>
    Friend Sub RedrawMonsters(Optional ByVal ShowAll As Boolean = False)
		Dim intCtr As Integer

		' set these up so we can check for obstacles in the LOS
		Dim x1 As Integer, y1 As Integer, _
			x2 As Integer, y2 As Integer, Z As Integer, _
			pathX As Integer, pathY As Integer, _
			bolObstacle As Boolean = False
		' x1,y1 = monster, x2,y2 = hero

		' if the monster isn't dead, and has been observed, and is within LOS then refresh it
		For intCtr = 0 To (m_arrMonster.Count - 1)
			If m_arrMonster(intCtr).dead = False Then
				If ShowAll Then
					WriteAt(m_arrMonster(intCtr).LocX, m_arrMonster(intCtr).LocY, m_arrMonster(intCtr).Character, m_arrMonster(intCtr).Color)
				Else
					' has the monster been observed already?
					If Level(m_arrMonster(intCtr).locx, m_arrMonster(intCtr).locy, TheHero.LocZ).Observed = True Then

						' is the monster within the sight range?
						If Abs(m_arrMonster(intCtr).locx - TheHero.LocX) <= TheHero.Sight _
						And Abs(m_arrMonster(intCtr).locy - TheHero.LocY) <= TheHero.Sight Then
							x1 = m_arrMonster(intCtr).locx
							y1 = m_arrMonster(intCtr).locy
							x2 = TheHero.LocX
							y2 = TheHero.LocY
							Z = TheHero.LocZ
							pathX = x1
							pathY = y1

							' determine Monster location relative to Hero
							If x1 < x2 Then
                                If y1 < y2 Then
                                    ' NW
                                    bolObstacle = CheckLOSObstacle("NW", x1, y1, x2, y2, Z, pathX, pathY)
                                ElseIf y1 = y2 Then
                                    ' W
                                    bolObstacle = CheckLOSObstacle("W", x1, y1, x2, y2, Z, pathX, pathY)
                                ElseIf y1 > y2 Then
                                    ' SW
                                    bolObstacle = CheckLOSObstacle("SW", x1, y1, x2, y2, Z, pathX, pathY)
                                End If
                            ElseIf x1 = x2 Then
                                If y1 < y2 Then
                                    ' N
                                    bolObstacle = CheckLOSObstacle("N", x1, y1, x2, y2, Z, pathX, pathY)
                                ElseIf y1 > y2 Then
                                    ' S
                                    bolObstacle = CheckLOSObstacle("S", x1, y1, x2, y2, Z, pathX, pathY)
                                End If
                            ElseIf x1 > x2 Then
                                If y1 < y2 Then
                                    ' NE
                                    bolObstacle = CheckLOSObstacle("NE", x1, y1, x2, y2, Z, pathX, pathY)
                                ElseIf y1 = y2 Then
                                    ' E
                                    bolObstacle = CheckLOSObstacle("E", x1, y1, x2, y2, Z, pathX, pathY)
                                ElseIf y1 > y2 Then
                                    ' SE
                                    bolObstacle = CheckLOSObstacle("SE", x1, y1, x2, y2, Z, pathX, pathY)
                                End If
                            End If

                            ' obstacle not encountered, so redraw
                            If Not bolObstacle Then
                                WriteAt(m_arrMonster(intCtr).LocX, m_arrMonster(intCtr).LocY, m_arrMonster(intCtr).Character, m_arrMonster(intCtr).Color)
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Function CheckLOSObstacle(direction As String, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, Z As Integer, pathX As Integer, pathY As Integer) As Boolean
        Do Until pathX = x2 And pathY = y2
            If Level(pathX, pathY, Z).FloorType = SquareType.Wall _
            Or Level(pathX, pathY, Z).FloorType = SquareType.NWCorner _
            Or Level(pathX, pathY, Z).FloorType = SquareType.NECorner _
            Or Level(pathX, pathY, Z).FloorType = SquareType.SECorner _
            Or Level(pathX, pathY, Z).FloorType = SquareType.SWCorner _
            Or Level(pathX, pathY, Z).FloorType = SquareType.Door Then
                ' obstacle encountered
                Return True
            End If

            Select Case direction
                Case "NW"
                    pathX += 1
                    pathY += 1
                Case "W"
                    pathX += 1
                Case "SW"
                    pathX += 1
                    pathY -= 1
                Case "N"
                    pathY += 1
                Case "S"
                    pathY += 1
                Case "NE"
                    pathX -= 1
                    pathY += 1
                Case "E"
                    pathX -= 1
                Case "SE"
                    pathX -= 1
                    pathY -= 1
            End Select
        Loop

        Return False

    End Function

    Friend Sub RedrawItems(Optional ByVal All As Boolean = True)
		Dim intXCtr As Integer, _
			intYCtr As Integer

        'TODO: Get Rid of "magic" numbers
        For intYCtr = 0 To CInt(22)
			For intXCtr = 0 To CInt(60)
				' refresh items on floor
				If All Then
					If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 Then
						WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
					End If
				Else
					If Level(intXCtr, intYCtr, TheHero.LocZ).itemcount > 0 _
					And Level(intXCtr, intYCtr, TheHero.LocZ).Observed = True Then
						WriteAt(intXCtr, intYCtr, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).symbol, Level(intXCtr, intYCtr, TheHero.LocZ).items(0).Color)
					End If
				End If
			Next
		Next

	End Sub

	Friend Function CheckForCollision(ByVal intX As Int16, _
									   ByVal intY As Int16, _
									   ByVal intZ As Int16, _
									   ByVal Direction As CompassDirection) As Int16
		Dim X As Int16, Y As Int16, Z As Int16

		' normalize the direction passed in
		Select Case Direction
			Case 1
				X = intX - 1 : Y = intY + 1 : Z = intZ
			Case 2
				X = intX : Y = intY + 1 : Z = intZ
			Case 3
				X = intX + 1 : Y = intY + 1 : Z = intZ
			Case 4
				X = intX - 1 : Y = intY : Z = intZ
			Case 5
				X = intX : Y = intY : Z = intZ
			Case 6
				X = intX + 1 : Y = intY : Z = intZ
			Case 7
				X = intX - 1 : Y = intY - 1 : Z = intZ
			Case 8
				X = intX : Y = intY - 1 : Z = intZ
			Case 9
				X = intX + 1 : Y = intY - 1 : Z = intZ
		End Select

		If TheHero.InTown Then
			If X > 1 And X < 78 And Y > 0 And Y < 17 Then
				With m_TownMap(X, Y, TheHero.Town)
					Select Case .CellType
						Case HA.Towns.CellType.door, HA.Towns.CellType.wall, HA.Towns.CellType.water
							If .Observed Then
								FixTown(X, Y)
							End If
							Return .CellType
						Case Else
							' no obstacles noted
							Return 0
					End Select
				End With

			Else
				Return 8888
			End If

		ElseIf TheHero.TerrainZoom Then
			If X > 1 And X < 78 And Y > 0 And Y < 17 Then
				Return 0
			Else
				Return 8888
			End If

		ElseIf TheHero.Overland Then
			With OverlandMap(X, Y)
				Select Case .TerrainType
                    Case OverlandTerrainType.Impassable, OverlandTerrainType.Mountain, OverlandTerrainType.Water
                        FixGround(X, Y)
						Return .TerrainType
					Case Else
						' no obstacles noted
						Return 0
				End Select
			End With

		Else
			With Level(X, Y, Z)
				Select Case .FloorType
					Case SquareType.Wall, SquareType.NWCorner, SquareType.NECorner, SquareType.SECorner, SquareType.SWCorner, SquareType.Door, SquareType.Secret
						If .Observed Then
							.Illumination = IlluminationStrength.Normal
							FixFloor(X, Y, Z)
						End If
						Return .FloorType
					Case Else
						' no obstacles noted
						Return 0
				End Select
			End With
		End If
	End Function

	Friend Sub ShowExplosion(ByVal xPos As Int16, ByVal yPos As Int16, ByVal color1 As Int16, ByVal color2 As Int16)
		Dim intX As Int16, intY As Int16, boomTime As Double

		For intY = -1 To 1
			For intX = -1 To 1
				WriteAt(xPos + intX, yPos + intY, "*", color1)
			Next
		Next
		boomTime = Timer()
		Do Until Timer >= boomTime + 0.1
		Loop

		For intY = -1 To 1
			For intX = -1 To 1
				WriteAt(xPos + intX, yPos + intY, "*", color2)
			Next
		Next
		boomTime = Timer()
		Do Until Timer >= boomTime + 0.1
		Loop

		For intY = -1 To 1
			For intX = -1 To 1
				WriteAt(xPos + intX, yPos + intY, "*", ConsoleColor.Black)
			Next
		Next
		boomTime = Timer()
		Do Until Timer >= boomTime + 0.1
		Loop
	End Sub

	Friend Function CheckForMonster(ByVal intX As Int16, _
									 ByVal intY As Int16, _
									 ByVal intZ As Int16, _
									 ByVal Direction As Int16) As Int16

		Dim X As Int16, Y As Int16, Z As Int16

		' normalize the direction passed in
		Select Case Direction
			Case 1
				X = intX - 1 : Y = intY + 1 : Z = intZ
			Case 2
				X = intX : Y = intY + 1 : Z = intZ
			Case 3
				X = intX + 1 : Y = intY + 1 : Z = intZ
			Case 4
				X = intX - 1 : Y = intY : Z = intZ
			Case 6
				X = intX + 1 : Y = intY : Z = intZ
			Case 7
				X = intX - 1 : Y = intY - 1 : Z = intZ
			Case 8
				X = intX : Y = intY - 1 : Z = intZ
			Case 9
				X = intX + 1 : Y = intY - 1 : Z = intZ
		End Select

		If X = TheHero.LocX And Y = TheHero.LocY Then
			Return 9999	 ' indicating hero
		End If

		' Return whatever monster is at the passed in location, 
		' if we return a 0, then no monsters (or heroes) were noticed
		Return Level(X, Y, Z).Monster

	End Function

	Friend Function WalkoverMsg() As String
		If TheHero.InTown Then
			Select Case m_TownMap(TheHero.LocX, TheHero.LocY, TheHero.Town).CellType
				Case HA.Towns.CellType.tree
                    WalkoverMsg = resTownTree
            End Select
		ElseIf TheHero.Overland Then
			' reset the current dungeon & town labels since we are overland
			TheHero.CurrentDungeon = ""
			TheHero.Town = 0

			Select Case OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType
                Case OverlandTerrainType.Road
                    WalkoverMsg = resOverlandRoad

                Case OverlandTerrainType.Plains
                    WalkoverMsg = resOverlandPlains

                Case OverlandTerrainType.Forest
                    WalkoverMsg = resOverlandForest

                Case OverlandTerrainType.Hills
                    WalkoverMsg = resOverlandHills

                Case OverlandTerrainType.Special
                    ' add coordinates here for various special encounters
                    If TheHero.OverX = 29 And TheHero.OverY = 14 Then
                        WalkoverMsg = resOverlandDungeonForbiddenDesc
                        TheHero.CurrentDungeon = resOverlandDungeonForbidden
                    End If
					If TheHero.OverX = 21 And TheHero.OverY = 11 Then
                        WalkoverMsg = resOverlandDungeonTempleDesc
                        TheHero.CurrentDungeon = resOverlandDungeonTemple
                    End If
					If TheHero.OverX = 5 And TheHero.OverY = 10 Then
                        WalkoverMsg = resOverlandDungeonMysteriousDesc
                        TheHero.CurrentDungeon = resOverlandDungeonMysterious
                    End If
					If TheHero.OverX = 49 And TheHero.OverY = 10 Then
                        WalkoverMsg = resOverlandDungeonVolcanicDesc
                        TheHero.CurrentDungeon = resOverlandDungeonVolcanic
                    End If
					If TheHero.OverX = 41 And TheHero.OverY = 8 Then
                        WalkoverMsg = resOverlandDungeonRuinsDesc
                        TheHero.CurrentDungeon = resOverlandDungeonRuins
                    End If
					If TheHero.OverX = 11 And TheHero.OverY = 5 Then
                        WalkoverMsg = resOverlandDungeonFloodedDesc
                        TheHero.CurrentDungeon = resOverlandDungeonFlooded
                    End If

                Case OverlandTerrainType.Town
                    If TheHero.OverX = 8 And TheHero.OverY = 14 Then
                        WalkoverMsg = Chr(34) & resOverlandTownFincastle & Chr(34) & resOverlandTownFincastleDesc
                        TheHero.Town = Town.Fincastle
					End If
					If TheHero.OverX = 58 And TheHero.OverY = 14 Then
                        WalkoverMsg = Chr(34) & resOverlandTownStonegate & Chr(34) & resOverlandTownStonegateDesc
                        TheHero.Town = Town.Stonegate
					End If
					If TheHero.OverX = 25 And TheHero.OverY = 6 Then
                        WalkoverMsg = Chr(34) & resOverlandTownLakeside & Chr(34) & resOverlandTownLakesideDesc
                        TheHero.Town = Town.Lakeside
					End If
					If TheHero.OverX = 49 And TheHero.OverY = 6 Then
                        WalkoverMsg = Chr(34) & resOverlandTownSawtooth & Chr(34) & resOverlandTownSawtoothDesc
                        TheHero.Town = Town.Sawtooth
					End If
					If TheHero.OverX = 17 And TheHero.OverY = 3 Then
                        WalkoverMsg = resOverlandTownAbandonedDesc
                        TheHero.Town = Town.Abandoned
					End If

                Case OverlandTerrainType.Volcano
                    WalkoverMsg = resOverlandVolcano
                Case Else
					WalkoverMsg = ""
			End Select
		Else
			Select Case Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).FloorType
				Case SquareType.StairsUp
                    WalkoverMsg = resStairsUp

                Case SquareType.StairsDn
                    WalkoverMsg = resStairsDown

                Case SquareType.Floor, SquareType.OpenDoor, SquareType.Trap
					WalkoverMsg = ""
					Select Case Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).itemcount
						Case 1
							If Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
								If Right(Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items(0).walkover, 1) = "s" Then
									WalkoverMsg = "There are " & Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items(0).walkover & " here. "
								Else
									WalkoverMsg = "There is a"
									Select Case Left(Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items(0).walkover, 1).ToLower
										Case "a", "e", "i", "o", "u"
											WalkoverMsg &= "n "
										Case Else
											WalkoverMsg &= " "
									End Select
									WalkoverMsg &= Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items(0).walkover & " here. "
								End If
							Else
                                WalkoverMsg = resDungeonFloorItem
                            End If
						Case Is > 1
                            WalkoverMsg = resDungeonFloorItems
                    End Select

					'If Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType > 0 And Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapDiscovered Then
					'    WalkoverMsg += "You bypass the " & GetTrap(Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType) & " trap. "
					'End If

				Case Else
					WalkoverMsg = ""
			End Select
		End If
	End Function

	Friend Sub DoTurnCounter(Optional ByVal show As Boolean = False)
		Dim turnIncrement As Integer = 1
        'TODO: get rid of additional turn cost and replace it with time cost
        With TheHero
            If .Overland And Not .InTown Then
                turnIncrement = OverlandMap(.OverX, .OverY).MovementCost
            End If

            .TurnCount += turnIncrement

            If show Then
                WriteAt(68, 12, .TurnCount)
            End If
        End With

        ' TODO: This may not be the best place for this
        TimeKeeper.Update()
    End Sub

    <DebuggerStepThrough()>
    Friend Sub RedrawDisplay()
		UpdateEffectiveStats()

		If TheHero.Overland Then
			ReDrawOverlandMap()
			OverlandLOS()
			WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)
		Else
			ReDrawMap()
			DisplayStats()
			DisplayValues()
			UpdateHPDisplay()
			HeroLOS()
			RedrawMonsters()
			RedrawItems(False)
			WriteAt(TheHero.LocX, TheHero.LocY, TheHero.Icon, TheHero.Color)
		End If

	End Sub

	Friend Sub MessageHandler(ByVal strMessage As String, Optional ByVal StartY As Integer = 0, Optional ByVal Color As ConsoleColor = ConsoleColor.White)
        ' TODO: in cases where only one word is left after a line wrap in the message handler, that word is not displayed.

        ' fix spacing in the message string
        strMessage = strMessage.Replace(".", ". ")
        strMessage = strMessage.Replace(". . .", "...")
        strMessage = strMessage.Replace(",", ", ")
        strMessage = strMessage.Replace(".  ", ". ")
        strMessage = strMessage.Replace(",  ", ", ")
        strMessage = strMessage.Replace("  ", " ")

        Dim OK As Boolean = False
        Dim i As Integer = 0

        i = strMessage.IndexOf(".")
        If i > -1 Then
            While Not OK
                If strMessage.Substring(i + 1) <> " " Then
                    If i + 1 <= strMessage.Length - 3 Then
                        If strMessage.Substring(i + 1, 3) = "..." Then
                            i += 3
                        Else
                            strMessage.Insert(i + 1, " ")
                        End If
                    End If
                End If
                i = strMessage.IndexOf(".", i + 1)
                If i = -1 Then OK = True
                If i >= strMessage.Length Then OK = True
            End While
        End If

        strMessage = Trim(strMessage)

        ' add this most recent message to the queue (if it's not empty)
        If strMessage.Length > 0 Then m_qMessage.Enqueue(strMessage)

        ' if the queue contains > 15 messages, trim it down
        If m_qMessage.Count = 16 Then m_qMessage.Dequeue()

		' clear the message space at top of screen
		WriteAt(1, StartY, CLEARSPACE)
		WriteAt(1, StartY + 1, CLEARSPACE)

		Dim arrMessage() As String, sbr As New StringBuilder, _
			intWordCtr As Int16, linecount As Int16 = 0, _
			intLastWord As Int16, intTotalWords As Int16, _
			intStartWord As Int16 = 0

		' break the message up into words
		arrMessage = Split(strMessage)

		' get a word count
		intTotalWords = arrMessage.Length - 1

		' if the message is only ONE word, no need for all the 
		' other stuff below. Just display it and get out.
		If intTotalWords = 0 Then
			WriteAt(1, StartY, CLEARSPACE)
			WriteAt(1, StartY + 1, CLEARSPACE)
			WriteAt(1, StartY, arrMessage(0), Color)
			Return
		End If

		intLastWord = 0
		Do While intLastWord < intTotalWords

			For intWordCtr = intStartWord To intTotalWords
				' clear the message area
				WriteAt(1, StartY, CLEARSPACE)
				WriteAt(1, StartY + 1, CLEARSPACE)

				' build the 1st line
				If linecount = 0 And sbr.Length + arrMessage(intWordCtr).Length < 80 Then
					sbr.Append(arrMessage(intWordCtr))
					sbr.Append(" ")
					intLastWord = intWordCtr
				Else
					sbr.AppendLine()
					linecount = 1
					intLastWord = intWordCtr
					Exit For
				End If
			Next

			' show the first line
			WriteAt(1, StartY, sbr.ToString, Color)

			sbr.Remove(0, sbr.Length)
			For intWordCtr = intLastWord To intTotalWords
				' build the 2nd line
				If linecount = 1 And sbr.Length + arrMessage(intWordCtr).Length <= 70 Then
					sbr.Append(arrMessage(intWordCtr))
					sbr.Append(" ")
					intLastWord = intWordCtr
				Else
					linecount = 0
					intLastWord = intWordCtr
					intStartWord = intLastWord
					Exit For
				End If
			Next

			' show the second line
			WriteAt(1, StartY + 1, sbr.ToString, Color)
			If intLastWord < intTotalWords Then
				More(sbr.Length + 2, 1)
				sbr.Remove(0, sbr.Length)
			End If

		Loop
		sbr = Nothing

	End Sub

	Friend Function GetArticle(ByVal word As String, Optional ByVal ForceLowercase As Boolean = False) As String

		Select Case Left(word.ToLower, 1)
			Case "a", "e", "i", "o", "u"
				Return IIf(ForceLowercase, "an ", "An ")
			Case Else
				Return IIf(ForceLowercase, "a ", "A ")
		End Select

	End Function

    <DebuggerStepThrough()>
    Public Sub WriteAt(ByVal x As Integer, ByVal y As Integer, ByVal s As String, Optional ByVal ForeColor As ConsoleColor = ConsoleColor.White, Optional ByVal BackColor As ConsoleColor = ConsoleColor.Black)
        Try
            Console.ForegroundColor = ForeColor
            Console.BackgroundColor = BackColor

            Console.SetCursorPosition(x, y)
            Console.Write(s)
        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub 'WriteAt

    Friend Sub DrawLine(ByVal LinePoints As List(Of coord))
		For iCtr As Integer = 0 To LinePoints.Count - 1
			With LinePoints(iCtr)
				If Level(.x, .y, TheHero.LocZ).Observed AndAlso Level(.x, .y, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
					FixFloor(.x - 1, .y - 1, TheHero.LocZ)
					FixFloor(.x, .y - 1, TheHero.LocZ)
					FixFloor(.x + 1, .y - 1, TheHero.LocZ)

					FixFloor(.x - 1, .y, TheHero.LocZ)
					FixFloor(.x, .y, TheHero.LocZ)
					FixFloor(.x + 1, .y, TheHero.LocZ)

					FixFloor(.x - 1, .y + 1, TheHero.LocZ)
					FixFloor(.x, .y + 1, TheHero.LocZ)
					FixFloor(.x + 1, .y + 1, TheHero.LocZ)
				End If
			End With
		Next

		For iCtr As Integer = 0 To LinePoints.Count - 1
			With LinePoints(iCtr)
				'WriteAt(.x, .y, TileSymbol(Level(.x, .y, TheHero.LocZ).FloorType), ConsoleColor.White, ConsoleColor.Yellow)
				CursorLeft = LinePoints(LinePoints.Count - 1).x
				CursorTop = LinePoints(LinePoints.Count - 1).y

				If Level(.x, .y, TheHero.LocZ).Observed AndAlso Level(.x, .y, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
					WriteAt(.x, .y, "*", ConsoleColor.Yellow)
				End If
			End With
		Next
	End Sub

	Friend Function DeterminePath(ByVal StartPosition As coord, ByVal EndPosition As coord) As List(Of coord)
		Dim myPoint As coord = StartPosition ' current point
		Dim myPath As New List(Of coord) ' collection of points, making our path

		' Get the difference between 2 points
		Dim deltaX As Integer = EndPosition.x - StartPosition.x
		Dim deltaY As Integer = EndPosition.y - StartPosition.y
		Dim leftover As Integer

		' Figure out the direction based on the pos/neg value of the deltas
		Dim dirX As Integer = IIf(deltaX < 0, -1, 1)
		Dim dirY As Integer = IIf(deltaY < 0, -1, 1)

		' get the absolute value because negatives would screw up 
		' the comparison to determine which axis is longest.
		deltaX = Abs(deltaX)
		deltaY = Abs(deltaY)

		' Uncomment this to add the first point to the path (list)
		' myPath.Add(myPoint)

		' iterate through whichever axis is longest
		If deltaX > deltaY Then
			leftover = (deltaY * 2) - deltaX
			While myPoint.x <> EndPosition.x
				If leftover >= 0 Then
					myPoint.y += dirY
					leftover -= deltaX
				End If
				myPoint.x += dirX
				leftover += deltaY
				myPath.Add(myPoint)
			End While
		Else
			leftover = (deltaX * 2) - deltaY
			While myPoint.y <> EndPosition.y
				If leftover >= 0 Then
					myPoint.x += dirX
					leftover -= deltaY
				End If
				myPoint.y += dirY
				leftover += deltaX
				myPath.Add(myPoint)
			End While
		End If

		Return myPath
	End Function

#End Region


End Module
