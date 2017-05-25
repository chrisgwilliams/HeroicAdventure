Imports HA.Common
Imports HA.Common.Helper
Imports System.Console

Module m_Generation

#Region " Character Creation Subs and Functions "

	Friend Sub GenderMenu()

		Clear()

		WriteAt(2, 1, "Choose your gender: ", ConsoleColor.Gray)
		WriteAt(10, 2, "[ ] male      [ ] female    [ ] random", ConsoleColor.Gray)
		WriteAt(10, 3, "Your choice (z for previous menu): ", ConsoleColor.Gray)

		WriteAt(11, 2, "m")
		WriteAt(25, 2, "f")
		WriteAt(39, 2, "*")
		WriteAt(23, 3, "z")

		CursorLeft = 45
		CursorTop = 3
		CursorVisible = True

		Select Case ReadKey().KeyChar.ToString()
			Case "m"
				TheHero.Gender = Gender.male
			Case "f"
				TheHero.Gender = Gender.female
			Case "*"
				TheHero.Gender = RND.Next(Gender.female, Gender.male)
			Case "z" ' previous menu
				Return
			Case Else
				GenderMenu()
		End Select

	End Sub
	Friend Sub RaceMenu()
		Clear()

		WriteAt(2, 1, "Choose a race: ", ConsoleColor.Gray)
		WriteAt(10, 2, "[ ] human     [ ] elf       [ ] half-elf", ConsoleColor.Gray)
		WriteAt(10, 3, "[ ] dwarf     [ ] gnome     [ ] halfling", ConsoleColor.Gray)
		WriteAt(10, 4, "[ ] halforc   [ ] ogre      [ ] pixie", ConsoleColor.Gray)
		WriteAt(10, 5, "                            [ ] random", ConsoleColor.Gray)
		WriteAt(10, 7, "Your choice (z for previous menu): ", ConsoleColor.Gray)

		WriteAt(11, 2, "a")
		WriteAt(25, 2, "b")
		WriteAt(39, 2, "c")
		WriteAt(11, 3, "d")
		WriteAt(25, 3, "e")
		WriteAt(39, 3, "f")
		WriteAt(11, 4, "g")
		WriteAt(25, 4, "h")
		WriteAt(39, 4, "i")
		WriteAt(39, 5, "*")
		WriteAt(23, 7, "z")

		CursorLeft = 45
		CursorTop = 7
		CursorVisible = True

		Select Case ReadKey().KeyChar.ToString
			Case "a" ' human
				TheHero.HeroRace = Race.Human
			Case "b" ' elf
				TheHero.HeroRace = Race.Elf
			Case "c" ' half-elf
				TheHero.HeroRace = Race.HalfElf
			Case "d" ' dwarf
				TheHero.HeroRace = Race.Dwarf
			Case "e" ' gnome
				TheHero.HeroRace = Race.Gnome
			Case "f" ' halfling
				TheHero.HeroRace = Race.Halfling
			Case "g" ' halforc
				TheHero.HeroRace = Race.HalfOrc
			Case "h" ' ogre
				TheHero.HeroRace = Race.Ogre
			Case "i" ' centaur
				TheHero.HeroRace = Race.Pixie
			Case "*" ' random
				TheHero.HeroRace = RND.Next(Race.Human, Race.Pixie)
			Case "z" ' previous menu
				Return
			Case Else
				RaceMenu()
		End Select

	End Sub
	Friend Function ClassMenu() As String
		Clear()
		'TODO: ENHANCEMENT! Add a "Defiler" class that derives it's "energy" for spells/attacks from killing

		ClassMenu = ""

		If TheHero.HeroRace < Race.Ogre Then
			WriteAt(2, 1, "Choose a class: ", ConsoleColor.Gray)
			WriteAt(10, 2, "[ ] Warrior   [ ] Paladin   [ ] Barbarian", ConsoleColor.Gray)
			WriteAt(10, 3, "[ ] Wizard    [ ] Sorceror  [ ] Thief", ConsoleColor.Gray)
			WriteAt(10, 4, "[ ] Assassin  [ ] Priest    [ ] Druid", ConsoleColor.Gray)
			WriteAt(10, 5, "                            [ ] random", ConsoleColor.Gray)

			WriteAt(10, 7, "Your choice (z for previous menu): ", ConsoleColor.Gray)

			WriteAt(11, 2, "a")
			WriteAt(25, 2, "b")
			WriteAt(39, 2, "c")
			WriteAt(11, 3, "d")
			WriteAt(25, 3, "e")
			WriteAt(39, 3, "f")
			WriteAt(11, 4, "g")
			WriteAt(25, 4, "h")
			WriteAt(39, 4, "i")
			WriteAt(39, 5, "*")
			WriteAt(23, 7, "z")

			CursorLeft = 45
			CursorTop = 7
			CursorVisible = True

			Select Case ReadKey().KeyChar.ToString
				Case "a" ' warrior
					TheHero.HeroClass = PCClass.Warrior
				Case "b" ' paladin
					TheHero.HeroClass = PCClass.Paladin
				Case "c" ' barbarian
					TheHero.HeroClass = PCClass.Barbarian
				Case "d" ' wizard
					TheHero.HeroClass = PCClass.Wizard
				Case "e" ' sorceror
					TheHero.HeroClass = PCClass.Sorceror
				Case "f" ' thief
					TheHero.HeroClass = PCClass.Thief
				Case "g" ' assassin
					TheHero.HeroClass = PCClass.Assassin
				Case "h" ' priest
					TheHero.HeroClass = PCClass.Priest
				Case "i" ' druid
					TheHero.HeroClass = PCClass.Druid
				Case "*" ' random
					TheHero.HeroClass = RND.Next(PCClass.Warrior, PCClass.Paladin)
				Case "z" ' previous menu
					Return ""
				Case Else
					ClassMenu()
			End Select

			AdjustStatsForClass()


		ElseIf TheHero.HeroRace = Race.Ogre _
			Or TheHero.HeroRace = Race.Pixie Then

			Return "Due to their special abilities, young " & GetRace(TheHero.HeroRace) & "s start without a class. You will select a class at maturity."
		End If

	End Function
	Friend Sub NameChar()

		WriteAt(2, 22, "Give yourself a name: ", ConsoleColor.Gray)
		CursorLeft = 24
		CursorTop = 22

		TheHero.Name = ReadLine()
		TheHero.Name = Left(TheHero.Name, 11)

        If TheHero.Name = "" Then
            Select Case D6()
                Case 1
                    TheHero.Name = "Pat"
                Case 2
                    TheHero.Name = "Robin"
                Case 3
                    TheHero.Name = "Tinky Winky"
                Case 4
                    TheHero.Name = "Butt Ugly"
                Case 5
                    TheHero.Name = "Troll Bait"
                Case 6
                    TheHero.Name = "Ineeda Name"
            End Select
        End If

    End Sub
	Friend Sub RollStats()

		' roll the dice for all attributes (the Top3of4 function takes the highest 3 rolls)
		TheHero.Strength = Top3of4(D6(), D6(), D6(), D6())
		TheHero.Intelligence = Top3of4(D6(), D6(), D6(), D6())
		TheHero.Wisdom = Top3of4(D6(), D6(), D6(), D6())
		TheHero.Dexterity = Top3of4(D6(), D6(), D6(), D6())
		TheHero.Constitution = Top3of4(D6(), D6(), D6(), D6())
		TheHero.Charisma = Top3of4(D6(), D6(), D6(), D6())

	End Sub
	Friend Sub AdjustStatsForClass()

		With TheHero
			' scale for the relevant class
			Select Case TheHero.HeroClass
				Case PCClass.Warrior
					If .Strength < 15 Then .Strength = D4() + 14
					If .Dexterity < 15 Then .Dexterity = D4() + 14
					.HitDie = 10
					.BaseAtkBonus = 1
					.CasterType = MagicType.None

				Case PCClass.Barbarian
					If .Strength < 15 Then .Strength = D4() + 14
					If .Constitution < 15 Then .Constitution = D4() + 14
					.HitDie = 12
					.BaseAtkBonus = 1
					.CasterType = MagicType.None

				Case PCClass.Paladin
					If .Strength < 15 Then .Strength = D4() + 14
					If .Charisma < 15 Then .Charisma = D4() + 14
					If .Wisdom < 15 Then .Wisdom = D4() + 14
					.HitDie = 10
					.BaseAtkBonus = 1
					.CasterType = MagicType.None

				Case PCClass.Wizard
					If .Intelligence < 15 Then .Intelligence = D4() + 14
					.HitDie = 4
					.BaseAtkBonus = 0
					.CasterType = MagicType.Arcane

				Case PCClass.Sorceror
					If .Charisma < 15 Then .Charisma = D4() + 14
					.HitDie = 4
					.BaseAtkBonus = 0
					.CasterType = MagicType.Arcane

				Case PCClass.Thief
					If .Dexterity < 15 Then .Dexterity = D4() + 14
					If .Intelligence < 15 Then .Intelligence = D4() + 14
					.HitDie = 6
					.BaseAtkBonus = 0
					.CasterType = MagicType.None

				Case PCClass.Assassin
					If .Dexterity < 15 Then .Dexterity = D4() + 14
					If .Constitution < 15 Then .Constitution = D4() + 14
					.HitDie = 6
					.BaseAtkBonus = 0
					.CasterType = MagicType.None

				Case PCClass.Priest
					If .Wisdom < 15 Then .Wisdom = D4() + 14
					.HitDie = 8
					.BaseAtkBonus = 0
					.CasterType = MagicType.Divine

				Case PCClass.Druid
					If .Wisdom < 15 Then .Wisdom = D4() + 14
					.HitDie = 8
					.BaseAtkBonus = 0
					.CasterType = MagicType.Divine
			End Select

			.TotalLevels = 1
		End With

	End Sub
	Friend Sub AdjustStatsForRace()

		With TheHero
			' adjust stats based on racial tendancies
			' in some cases we override the "best 3 of 4" method for
			' races that should have an exceptionally high stat
			Select Case TheHero.HeroRace
				Case Race.Human
					' no mods

				Case Race.Elf
					.Dexterity += 2
					.Constitution -= 2
					.SleepResist = 18
					.CharmResist = 18

				Case Race.HalfElf
					.SleepResist = 8
					.CharmResist = 8

				Case Race.Dwarf
					.Constitution = 13 + D8()
					.Charisma -= 2
					.DarkVision = 6

				Case Race.Gnome
					.Constitution += 2
					.Strength -= 2

				Case Race.Halfling
					.Dexterity += 2
					.Strength -= 2

				Case Race.HalfOrc
					.Strength = 13 + D8()
					.Strength += 2
					.Intelligence -= 2
					.Constitution = 11 + D8()
					.Charisma -= 2
					.DarkVision = 6

				Case Race.Ogre
					.Strength = 15 + D6()
					.Intelligence -= 4
					.Constitution = 11 + D8()
					.Charisma -= 4
					.NaturalArmor += 3
					.MiscACMod += 0
					.DarkVision = 6
					.HitDie = 8
					.BaseAtkBonus = 0

				Case Race.Pixie
					.Strength -= 4
					.Dexterity += 4
					.Intelligence += 2
					.Charisma += 2
					.NaturalArmor += 0
					.MiscACMod += 1
					.DarkVision = 0
					.HitDie = 6
					.BaseAtkBonus = 1

			End Select
		End With
	End Sub
	Friend Function GetRaceMods(ByVal rRace As Race) As String
		Dim RaceMods As String = ""

		Select Case rRace
			Case Race.Elf
				RaceMods = "Dex: +2, Con: -2"
			Case Race.Dwarf
				RaceMods = "Con: 13+d8, Cha: -2"
			Case Race.Gnome
				RaceMods = "Con: +2, Str: -2"
			Case Race.Halfling
				RaceMods = "Dex: +2, Str: -2"
			Case Race.HalfOrc
				RaceMods = "Str: 13+d8, Int: -2, Con: 11+d8, Cha: -2"
			Case Race.Ogre
				RaceMods = "Str: 15+d6, Int: -4, Con: 11+d8, Cha: -4"
			Case Race.Pixie
				RaceMods = "Str: -4, Dex: +4, Int: +2, Cha: +2"
			Case Else
				RaceMods = ""
		End Select

		Return RaceMods
	End Function
	Friend Sub CalculateEnergy()
		' this determines the base energy level for the Hero,
		' based on constitution (as modified by race and class)
		Dim energy As Integer
		energy = 400
		energy = 100 + (energy * (5 + AbilityMod(TheHero.Constitution)))
		TheHero.Energy = energy
		TheHero.CurrentEnergy = energy

	End Sub
	Friend Sub CalculateHP()

		If TheHero.TotalLevels = 1 Then
			TheHero.HP = TheHero.HitDie + AbilityMod(TheHero.Constitution)
		Else
			TheHero.HP += RND.Next(1, TheHero.HitDie + 1) + AbilityMod(TheHero.Constitution)
		End If

		TheHero.CurrentHP = TheHero.HP

	End Sub
	Friend Sub AssignGear()

		With TheHero
			Select Case .HeroRace
				Case Race.Human
					' add food
					Dim apple As New Apple
					apple.Quantity = 3
					.Equipped.BackPack.Add(apple)

					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New StuddedLeather
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New Club
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New LeatherHelmet
						Case PCClass.Barbarian
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New Club
							.Equipped.Boots = New FurBoots
							.Equipped.Helmet = New FurHat
						Case PCClass.Paladin
							.Equipped.Armor = New BreastPlate
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LongSword
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Sorceror
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Priest
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightMace
							.Equipped.Boots = New ChainBoots
							.Equipped.Helmet = New ChainHood
						Case PCClass.Druid
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New LightHammer
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New ShortSword
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Assassin
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New ShortSword
							.Equipped.Boots = New LeatherBoots
					End Select

				Case Race.Elf
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New ElvenChain
							.Equipped.RightHand = New ShortSpear
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Barbarian
							.Equipped.Armor = New StuddedLeather
							.Equipped.RightHand = New ShortSpear
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New LeatherHelmet
						Case PCClass.Paladin
							.Equipped.Armor = New ElvenChain
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LongSword
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New ChainHood
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Sorceror
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Priest
							.Equipped.Armor = New StuddedLeather
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightMace
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Druid
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New LightHammer
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Scimitar
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Assassin
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Dagger
							.Equipped.Boots = New LeatherBoots
					End Select

				Case Race.HalfElf
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New StuddedLeather
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New ShortSpear
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Barbarian
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New ShortSpear
							.Equipped.Boots = New FurBoots
							.Equipped.Helmet = New FurHat
						Case PCClass.Paladin
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LongSword
							.Equipped.Boots = New ChainBoots
							.Equipped.Helmet = New ChainHood
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Sorceror
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Priest
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightMace
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Druid
							.Equipped.Armor = New StuddedLeather
							.Equipped.RightHand = New HandAxe
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Dagger
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Assassin
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Dagger
							.Equipped.Boots = New LeatherBoots
					End Select

				Case Race.Dwarf
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New BreastPlate
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New Warhammer
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New HornedHelmet
						Case PCClass.Barbarian
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New Warhammer
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New HornedHelmet
						Case PCClass.Paladin
							.Equipped.Armor = New BreastPlate
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New Warhammer
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New HornedHelmet
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
							.Equipped.Helmet = New SkullCap
						Case PCClass.Sorceror
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
							.Equipped.Helmet = New SkullCap
						Case PCClass.Priest
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New Warhammer
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New ChainHood
						Case PCClass.Druid
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New LightMace
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New FurHat
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New ShortSword
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Assassin
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New ShortSword
							.Equipped.Boots = New LeatherBoots
					End Select

				Case Race.Gnome
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New Hide
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New ShortSword
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Barbarian
							.Equipped.Armor = New Furs
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightHammer
							.Equipped.Boots = New FurBoots
							.Equipped.Helmet = New FurHat
						Case PCClass.Paladin
							.Equipped.Armor = New ScaleMail
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New BattleAxe
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New ScaleHelm
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
							.Equipped.Helmet = New SkullCap
						Case PCClass.Sorceror
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Quarterstaff
							.Equipped.Boots = New Sandles
						Case PCClass.Priest
							.Equipped.Armor = New StuddedLeather
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightMace
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New LeatherHelmet
						Case PCClass.Druid
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Club
							.Equipped.Boots = New LeatherBoots
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Dagger
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New LeatherHelmet
						Case PCClass.Assassin
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Dagger
							.Equipped.Boots = New LeatherBoots
					End Select

				Case Race.Halfling
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New Leather
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New ShortSword
						Case PCClass.Barbarian
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New ShortSpear
						Case PCClass.Paladin
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New ShortSword
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
						Case PCClass.Sorceror
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Quarterstaff
						Case PCClass.Priest
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New LightMace
						Case PCClass.Druid
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New LightHammer
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Dagger
						Case PCClass.Assassin
							.Equipped.Armor = New Clothing
							.Equipped.RightHand = New Dagger
					End Select

				Case Race.HalfOrc
					Select Case .HeroClass
						Case PCClass.Warrior
							.Equipped.Armor = New Hide
							.Equipped.RightHand = New BattleAxe
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New OrcishHelm
						Case PCClass.Barbarian
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New GreatClub
							.Equipped.Boots = New FurBoots
							.Equipped.Helmet = New OrcishHelm
						Case PCClass.Paladin
							.Equipped.Armor = New ChainShirt
							.Equipped.LeftHand = New LargeShield
							.Equipped.RightHand = New Greatsword
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New OrcishHelm
						Case PCClass.Wizard
							.Equipped.Armor = New Robes
							.Equipped.RightHand = New Quarterstaff
						Case PCClass.Sorceror
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Quarterstaff
						Case PCClass.Priest
							.Equipped.Armor = New Hide
							.Equipped.LeftHand = New SmallShield
							.Equipped.RightHand = New MorningStar
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New OrcishHelm
						Case PCClass.Druid
							.Equipped.Armor = New Furs
							.Equipped.RightHand = New Warhammer
							.Equipped.Boots = New FurBoots
						Case PCClass.Thief
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New LongSword
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New OrcishHelm
						Case PCClass.Assassin
							.Equipped.Armor = New Leather
							.Equipped.RightHand = New Rapier
							.Equipped.Boots = New LeatherBoots
							.Equipped.Helmet = New OrcishHelm
					End Select

				Case Race.Ogre
					.Equipped.Armor = New ChainShirt
					.Equipped.LeftHand = New LargeShield
					.Equipped.RightHand = New GreatClub
					.Equipped.Boots = New LeatherBoots
					.Equipped.Helmet = New ChainHood

				Case Race.Pixie
					.Equipped.Armor = New Hide
					.Equipped.RightHand = New Dagger

			End Select
		End With
	End Sub

	Private Sub ImproveSkill(ByVal skill As String)
		' if the hero has this skill, add 1 rank
		' otherwise add the skill to the list (at 0 ranks)
		With TheHero
			If HasSkill(skill) >= 0 Then
				.Skills(skill) += 1
			Else
				.Skills.Add(skill, 1)
			End If
		End With
	End Sub
	Friend Function HasSkill(ByVal skill As String) As Int16

		With TheHero
			If .Skills.ContainsKey(skill) Then
				Return .Skills(skill)
			Else
				Return -1
			End If
		End With

	End Function
	Friend Sub AssignSkills()
		With TheHero

			Select Case .HeroRace
				Case Race.Human
				Case Race.Elf
					ImproveSkill("Search")
					ImproveSkill("Awareness")
					ImproveSkill("Hide")
				Case Race.HalfElf
					ImproveSkill("Search")
					ImproveSkill("Awareness")
					ImproveSkill("Hide")
				Case Race.Dwarf
					ImproveSkill("Mining")
				Case Race.Gnome
					ImproveSkill("Literacy")
				Case Race.Halfling
					ImproveSkill("Hide")
				Case Race.HalfOrc
					ImproveSkill("Survival")
				Case Race.Ogre
					ImproveSkill("Healing")
				Case Race.Pixie
					ImproveSkill("Hide")
			End Select

			Select Case .HeroClass
				Case PCClass.Warrior
					ImproveSkill("Find Weakness")
					ImproveSkill("Swimming")

				Case PCClass.Wizard
					ImproveSkill("Literacy")
					ImproveSkill("Concentration")
					ImproveSkill("Spellcraft")

				Case PCClass.Thief
					ImproveSkill("Pick Pockets")
					ImproveSkill("Pick Locks")
					ImproveSkill("Disable Trap")
					ImproveSkill("Awareness")
					ImproveSkill("Backstab")
					ImproveSkill("Appraise")
					ImproveSkill("Hide")
					ImproveSkill("Find Weakness")

				Case PCClass.Priest
					ImproveSkill("Concentration")
					ImproveSkill("Healing")
					ImproveSkill("Spellcraft")
					ImproveSkill("Literacy")

				Case PCClass.Barbarian
					ImproveSkill("Climbing")
					ImproveSkill("Swimming")
					ImproveSkill("Survival")

				Case PCClass.Sorceror
					ImproveSkill("Literacy")
					ImproveSkill("Concentration")
					ImproveSkill("Spellcraft")

				Case PCClass.Assassin
					ImproveSkill("Backstab")
					ImproveSkill("Pick Locks")
					ImproveSkill("Hide")
					ImproveSkill("Find Weakness")

				Case PCClass.Druid
					ImproveSkill("Literacy")
					ImproveSkill("Concentration")
					ImproveSkill("Spellcraft")
					ImproveSkill("Healing")

				Case PCClass.Paladin
					ImproveSkill("Concentration")
					ImproveSkill("Healing")
					ImproveSkill("Spellcraft")
					ImproveSkill("Literacy")
					ImproveSkill("Find Weakness")
					ImproveSkill("Swimming")

			End Select
		End With
	End Sub
	Friend Sub CalculateAC()
		With TheHero
			.AC = 10
			.AC += .NaturalArmor
			.AC += AbilityMod(.Dexterity)
			.AC += .MiscACMod
			If Not .Equipped.Helmet Is Nothing Then
				.AC += .Equipped.Helmet.acbonus
			End If
			If Not .Equipped.Neck Is Nothing Then
				If .Equipped.Neck.acbonus > 0 Then
					.AC += .Equipped.Neck.acbonus
				End If
			End If
			If Not .Equipped.Cloak Is Nothing Then
				.AC += .Equipped.Cloak.acbonus
			End If
			If Not .Equipped.Armor Is Nothing Then
				.AC += .Equipped.Armor.acbonus
			End If
			If Not .Equipped.LeftHand Is Nothing Then
				If .Equipped.LeftHand.type = ItemType.Shield Then .AC += .Equipped.LeftHand.acbonus
			End If
			If Not .Equipped.RightHand Is Nothing Then
				If .Equipped.RightHand.type = ItemType.Shield Then .AC += .Equipped.RightHand.acbonus
			End If
			If Not .Equipped.LeftRing Is Nothing Then
				.AC += .Equipped.LeftRing.acbonus
			End If
			If Not .Equipped.RightRing Is Nothing Then
				.AC += .Equipped.RightRing.acbonus
			End If
			If Not .Equipped.Gloves Is Nothing Then
				.AC += .Equipped.Gloves.acbonus
			End If
			If Not .Equipped.Bracers Is Nothing Then
				.AC += .Equipped.Bracers.acbonus
			End If
			If Not .Equipped.Boots Is Nothing Then
				.AC += .Equipped.Boots.acbonus
			End If
		End With
	End Sub
	Friend Sub SetAvatarColor(Optional ByVal AvatarColor As Integer = -1)
		With TheHero
			If AvatarColor >= 0 Then
				' if we explicitly pass in a color, use that one (most likely a magical effect)
				' otherwise assign color to class
				.Color = AvatarColor
			Else
				Select Case .HeroClass
					Case PCClass.Warrior
						.Color = ConsoleColor.Gray
					Case PCClass.Barbarian
						.Color = ConsoleColor.DarkGreen
					Case PCClass.Paladin
						.Color = ConsoleColor.White
					Case PCClass.Wizard
						.Color = ConsoleColor.Blue
					Case PCClass.Sorceror
						.Color = ConsoleColor.Magenta
					Case PCClass.Priest
						.Color = ConsoleColor.Cyan
					Case PCClass.Druid
						.Color = ConsoleColor.Green
					Case PCClass.Thief
						.Color = ConsoleColor.DarkYellow
					Case PCClass.Assassin
						.Color = ConsoleColor.DarkGray
					Case Else
						Select Case .HeroRace
							Case Race.Ogre
								.Color = ConsoleColor.Red
							Case Race.Pixie
								.Color = ConsoleColor.DarkCyan
						End Select
				End Select
			End If
		End With
	End Sub

	Friend Function RaceDescription() As String
		Select Case TheHero.HeroRace
			Case Race.Human
				Return "A simple race with much potential."
			Case Race.Elf
				Return "Long-lived and good with magic."
			Case Race.HalfElf
				Return "Resourceful and quick. Some magic."
			Case Race.Dwarf
				Return "Sturdy as the stone. Devout folk."
			Case Race.Gnome
				Return "A curious race. Very studious."
			Case Race.Halfling
				Return "Fun loving, but sneaky. Quick too."
			Case Race.HalfOrc
				Return "Strong, proud, excellent warriors."
			Case Race.Ogre
				Return "Dumb as a post, a killing machine."
			Case Race.Pixie
				Return "Fast, magical, but oh so fragile."
			Case Else
				Return ""
		End Select
	End Function

#End Region


End Module
