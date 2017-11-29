Imports DBuild.DunGen3
Imports System.Console
Imports System.Collections.Generic
Imports HA.Common
Imports HA.Screens
Imports HA.My.Resources.English

Module MainModule

#Region " Constants and Module Level Variables "
	Friend Const CLEARSPACE As String _
		= "                                                                               "

	Friend Const MINITEMSPERLEVEL As Int16 = 5
	Friend Const MAXITEMSPERLEVEL As Int16 = 10
	Friend Const MAXWIDTH As Int16 = 59
	Friend Const MAXHEIGHT As Int16 = 21

    Friend TheHero As Hero
    Friend Level(,,) As GridCell
	Friend m_arrMonster As New ArrayList
	Friend m_version As New Version("0.1.7")
	Friend RND As New DBuild.MersenneTwister

	Public m_stairsUp As coord
	Public m_stairsDn As coord

	Public m_qMessage As New Queue

#End Region

	Public Sub Main()

		TitleScreen()
		MainMenu()

		' new character created, setup world
		InitializeOverland()
        InitializeVerbLists()
        TimeKeeper.InitializeTimeKeeper()
        ItemFactory.InitializeItems()
        TownFactory.InitializeTowns()

		SetupHero()

		' start the game
		MainLoop()

		' at this point the game is over, so call various end-of-game screens
		EndScreens()

	End Sub

	Private Sub MainLoop()
		Dim gameOver As Boolean, strKey As ConsoleKeyInfo, _
			bolValid As Boolean, strMessage As String = "", _
			strCommand As String, strAutoWalkDirection As String = "5"

		Debug.WriteLine("Entering Main Game Loop")
		Do While Not gameOver
			' assume the player will press a valid key, 
			' but set it to FALSE if he doesn't
			bolValid = True

			If TheHero.AutoWalk Then
				' if the hero is autowalking, skip manual input
				strCommand = EvaluateDirection(strAutoWalkDirection)

			ElseIf TheHero.Sleeping Then
				' if the hero is asleep... we stay in place
				strCommand = 5
			Else
				strKey = ReadKey(True)
				If strKey.KeyChar = Nothing Then
					strCommand = strKey.Key.ToString
				Else
					strCommand = strKey.KeyChar.ToString
				End If
			End If

			'Sort monsters (and Hero) by initiative
			'Dim initList As List(Of Avatar) = SortByInitiative()

			' first we process the players turn... unless he's sleeping or confused
			If Not TheHero.Sleeping Then
				strMessage = ProcessKeyStroke(strCommand, strMessage, strAutoWalkDirection, bolValid, gameOver)
			Else
				strMessage = ""
				DoTurnCounter()
			End If

			' don't execute the rest of the turn unless a valid key was pressed (or hero was asleep)
			If bolValid Then

				' Process Monster and NPC Turns
				If TheHero.Overland And Not TheHero.TerrainZoom Then
					'strMessage = CheckForOverlandEncounter(strMessage)
				Else
					strMessage = MonsterAction(strMessage)
				End If

				If strMessage Is Nothing Or strMessage = "" Then
					strMessage = ""
				Else
					Debug.WriteLine("strmessage = " & strMessage)
				End If

				If Not TheHero.Dead Then
					' if hero is elf, autocheck for secret doors (1 in 6)
					If Not TheHero.Overland And Not TheHero.InTown Then
						strMessage = ElfCheckForSecret(strMessage)
					End If

					' Check for things like poison, confusion, etc...
					strMessage = TheHero.CheckStatuses(strMessage)
				End If

				' display the battle messages (if any)
				MessageHandler(strMessage)

				' uhoh, monster kills hero... 
				If TheHero.Dead Then
					Debug.WriteLine("Hero is dead.")
					More(CursorLeft + 2, CursorTop)
					gameOver = True
				Else
					' check for healing (1 HP every 35-(Con bonus * 5) turns)
					HeroHealing()

					' check for levelup
					HeroLevelUpCheck()

					If Not TheHero.Overland Then SpawnMonsters()

				End If
			Else
				' hero did something that generates a message but not a 
				' turn, like running into a wall or door...
				If strMessage.Length > 0 Then
					MessageHandler(strMessage)
				End If
			End If

		Loop

	End Sub

	Private Sub EndScreens()

		' show the entire dungeon map?
		If Not TheHero.Overland Then ShowFullMapScreen()

		' show the entire overland map?
		ShowFullOverlandMap()

		' show the message buffer
		ShowMessageBuffer()

		' show the high score
		HighScoreScreen()

		' game over - hero died
		If TheHero.Dead Then DeathScreen()
	End Sub

	Private Sub SetupHero()
		' set hero start point
		TheHero.Overland = True
		TheHero.OverX = 1
		TheHero.OverY = 16
		TheHero.Icon = "@"

		' initial map draw
		ReDrawOverlandMap()
		OverlandLOS()
		WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)

	End Sub

#Region " Dungeon Creation Subs and Functions "

	Friend Function BuildIt() As Boolean
		Dim dungeon As Builder = New Builder(60, 22, 1, 8)

		' clear the screen
		Clear()
        WriteAt(1, 1, resBuilderExcavating)

        ' call the DLL to build the level, DLL returns an array
        Try
			Level = dungeon.BuildDungeon
			BuildIt = True
		Catch ex As Exception
            WriteAt(1, 2, resErrorCaveIn)
            WriteAt(1, 24, resErrorWhereBuild & ex.Message)
            BuildIt = False
		End Try

	End Function

	Friend Sub FindStairs()
		Dim intCtrX As Integer, _
			intCtrY As Integer, _
			Z As Integer

		' just show the first level for now
		Z = TheHero.LocZ

		Clear()
		CursorVisible = False

		' go through the array of the level and find the up and down stairs
		For intCtrY = 0 To 22
			For intCtrX = 0 To 60
				Select Case Level(intCtrX, intCtrY, Z).FloorType
					Case SquareType.StairsUp
						m_stairsUp.x = intCtrX
						m_stairsUp.y = intCtrY
					Case SquareType.StairsDn
						m_stairsDn.x = intCtrX
						m_stairsDn.y = intCtrY
				End Select
			Next
		Next
	End Sub

	Friend Sub ReDrawMap()
		Dim intCtrX As Integer, _
			intCtrY As Integer, _
			Z As Integer

		' just show the first level for now
		Z = TheHero.LocZ

		Clear()
		CursorVisible = False

		' go through the array of the level and build a string
		For intCtrY = 0 To CInt(22)
			For intCtrX = 0 To CInt(60)
				With Level(intCtrX, intCtrY, Z)
					' if the room is dark then dont bother writing to the screen (it's still in the array)
					If .Illumination > IlluminationStrength.Dark _
					Or .FloorType = SquareType.StairsUp _
					Or .FloorType = SquareType.StairsDn Then

						Select Case .FloorType
							Case SquareType.Rock
								' do nothing, black/black is the default

							Case SquareType.Wall
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
								End If
							Case SquareType.NWCorner
								If .Observed = True Then
									' eventually replace with appropriate corner tile
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
								End If
							Case SquareType.NECorner
								If .Observed = True Then
									' eventually replace with appropriate corner tile
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
								End If
							Case SquareType.SECorner
								If .Observed = True Then
									' eventually replace with appropriate corner tile
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
								End If
							Case SquareType.SWCorner
								If .Observed = True Then
									' eventually replace with appropriate corner tile
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
								End If

							Case SquareType.Floor
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, ".", ConsoleColor.Gray)
								End If

							Case SquareType.Door
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, "+", ConsoleColor.DarkYellow)
								End If

							Case SquareType.Secret
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkYellow)
								End If

							Case SquareType.OpenDoor
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, "/", ConsoleColor.DarkYellow)
								End If

							Case SquareType.StairsUp
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, "<", ConsoleColor.Gray)
								End If

							Case SquareType.StairsDn
								If .Observed = True Then
									WriteAt(intCtrX, intCtrY, ">", ConsoleColor.Gray)
								End If

							Case SquareType.Trap
								If .TrapDiscovered = True Then
									WriteAt(intCtrX, intCtrY, "^", Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).TrapType)
								End If
						End Select
					End If
				End With
			Next
		Next

	End Sub

#End Region

#Region " Status Panel Subs and Functions "

    <DebuggerStepThrough()> Friend Sub DisplayStats()

		WriteAt(61, 2, "Name:", ConsoleColor.Gray)
		WriteAt(61, 3, "Class:", ConsoleColor.Gray)
		WriteAt(61, 4, "Race:", ConsoleColor.Gray)
		WriteAt(61, 6, "Str:", ConsoleColor.Gray)
		WriteAt(69, 6, "Int:", ConsoleColor.Gray)
		WriteAt(61, 7, "Wis:", ConsoleColor.Gray)
		WriteAt(69, 7, "Dex:", ConsoleColor.Gray)
		WriteAt(61, 8, "Con:", ConsoleColor.Gray)
		WriteAt(69, 8, "Cha:", ConsoleColor.Gray)
		WriteAt(61, 10, "HP:", ConsoleColor.Gray)
		WriteAt(61, 11, "XP:", ConsoleColor.Gray)
		WriteAt(61, 12, "Turns: ", ConsoleColor.Gray)

		WriteAt(61, 14, "Atk Bonus: ", ConsoleColor.Gray)

		WriteAt(61, 16, "AC: ", ConsoleColor.Gray)
		WriteAt(61, 17, "Level: ", ConsoleColor.Gray)

		WriteAt(61, 19, "D/L: ", ConsoleColor.Gray)

		With TheHero
			If .Poisoned And .PoisonDuration > 0 Then
				WriteAt(61, 21, "Poisoned", ConsoleColor.Green)
			End If

            'ToDo: Display other status messages here:  Confused, Asleep, etc

        End With
    End Sub
	Friend Sub DisplayValues()

		WriteAt(68, 2, TheHero.Name)
		WriteAt(68, 3, Helper.GetClass(TheHero.HeroClass))
		WriteAt(68, 4, Helper.GetRace(TheHero.HeroRace))

		If TheHero.EStrength = TheHero.Strength Then
			WriteAt(66, 6, TheHero.EStrength)
		ElseIf TheHero.EStrength > TheHero.Strength Then
			WriteAt(66, 6, TheHero.EStrength, ConsoleColor.Green)
		ElseIf TheHero.EStrength < TheHero.Strength Then
			WriteAt(66, 6, TheHero.EStrength, ConsoleColor.Red)
		End If

		If TheHero.EIntelligence = TheHero.Intelligence Then
			WriteAt(74, 6, TheHero.EIntelligence)
		ElseIf TheHero.EIntelligence > TheHero.Intelligence Then
			WriteAt(74, 6, TheHero.EIntelligence, ConsoleColor.Green)
		ElseIf TheHero.EIntelligence < TheHero.Intelligence Then
			WriteAt(74, 6, TheHero.EIntelligence, ConsoleColor.Red)
		End If

		If TheHero.EWisdom = TheHero.Wisdom Then
			WriteAt(66, 7, TheHero.EWisdom)
		ElseIf TheHero.EWisdom > TheHero.Wisdom Then
			WriteAt(66, 7, TheHero.EWisdom, ConsoleColor.Green)
		ElseIf TheHero.EWisdom < TheHero.Wisdom Then
			WriteAt(66, 7, TheHero.EWisdom, ConsoleColor.Red)
		End If

		If TheHero.EDexterity = TheHero.Dexterity Then
			WriteAt(74, 7, TheHero.EDexterity)
		ElseIf TheHero.EDexterity > TheHero.Dexterity Then
			WriteAt(74, 7, TheHero.EDexterity, ConsoleColor.Green)
		ElseIf TheHero.EDexterity < TheHero.Dexterity Then
			WriteAt(74, 7, TheHero.EDexterity, ConsoleColor.Red)
		End If

		If TheHero.EConstitution = TheHero.Constitution Then
			WriteAt(66, 8, TheHero.EConstitution)
		ElseIf TheHero.EConstitution > TheHero.Constitution Then
			WriteAt(66, 8, TheHero.EConstitution, ConsoleColor.Green)
		ElseIf TheHero.EConstitution < TheHero.Constitution Then
			WriteAt(66, 8, TheHero.EConstitution, ConsoleColor.Red)
		End If

		If TheHero.ECharisma = TheHero.Charisma Then
			WriteAt(74, 8, TheHero.ECharisma)
		ElseIf TheHero.ECharisma > TheHero.Charisma Then
			WriteAt(74, 8, TheHero.ECharisma, ConsoleColor.Green)
		ElseIf TheHero.ECharisma < TheHero.Charisma Then
			WriteAt(74, 8, TheHero.ECharisma, ConsoleColor.Red)
		End If

		UpdateHPDisplay()

		WriteAt(65, 11, TheHero.XP)
		WriteAt(68, 12, TheHero.TurnCount)

		If TheHero.BaseAtkBonus >= 0 Then
			WriteAt(72, 14, "+" & TheHero.BaseAtkBonus + Helper.AbilityMod(TheHero.EStrength))
		Else
			WriteAt(72, 14, TheHero.BaseAtkBonus + Helper.AbilityMod(TheHero.EStrength))
		End If

		WriteAt(65, 16, TheHero.AC)
		WriteAt(68, 17, TheHero.TotalLevels)

		WriteAt(66, 19, TheHero.CurrentDungeon & "/" & TheHero.CurrentLevel.ToString)

	End Sub
#End Region

	Private Function SortByInitiative() As List(Of Avatar)
		'********************************************************************************************************
		'Cullen's changes for sorting characters by initiative
		Debug.WriteLine("Sorting Characters/Monsters by initiative")

		'Create a List, passing in the comparer implemented in AvatarSorter.vb
		Dim initList As New List(Of Avatar)()

		'Add the hero to the list
		initList.Add(TheHero)

		'Add each monster to the list
		For Each monster As Monster In m_arrMonster
			initList.Add(monster)
		Next

		initList.Sort(New AvatarSorter)
		'A simple for loop now will give us all of the avatars, based on initiative

		'end Cullen's changes for sorting characters by initiative
		'********************************************************************************************************
		Return initList

	End Function

End Module
