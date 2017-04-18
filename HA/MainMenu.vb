Imports System.Console
Imports HA.Common
Imports HA.Common.Helper

Module m_MainMenu

#Region " Main Menu Options "

	Friend Sub MainMenu()
		Dim Action As ConsoleKeyInfo

		Clear()

		WriteAt(34, 2, "Heroic Adventure!", ConsoleColor.Yellow)

		WriteAt(30, 4, "Against the Dark Lord", ConsoleColor.Red)

		WriteAt(2, 2, "Welcome to the first episode of", ConsoleColor.Gray)
		WriteAt(2, 6, "As your destiny unfolds before you, you are presented with the first of many", ConsoleColor.Gray)
		WriteAt(2, 7, "choices.  What action shall you take?", ConsoleColor.Gray)
		WriteAt(20, 10, "[ ] create a new hero", ConsoleColor.Gray)
		WriteAt(20, 11, "[ ] read the FAQ", ConsoleColor.Gray)
		WriteAt(20, 12, "[ ] read the intro story", ConsoleColor.Gray)
		WriteAt(20, 13, "[ ] read the manual", ConsoleColor.Gray)
		WriteAt(20, 14, "[ ] resume a previous game", ConsoleColor.Gray)
		WriteAt(20, 15, "[ ] quit, before it gets too tough", ConsoleColor.Gray)

		WriteAt(20, 19, "Your choice: ", ConsoleColor.Gray)

		WriteAt(21, 10, "c")
		WriteAt(21, 11, "f")
		WriteAt(21, 12, "i")
		WriteAt(21, 13, "m")
		WriteAt(21, 14, "r")
		WriteAt(21, 15, "q")

		CursorLeft = 34
		CursorTop = 19
		CursorVisible = True

		Action = Console.ReadKey

		Select Case Action.KeyChar.ToString
			Case "c" ' create new hero
				CreateHero()
			Case "f" ' read the faq
				DisplayFAQ()
			Case "i" ' read the intro story
				DisplayIntro()
			Case "m" ' read the manual
				DisplayManual()
			Case "r" ' resume a previous game
				ResumeGame()
			Case "q" ' quit the game
				End
			Case Else
				MainMenu()
		End Select

	End Sub
	Private Function StuffStats(ByVal arrStat As Array, ByVal index As Integer) As Array

		arrStat(0, index) = TheHero.Strength
		arrStat(1, index) = TheHero.Intelligence
		arrStat(2, index) = TheHero.Wisdom
		arrStat(3, index) = TheHero.Dexterity
		arrStat(4, index) = TheHero.Constitution
		arrStat(5, index) = TheHero.Charisma

		If TheHero.HeroRace = Race.HalfOrc Then
			arrStat(0, 0) = 13
			arrStat(4, 0) = 11
		End If

		If TheHero.HeroRace = Race.Ogre Then
			arrStat(0, 0) = 15
			arrStat(4, 0) = 11
		End If

		If TheHero.HeroRace = Race.Dwarf Then
			arrStat(4, 0) = 13
		End If

		Return arrStat
	End Function

	Private Sub GrabStats(ByVal arrStat As Array, ByVal index As Integer)
		TheHero.Strength = arrStat(0, index)
		TheHero.Intelligence = arrStat(1, index)
		TheHero.Wisdom = arrStat(2, index)
		TheHero.Dexterity = arrStat(3, index)
		TheHero.Constitution = arrStat(4, index)
		TheHero.Charisma = arrStat(5, index)
	End Sub

	Private Function CheckForBaseStat(ByVal CheckStat As String) As ConsoleColor
		' these races have at least one "base" stat plus a modifier, so we gray out the base stat
		If TheHero.HeroRace = Race.HalfOrc And CheckStat = "strength" Then Return ConsoleColor.DarkGray
		If TheHero.HeroRace = Race.HalfOrc And CheckStat = "constitution" Then Return ConsoleColor.DarkGray

		If TheHero.HeroRace = Race.Dwarf And CheckStat = "constitution" Then Return ConsoleColor.DarkGray

		If TheHero.HeroRace = Race.Ogre And CheckStat = "strength" Then Return ConsoleColor.DarkGray
		If TheHero.HeroRace = Race.Ogre And CheckStat = "constitution" Then Return ConsoleColor.DarkGray

		Return ConsoleColor.White
	End Function

	Friend Sub CreateHero()
		Dim errMsg As String = ""
		Dim RaceMods As String = ""
		Dim arrStat(6, 2) As Integer

		' create a Hero object
		TheHero = New Hero

		' generate the basic stats using best 3 dice out of 4
		RollStats()
		arrStat = StuffStats(arrStat, 0)

		Dim ok As Boolean
		Do While Not ok
			Clear()
			Dim modColor As ConsoleColor

			MessageHandler(errMsg, 17, ConsoleColor.Red)

			WriteAt(2, 8, "ATTRIBUTES   DIE  MOD  BONUS  COMMENTS", ConsoleColor.White)
			WriteAt(2, 9, "-------------------------------------------------------", ConsoleColor.Gray)

			WriteAt(25, 2, "Description: ", ConsoleColor.Gray)
			WriteAt(38, 2, RaceDescription())
			WriteAt(25, 4, "Portrait:", ConsoleColor.Gray)
			WriteAt(35, 4, "@", TheHero.Color)

			WriteAt(2, 10, "Strength:", ConsoleColor.Gray)
			WriteAt(16, 10, arrStat(0, 0), CheckForBaseStat("strength"))
			modColor = ConsoleColor.White
			If arrStat(0, 1) > arrStat(0, 0) Then modColor = ConsoleColor.Green
			If arrStat(0, 1) < arrStat(0, 0) Then modColor = ConsoleColor.Red
			If arrStat(0, 1) > 0 Then
				WriteAt(21, 10, arrStat(0, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(0, 1)) >= 0, 27, 26), 10, AbilityMod(arrStat(0, 1)), ConsoleColor.White)
			End If
			If TheHero.HeroRace = Race.HalfOrc Then WriteAt(32, 10, "Base stat fixed, plus d8", ConsoleColor.White)
			If TheHero.HeroRace = Race.Ogre Then WriteAt(32, 10, "Base stat fixed, plus d6", ConsoleColor.White)

			WriteAt(2, 11, "Intelligence:", ConsoleColor.Gray)
			WriteAt(16, 11, arrStat(1, 0), CheckForBaseStat("intelligence"))
			modColor = ConsoleColor.White
			If arrStat(1, 1) > arrStat(1, 0) Then modColor = ConsoleColor.Green
			If arrStat(1, 1) < arrStat(1, 0) Then modColor = ConsoleColor.Red
			If arrStat(1, 1) > 0 Then
				WriteAt(21, 11, arrStat(1, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(1, 1)) >= 0, 27, 26), 11, AbilityMod(arrStat(1, 1)), ConsoleColor.White)
			End If

			WriteAt(2, 12, "Wisdom:", ConsoleColor.Gray)
			WriteAt(16, 12, arrStat(2, 0), CheckForBaseStat("wisdom"))
			modColor = ConsoleColor.White
			If arrStat(2, 1) > arrStat(2, 0) Then modColor = ConsoleColor.Green
			If arrStat(2, 1) < arrStat(2, 0) Then modColor = ConsoleColor.Red
			If arrStat(2, 1) > 0 Then
				WriteAt(21, 12, arrStat(2, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(2, 1)) >= 0, 27, 26), 12, AbilityMod(arrStat(2, 1)), ConsoleColor.White)
			End If

			WriteAt(2, 13, "Dexterity:", ConsoleColor.Gray)
			WriteAt(16, 13, arrStat(3, 0), CheckForBaseStat("dexterity"))
			modColor = ConsoleColor.White
			If arrStat(3, 1) > arrStat(3, 0) Then modColor = ConsoleColor.Green
			If arrStat(3, 1) < arrStat(3, 0) Then modColor = ConsoleColor.Red
			If arrStat(3, 1) > 0 Then
				WriteAt(21, 13, arrStat(3, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(3, 1)) >= 0, 27, 26), 13, AbilityMod(arrStat(3, 1)), ConsoleColor.White)
			End If

			WriteAt(2, 14, "Constitution:", ConsoleColor.Gray)
			WriteAt(16, 14, arrStat(4, 0), CheckForBaseStat("constitution"))
			modColor = ConsoleColor.White
			If arrStat(4, 1) > arrStat(4, 0) Then modColor = ConsoleColor.Green
			If arrStat(4, 1) < arrStat(4, 0) Then modColor = ConsoleColor.Red
			If arrStat(4, 1) > 0 Then
				WriteAt(21, 14, arrStat(4, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(4, 1)) >= 0, 27, 26), 14, AbilityMod(arrStat(4, 1)), ConsoleColor.White)
			End If
			If TheHero.HeroRace = Race.Dwarf Or TheHero.HeroRace = Race.Ogre Or TheHero.HeroRace = Race.HalfOrc Then WriteAt(32, 14, "Base stat fixed, plus d8", ConsoleColor.White)

			WriteAt(2, 15, "Charisma:", ConsoleColor.Gray)
			WriteAt(16, 15, arrStat(5, 0), CheckForBaseStat("charisma"))
			modColor = ConsoleColor.White
			If arrStat(5, 1) > arrStat(5, 0) Then modColor = ConsoleColor.Green
			If arrStat(5, 1) < arrStat(5, 0) Then modColor = ConsoleColor.Red
			If arrStat(5, 1) > 0 Then
				WriteAt(21, 15, arrStat(5, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(5, 1)) >= 0, 27, 26), 15, AbilityMod(arrStat(5, 1)), ConsoleColor.White)
			End If

			WriteAt(2, 0, "Character Generation Menu: ", ConsoleColor.Gray)

			WriteAt(4, 2, "[g]ender: ", ConsoleColor.Gray)
			WriteAt(4, 3, "[r]ace:  ", ConsoleColor.Gray)
			WriteAt(4, 4, "[c]lass:  ", ConsoleColor.Gray)

			WriteAt(5, 2, "g")
			WriteAt(5, 3, "r")
			WriteAt(5, 4, "c")

			WriteAt(2, 20, "Your choice (r[e]roll, [k]eep, z for previous menu, ! for quickselect): ", ConsoleColor.Gray)
			WriteAt(17, 20, "e", ConsoleColor.White)
			WriteAt(26, 20, "k", ConsoleColor.White)
			WriteAt(33, 20, "z", ConsoleColor.White)
			WriteAt(54, 20, "!", ConsoleColor.Yellow)

			WriteAt(15, 2, GetGender(TheHero.Gender))
			WriteAt(15, 3, GetRace(TheHero.HeroRace))
			WriteAt(15, 4, GetClass(TheHero.HeroClass))

			WriteAt(25, 3, "Race Mod:", ConsoleColor.Gray)
			RaceMods = GetRaceMods(TheHero.HeroRace)
			WriteAt(35, 3, RaceMods, ConsoleColor.Yellow)

			CursorLeft = 74
			CursorTop = 20

			Dim Action As ConsoleKeyInfo
			Action = Console.ReadKey

			Select Case Action.KeyChar.ToString
				Case "g"
					GenderMenu()
					errMsg = ""

				Case "c"
					Dim oldClass As PCClass = TheHero.HeroClass


					'reset stats to die rolls
					GrabStats(arrStat, 0)

					' adjust stats +- based on race (because race HAS to be first)
					AdjustStatsForRace()

					' pick the class now
					errMsg = ClassMenu()

					If TheHero.HeroClass <> oldClass Then

						' set the color of the '@' avatar
						SetAvatarColor()

						'update display array
						arrStat = StuffStats(arrStat, 1)
					End If

				Case "r"
					Dim oldRace As Race = TheHero.HeroRace
					RaceMenu()
					errMsg = ""

					' in case we enter the race menu and reselect the same race or hit Z
					' this is to prevent from re-gen'ing the race bonuses
					If TheHero.HeroRace <> oldRace Then
						'reset stats to die rolls
						GrabStats(arrStat, 0)

						' adjust stats +- based on race
						AdjustStatsForRace()

						'update display array
						arrStat = StuffStats(arrStat, 1)
					End If

				Case "!"
					TheHero.Gender = RND.Next(Gender.female, Gender.male)
					TheHero.HeroRace = RND.Next(Race.Human, Race.Pixie)
					If TheHero.HeroRace < Race.Ogre Then
						TheHero.HeroClass = RND.Next(PCClass.Warrior, PCClass.Paladin)
					Else
						TheHero.HeroClass = PCClass.None
					End If

					WriteAt(17, 2, GetGender(TheHero.Gender) & "           ")
					WriteAt(17, 3, GetRace(TheHero.HeroRace) & "           ")
					WriteAt(17, 4, GetClass(TheHero.HeroClass) & "           ")

					errMsg = ""

					' generate the basic stats using best 3 dice out of 4
					RollStats()
					arrStat = StuffStats(arrStat, 0)

					' reroll certain prime stats based on class (scaled UP)
					'AdjustStatsForClass()

					' adjust stats +- based on race
					AdjustStatsForRace()
					'AlreadyAdjusted = True

					'update display array
					arrStat = StuffStats(arrStat, 1)

					' set effective stats equal to real stats initially
					UpdateEffectiveStats()

					' set the color of the '@' avatar
					SetAvatarColor()

				Case "z"
					MainMenu()

				Case "e"
					' generate the basic stats using best 3 dice out of 4
					RollStats()
					arrStat = StuffStats(arrStat, 0)

					' adjust stats +- based on race
					AdjustStatsForRace()

					' reroll certain prime stats based on class (scaled UP)
					AdjustStatsForClass()

					'update display array
					arrStat = StuffStats(arrStat, 1)

				Case "k"
					With TheHero
						If .Gender > 0 AndAlso .HeroRace > 0 AndAlso .HeroClass > 0 Then
							ok = True
							errMsg = ""

							'reset stats to modded rolls
							GrabStats(arrStat, 1)

						ElseIf .HeroRace >= Race.Ogre AndAlso .HeroClass > 0 Then
							errMsg = "Ogres and Pixies may not start with a Class."

						ElseIf .Gender > 0 AndAlso .HeroRace >= Race.Ogre AndAlso .HeroClass = 0 Then
							ok = True
							errMsg = ""
						Else
							errMsg = "You must select Gender, Race & Class before proceeding."
						End If
					End With
			End Select


			' calculate energy based on race/class modified stats
			CalculateEnergy()
			' roll HP (max at first level) and add Con bonus
			CalculateHP()
			' give the Hero some armor and a shield based on race and class
			AssignGear()
			' assign skills based on class and race
			AssignSkills()
			' look at all the factors and compute the final AC
			CalculateAC()

			' add effects/blessings/curses from birth sign
			'TODO: StarSigns()

		Loop

		' give your character a name
		NameChar()


	End Sub
	Friend Sub DisplayFAQ()
		Clear()
		WriteAt(8, 10, "I'm sorry, this feature has not been completed yet.")

		PressAKey()
		MainMenu()
	End Sub
	Friend Sub DisplayIntro()
		Dim intY As Integer = 2, _
			intX As Integer = 5

		Clear()
		WriteAt(intX, intY, "In ages past, before the rise of modern", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "man and all his technological miracles...", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "There existed a world of magic and wonder.", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "A place where mighty sorcerors and noble", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "warriors walked the lands in search of", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "adventure.", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "With these adventures, many battles were", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "fought. Good clashed with Evil, and great", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "kingdoms on both sides trembled and fell", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "only to be rebuilt anew.", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "But one day a great evil rose and all the", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "kingdoms of the land fell to darkness...", ConsoleColor.Gray)
		intY += 3

		PressAKey()
		intY = 2

		Clear()
		WriteAt(intX, intY, "Ancient prophecies foretold the fall, but", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "the world of man was not without hope.", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "The prophecy spoke of a hero. A child born", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "unto the world with a curious mark on his", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "person, possessed of great courage, would", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "one day rise to overthrow the dark lord.", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "But the dark lord was not afraid. Twice a", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "year, he sent out his inquisitors to every", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "kingdom with orders to inspect all the", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "newborn children, and return with any who", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "bore a mark.", ConsoleColor.Gray)
		intY += 3

		WriteAt(intX, intY, "For 300 years, the dark lord has ruled the", ConsoleColor.Gray)
		intY += 1
		WriteAt(intX, intY, "land unchallenged...", ConsoleColor.Gray)
		intY += 3

		PressAKey()

		MainMenu()
	End Sub
	Friend Sub DisplayManual()
		Clear()
		WriteAt(8, 10, "I'm sorry, this feature has not been completed yet.")

		PressAKey()
		MainMenu()
	End Sub
	Friend Sub ResumeGame()
		Clear()
		WriteAt(8, 10, "I'm sorry, this feature has not been completed yet.")

		PressAKey()
		MainMenu()
	End Sub

#End Region

End Module
