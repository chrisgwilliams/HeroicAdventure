Imports System.Console
Imports HA.Common
Imports HA.Common.Helper
Imports HA.My.Resources.English

Module m_MainMenu

#Region " Main Menu Options "

	Friend Sub MainMenu()
		Dim Action As ConsoleKeyInfo

		Clear()

        WriteAt(34, 2, resTitle, ConsoleColor.Yellow)
        WriteAt(30, 4, resSubtitle, ConsoleColor.Red)

        WriteAt(2, 2, resMenuFirstEpisode, ConsoleColor.Gray)
        WriteAt(2, 6, resMenuDestiny, ConsoleColor.Gray)
        WriteAt(2, 7, resMenuChoices, ConsoleColor.Gray)
        WriteAt(20, 10, resMenuNew, ConsoleColor.Gray)
        WriteAt(20, 11, resMenuFAQ, ConsoleColor.Gray)
        WriteAt(20, 12, resMenuIntro, ConsoleColor.Gray)
        WriteAt(20, 13, resMenuManual, ConsoleColor.Gray)
        WriteAt(20, 14, resMenuResume, ConsoleColor.Gray)
        WriteAt(20, 15, resMenuQuit, ConsoleColor.Gray)

        WriteAt(20, 19, resMenuYourChoice, ConsoleColor.Gray)

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
        If TheHero.HeroRace = Race.HalfOrc And CheckStat = resStatStr Then Return ConsoleColor.DarkGray
        If TheHero.HeroRace = Race.HalfOrc And CheckStat = resStatCon Then Return ConsoleColor.DarkGray

        If TheHero.HeroRace = Race.Dwarf And CheckStat = resStatCon Then Return ConsoleColor.DarkGray

        If TheHero.HeroRace = Race.Ogre And CheckStat = resStatStr Then Return ConsoleColor.DarkGray
        If TheHero.HeroRace = Race.Ogre And CheckStat = resStatCon Then Return ConsoleColor.DarkGray

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

            WriteAt(25, 2, resLabelDescription, ConsoleColor.Gray)
            WriteAt(38, 2, RaceDescription())
            WriteAt(25, 4, resLabelPortrait, ConsoleColor.Gray)
            WriteAt(35, 4, "@", TheHero.Color)

            WriteAt(2, 10, resLabelStrength, ConsoleColor.Gray)
            WriteAt(16, 10, arrStat(0, 0), CheckForBaseStat("strength"))
            modColor = ConsoleColor.White
			If arrStat(0, 1) > arrStat(0, 0) Then modColor = ConsoleColor.Green
			If arrStat(0, 1) < arrStat(0, 0) Then modColor = ConsoleColor.Red
			If arrStat(0, 1) > 0 Then
				WriteAt(21, 10, arrStat(0, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(0, 1)) >= 0, 27, 26), 10, AbilityMod(arrStat(0, 1)), ConsoleColor.White)
			End If
            If TheHero.HeroRace = Race.HalfOrc Then WriteAt(32, 10, resFixedStatPlusd8, ConsoleColor.White)
            If TheHero.HeroRace = Race.Ogre Then WriteAt(32, 10, resFixedStatPlusd6, ConsoleColor.White)

            WriteAt(2, 11, resLabelIntelligence, ConsoleColor.Gray)
            WriteAt(16, 11, arrStat(1, 0), CheckForBaseStat("intelligence"))
			modColor = ConsoleColor.White
			If arrStat(1, 1) > arrStat(1, 0) Then modColor = ConsoleColor.Green
			If arrStat(1, 1) < arrStat(1, 0) Then modColor = ConsoleColor.Red
			If arrStat(1, 1) > 0 Then
				WriteAt(21, 11, arrStat(1, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(1, 1)) >= 0, 27, 26), 11, AbilityMod(arrStat(1, 1)), ConsoleColor.White)
			End If

            WriteAt(2, 12, resLabelWisdom, ConsoleColor.Gray)
            WriteAt(16, 12, arrStat(2, 0), CheckForBaseStat("wisdom"))
			modColor = ConsoleColor.White
			If arrStat(2, 1) > arrStat(2, 0) Then modColor = ConsoleColor.Green
			If arrStat(2, 1) < arrStat(2, 0) Then modColor = ConsoleColor.Red
			If arrStat(2, 1) > 0 Then
				WriteAt(21, 12, arrStat(2, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(2, 1)) >= 0, 27, 26), 12, AbilityMod(arrStat(2, 1)), ConsoleColor.White)
			End If

            WriteAt(2, 13, resLabelDexterity, ConsoleColor.Gray)
            WriteAt(16, 13, arrStat(3, 0), CheckForBaseStat("dexterity"))
			modColor = ConsoleColor.White
			If arrStat(3, 1) > arrStat(3, 0) Then modColor = ConsoleColor.Green
			If arrStat(3, 1) < arrStat(3, 0) Then modColor = ConsoleColor.Red
			If arrStat(3, 1) > 0 Then
				WriteAt(21, 13, arrStat(3, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(3, 1)) >= 0, 27, 26), 13, AbilityMod(arrStat(3, 1)), ConsoleColor.White)
			End If

            WriteAt(2, 14, resLabelConstitution, ConsoleColor.Gray)
            WriteAt(16, 14, arrStat(4, 0), CheckForBaseStat("constitution"))
			modColor = ConsoleColor.White
			If arrStat(4, 1) > arrStat(4, 0) Then modColor = ConsoleColor.Green
			If arrStat(4, 1) < arrStat(4, 0) Then modColor = ConsoleColor.Red
			If arrStat(4, 1) > 0 Then
				WriteAt(21, 14, arrStat(4, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(4, 1)) >= 0, 27, 26), 14, AbilityMod(arrStat(4, 1)), ConsoleColor.White)
			End If
            If TheHero.HeroRace = Race.Dwarf Or TheHero.HeroRace = Race.Ogre Or TheHero.HeroRace = Race.HalfOrc Then WriteAt(32, 14, resFixedStatPlusd8, ConsoleColor.White)

            WriteAt(2, 15, resLabelCharisma, ConsoleColor.Gray)
            WriteAt(16, 15, arrStat(5, 0), CheckForBaseStat("charisma"))
			modColor = ConsoleColor.White
			If arrStat(5, 1) > arrStat(5, 0) Then modColor = ConsoleColor.Green
			If arrStat(5, 1) < arrStat(5, 0) Then modColor = ConsoleColor.Red
			If arrStat(5, 1) > 0 Then
				WriteAt(21, 15, arrStat(5, 1), modColor)
				WriteAt(IIf(AbilityMod(arrStat(5, 1)) >= 0, 27, 26), 15, AbilityMod(arrStat(5, 1)), ConsoleColor.White)
			End If

            WriteAt(2, 0, resMenuGenerationTitle, ConsoleColor.Gray)

            WriteAt(4, 2, "[ ]ender: ", ConsoleColor.Gray)
            WriteAt(4, 3, "[ ]ace:  ", ConsoleColor.Gray)
            WriteAt(4, 4, "[ ]lass:  ", ConsoleColor.Gray)

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

            WriteAt(25, 3, resLabelRaceMod, ConsoleColor.Gray)
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
                    TheHero.Gender = RND.Next(Avatar.Sex.female, Avatar.Sex.male)
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
                            errMsg = resErrorOgrePixieClass

                        ElseIf .Gender > 0 AndAlso .HeroRace >= Race.Ogre AndAlso .HeroClass = 0 Then
							ok = True
							errMsg = ""
						Else
                            errMsg = resErrorMissingSelections
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
        WriteAt(8, 10, resFeatureNotComplete)

        PressAKey()
		MainMenu()
	End Sub
	Friend Sub DisplayIntro()
		Dim intY As Integer = 2, _
			intX As Integer = 5

		Clear()
        WriteAt(intX, intY, resIntroLine1, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine2, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine3, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine4, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine5, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine6, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine7, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine8, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine9, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine10, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine11, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine12, ConsoleColor.Gray)

        intY += 3
        PressAKey()

        intY = 2
        Clear()
        WriteAt(intX, intY, resIntroLine13, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine14, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine15, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine16, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine17, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine18, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine19, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine20, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine21, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine22, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine23, ConsoleColor.Gray)

        intY += 3
        WriteAt(intX, intY, resIntroLine24, ConsoleColor.Gray)
        intY += 1
        WriteAt(intX, intY, resIntroLine25, ConsoleColor.Gray)

        intY += 3
        PressAKey()

        MainMenu()
	End Sub
	Friend Sub DisplayManual()
		Clear()
        WriteAt(8, 10, resFeatureNotComplete)

        PressAKey()
		MainMenu()
	End Sub
	Friend Sub ResumeGame()
		Clear()
        WriteAt(8, 10, resFeatureNotComplete)

        PressAKey()
		MainMenu()
	End Sub

#End Region

End Module
