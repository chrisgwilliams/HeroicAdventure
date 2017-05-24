Imports HA.Common
Imports DBuild.DunGen3
Imports System.Text
Imports System.Console
Imports System.Text.RegularExpressions
Imports HA.Common.Helper
Imports HA.Screens
Imports HA.My.Resources.English


Module m_Keybound

#Region " Key Processing"
    Friend Function ProcessKeyStroke(ByVal strCommand As String, _
									 ByVal strMessage As String, _
									 ByRef strAutoWalkDirection As String, _
									 ByRef bolValid As Boolean, _
									 ByRef gameOver As Boolean) As String

		Select Case strCommand	' main loop for keybound commands
			Case "?" ' display keybindings
				kbShowKeyBindings()
				bolValid = False
				strMessage = ""

			Case "," ' pick something up
				If TheHero.Confused Then
					'flip coin for right/left, back/forward
					Dim x As Integer = RND.Next(-1, 1)
					Dim y As Integer = RND.Next(-1, 1)
                    strMessage = kbGet(TheHero.LocX + x, TheHero.LocY + y, TheHero.LocZ) + resPerhaps
                Else
					strMessage = kbGet(TheHero.LocX, TheHero.LocY, TheHero.LocZ)
				End If

				DoTurnCounter()

			Case "w" ' walk until danger or door

				' I think what we really need here is a queued command property off the  
				' Hero class that we can check and if there is anything in there, use it 
				' instead of checking for a keypress.

				strMessage = kbWalk()

				WriteAt(1, 0, CLEARSPACE)
                MessageHandler(resPromptSelectDirection)
                CursorLeft = 34
				CursorTop = 0
				CursorVisible = True

				Dim OK As Boolean = False

				Dim WalkDirection As ConsoleKeyInfo = Console.ReadKey(True)
				If WalkDirection.KeyChar = Nothing Then
					strAutoWalkDirection = WalkDirection.Key.ToString
				Else
					strAutoWalkDirection = WalkDirection.KeyChar.ToString
				End If

				Do Until OK
					Select Case strAutoWalkDirection
						Case 1 To 9	' SW, S, SE, W, WAIT, E, NW, N, NE
							TheHero.AutoWalk = True
							strCommand = EvaluateDirection(strAutoWalkDirection)
							bolValid = True
                            strMessage = resWalking
                            OK = True

                        Case "UpArrow", "DownArrow", "LeftArrow", "RightArrow"
                            Select Case strAutoWalkDirection
                                Case "UpArrow"
                                    strAutoWalkDirection = 8
                                Case "DownArrow"
                                    strAutoWalkDirection = 2
                                Case "LeftArrow"
                                    strAutoWalkDirection = 4
                                Case "RightArrow"
                                    strAutoWalkDirection = 6
                            End Select

                            TheHero.AutoWalk = True
                            strCommand = EvaluateDirection(strAutoWalkDirection)
                            bolValid = True
                            strMessage = resWalking
                            OK = True

                        Case Else
                            TheHero.AutoWalk = False
                            strMessage = ""
                            OK = True
                    End Select

                Loop
                CursorVisible = False

            Case "UpArrow", "DownArrow", "LeftArrow", "RightArrow", 1 To 9
                ' move/attack in the specified direction
                ' if hero walks into a wall or door, it shouldn't count 
                ' as an action, so set Valid to False and don't allow a 
                ' monster action
                Select Case strCommand
                    Case "UpArrow"
                        strCommand = 8
                    Case "DownArrow"
                        strCommand = 2
                    Case "LeftArrow"
                        strCommand = 4
                    Case "RightArrow"
                        strCommand = 6
                End Select

                If TheHero.Confused Then
                    strCommand = RND.Next(1, 9)
                End If

                strMessage = ""
                bolValid = kbDirection(strCommand, strMessage)
                If strMessage.Length > 0 Then
                    Debug.WriteLine("strmessage = " & strMessage)
                End If

            Case "a" ' apply skills
                bolValid = False
                If TheHero.Confused Then
                    strMessage = resConfusedDefault
                Else
                    strMessage = kbSkills(bolValid)
                End If

            Case "c" ' close the door
                If TheHero.Confused Then
                    strMessage = resConfusedCloseDoor
                Else
                    strMessage = kbClose()
                End If
                DoTurnCounter()

            Case "C" ' chat with someone or something
                If TheHero.Confused Then
                    strMessage = resConfusedChat
                Else
                    strMessage = kbChat()
                End If
                DoTurnCounter()

            Case "d" ' drop something
                'TODO: small chance of dropping EVERYTHING or wrong item in backpack if attempting to drop an item while confused.
                strMessage = kbDrop()

            Case "D" ' drink something
                strMessage = kbDrink()
                DoTurnCounter()

            Case "i" ' show inventory screen
                If TheHero.Confused Then
                    strMessage = resConfusedInventory
                Else
                    kbInventory()
                    strMessage = ""
                    bolValid = False
                End If

            Case "k" ' kick something
                'confusion is handled in the kbKick() routine
                strMessage = kbKick()
                DoTurnCounter()

            Case "l" ' look at a tile or monster
                kbLook()
                strMessage = ""
                bolValid = False

            Case "o" ' open the door
                If TheHero.Confused Then
                    strMessage = resConfusedOpenDoor
                Else
                    strMessage = kbOpen()
                    If Not TheHero.Overland Then
                        HeroLOS()
                    ElseIf TheHero.InTown Then
                        TownLOS(TheHero.Town)
                    Else
                        OverlandLOS()
                    End If
                End If

                DoTurnCounter()

            Case "Q" ' quit the game
                If kbQuit() Then
                    gameOver = True
                End If
                bolValid = False

            Case "@" ' show character info
                bolValid = False
                HeroDisplay()
                strMessage = ""

            Case "s" ' search immediate area
                If TheHero.Confused Then
                    strMessage = resConfusedSearch
                Else
                    strMessage = kbSearch()
                End If
                DoTurnCounter()

            Case "t" ' ranged combat
                If TheHero.Confused Then
                    strMessage = resConfusedAim
                Else
                    strMessage = kbTarget()
                End If
                DoTurnCounter()

            Case "z" ' cast a spell
                If TheHero.Confused Then
                    strMessage = resConfusedCast
                Else
                    strMessage = kbSpellCast()
                End If
                DoTurnCounter()

            Case "Z" ' use a wand
                If TheHero.Confused Then
                    strMessage = resConfusedZap
                Else
                    strMessage = kbUseWand()
                End If
                DoTurnCounter()

            Case ">" ' descend 1 level (DoTurnCounter is included in kbDown sub)
                strMessage = kbDown()
            Case "<" ' ascend 1 level (DoTurnCounter is included in kbUp sub)
                strMessage = kbUp()
            Case "^" ' for testing only
                TheHero.XP += 500
                UpdateXPDisplay()
                strMessage = ""
            Case Else 'invalid keypress
                bolValid = False
                strMessage = ""
        End Select
        Return strMessage
    End Function
#End Region

#Region " KeyBound Commands "

    Friend Function kbDirection(ByVal strKey As String, ByRef strMessage As String) As Boolean
        Dim Collision As Integer = 0,
            Encounter As Integer = 0

        If Not TheHero.Overland Then ' must be in a dungeon...
            Collision = CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, CType(strKey, Integer))
            Encounter = CheckForMonster(TheHero.LocX, TheHero.LocY, TheHero.LocZ, CType(strKey, Integer))
        ElseIf TheHero.TerrainZoom Then
            Collision = CheckForCollision(TheHero.LocX, TheHero.LocY, OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType, CType(strKey, Integer))
            Encounter = 0
        ElseIf TheHero.InTown Then
            Collision = CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.Town, CType(strKey, Integer))
            Encounter = 0
        Else ' overland, not in town or zoomed into a region...
            Collision = CheckForCollision(TheHero.OverX, TheHero.OverY, 0, CType(strKey, Integer))
            Encounter = 0
        End If

        If Collision = 0 And Encounter = 0 Then
            If Not TheHero.Overland Then
                If Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).Illumination > IlluminationStrength.Dark Then
                    FixFloor(TheHero.LocX, TheHero.LocY, TheHero.LocZ)
                Else
                    WriteAt(TheHero.LocX, TheHero.LocY, " ")
                    strMessage = resPitchBlack
                End If
            ElseIf TheHero.TerrainZoom Then
                FixTerrain(TheHero.LocX, TheHero.LocY, OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType)
            ElseIf TheHero.InTown Then
                FixTown(TheHero.LocX, TheHero.LocY)
            Else
                FixGround(TheHero.OverX, TheHero.OverY)
            End If

            TheHero.Walk(CType(strKey, Integer))

            ' make sure cursor is normally off, only turning on when hero is invisible.
            Console.CursorVisible = False
            Console.CursorLeft = TheHero.LocX + 1
            Console.CursorTop = TheHero.LocY + 3

            If Not TheHero.Overland Then                                             ' we're in a dungeon
                ' check to see if hero stepped on a trap
                strMessage += CheckForTrap()
                If TheHero.Invisible Then
                    Console.CursorLeft = TheHero.LocX
                    Console.CursorTop = TheHero.LocY
                    Console.CursorVisible = True
                Else
                    WriteAt(TheHero.LocX, TheHero.LocY, TheHero.Icon, TheHero.Color)
                End If
            ElseIf TheHero.TerrainZoom Then                                          ' we're on a terrain map
                If TheHero.Invisible Then
                    Console.CursorVisible = True
                Else
                    WriteAt(TheHero.LocX + 1, TheHero.LocY + 3, TheHero.Icon, TheHero.Color)
                End If
            ElseIf TheHero.InTown Then                                               ' we're in town
                If TheHero.Invisible Then
                    Console.CursorVisible = True
                Else
                    WriteAt(TheHero.LocX + 1, TheHero.LocY + 3, TheHero.Icon, TheHero.Color)
                End If
            Else                                                                    ' we're overland
                If TheHero.Invisible Then
                    Console.CursorLeft = TheHero.OverX + 1
                    Console.CursorTop = TheHero.OverY + 3
                    Console.CursorVisible = True
                Else
                    WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)
                End If
            End If

            ' see if the @ is stepping over any items
            If strKey <> 5 Then
                If Not TheHero.Sleeping Then
                    strMessage += WalkoverMsg()
                Else
                    strMessage += resSnoring
                End If
            End If

            ' hero took a step, so increment the turn counter
            DoTurnCounter()
            kbDirection = True

        ElseIf Collision > 0 Then
            If Not TheHero.Overland Then
                Select Case Collision
                    Case SquareType.Wall, SquareType.NECorner, SquareType.NWCorner, SquareType.SECorner, SquareType.SWCorner
                        strMessage = resCollideWall
                    Case SquareType.Door
                        strMessage = resCollideDoor
                    Case SquareType.Secret
                        strMessage = resCollideWall
                End Select

            ElseIf TheHero.TerrainZoom Then
                If Collision = 8888 Then
                    WriteAt(1, 0, CLEARSPACE)
                    WriteAt(1, 1, CLEARSPACE)
                    WriteAt(1, 0, resPromptReturnToWilderness)

                    Dim ok As Boolean
                    Do While Not ok
                        Dim Answer As ConsoleKeyInfo
                        Answer = ReadKey()
                        Select Case Answer.KeyChar.ToString
                            Case Chr(13), "y", "Y"
                                TheHero.TerrainZoom = False
                                ReDrawOverlandMap()
                                WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)
                                ok = True

                            Case "n", "N"
                                WriteAt(1, 0, CLEARSPACE)
                                WriteAt(1, 1, CLEARSPACE)
                                ok = True
                            Case Else
                                WriteAt(1, 0, CLEARSPACE)
                                WriteAt(1, 1, CLEARSPACE)
                                WriteAt(1, 0, resPromptReturnToWilderness)
                                ok = False
                        End Select
                    Loop
                Else
                    Select Case Collision
                        Case HA.Towns.CellType.water
                            'TODO: Check for swim skill
                            strMessage = resNoSwimmingYet
                        Case HA.Towns.CellType.wall
                            strMessage = resCollideWall
                        Case HA.Towns.CellType.door
                            strMessage = resCollideDoor
                    End Select
                End If

            ElseIf TheHero.InTown Then
                If Collision = 8888 Then
                    WriteAt(1, 0, CLEARSPACE)
                    WriteAt(1, 1, CLEARSPACE)
                    WriteAt(1, 0, resPromptReturnToWilderness)

                    Dim ok As Boolean
                    Do While Not ok
                        Dim Answer As ConsoleKeyInfo
                        Answer = ReadKey()
                        Select Case Answer.KeyChar.ToString
                            Case Chr(13), "y", "Y"
                                TheHero.InTown = False
                                TheHero.Town = -1
                                ReDrawOverlandMap()
                                WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)
                                ok = True

                            Case "n", "N"
                                WriteAt(1, 0, CLEARSPACE)
                                WriteAt(1, 1, CLEARSPACE)
                                ok = True
                            Case Else
                                WriteAt(1, 0, CLEARSPACE)
                                WriteAt(1, 1, CLEARSPACE)
                                WriteAt(1, 0, resPromptReturnToWilderness)
                                ok = False
                        End Select
                    Loop
                Else
                    Select Case Collision
                        Case HA.Towns.CellType.water
                            strMessage = resNoSwimmingYet
                        Case HA.Towns.CellType.wall
                            strMessage = resCollideWall
                        Case HA.Towns.CellType.door
                            strMessage = resCollideDoor
                    End Select
                End If
            Else
                Select Case Collision
                    Case TerrainType.Impassable
                        strMessage = resImpassable
                    Case TerrainType.Mountain
                        'TODO: Add check for climbing gear
                        strMessage = resNoClimbingYet
                    Case TerrainType.Water
                        'TODO: Add check for swimming skill
                        strMessage = resNoSwimmingYet
                End Select
            End If
            ' running into a wall does NOT increment the turn counter, 
            ' or cause monster actions
            kbDirection = False

        ElseIf Encounter > 0 Then
            Encounter -= 1

            ' you just walked into a monster of some sort
            Debug.WriteLine("Encounter: " & Encounter)

            ' walking into a monster is the same as attacking
            strMessage = HeroAttack(Encounter) & " "

            ' hero attacked something, so increment the turn counter and return a valid action
            DoTurnCounter()
            kbDirection = True
        End If

        ' Hero Line of Sight
        If TheHero.InTown Then
            TownLOS(TheHero.Town)
        ElseIf TheHero.TerrainZoom Then
            TerrainLOS(OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType)
        ElseIf TheHero.Overland Then
            OverlandLOS()
        Else
            HeroLOS()
        End If

    End Function
    <DebuggerStepThrough()> Friend Function kbOpen() As String
        Dim ok As Boolean,
            strAnswer As ConsoleKeyInfo

        ' autodetect door to open if only 1 adjacent 
        ' or ask "which door" if multiple doors are present
        Dim doorCount As Integer,
            qDoorQueue As New Queue

        doorCount = 0

        If Not TheHero.Overland Then
            ' iterate through compass, looking for doors
            If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(7)
            End If

            If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(8)
            End If

            If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(9)
            End If

            If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(4)
            End If

            If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(6)
            End If

            If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(1)
            End If

            If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(2)
            End If

            If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door Then
                doorCount += 1
                qDoorQueue.Enqueue(3)
            End If
        ElseIf TheHero.InTown Then
            ' iterate through compass, looking for doors
            If m_TownMap(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(7)
            End If

            If m_TownMap(TheHero.LocX, TheHero.LocY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(8)
            End If

            If m_TownMap(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(9)
            End If

            If m_TownMap(TheHero.LocX - 1, TheHero.LocY, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(4)
            End If

            If m_TownMap(TheHero.LocX + 1, TheHero.LocY, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(6)
            End If

            If m_TownMap(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(1)
            End If

            If m_TownMap(TheHero.LocX, TheHero.LocY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(2)
            End If

            If m_TownMap(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then
                doorCount += 1
                qDoorQueue.Enqueue(3)
            End If
        Else
            doorCount = 0
        End If

        kbOpen = ""
        Select Case doorCount
            Case 0
                kbOpen = resThereIsNoDoor
            Case 1
                kbOpen = OpenDoor(CType(qDoorQueue.Dequeue, Integer))
            Case Else ' more than 1 door present
                ok = False
                Do While Not ok
                    WriteAt(1, 0, CLEARSPACE)
                    WriteAt(1, 0, resPromptWhichDirection)
                    CursorLeft = 19
                    CursorTop = 0

                    CursorVisible = True
                    strAnswer = ReadKey()
                    If IsNumeric(strAnswer.KeyChar.ToString) Then
                        kbOpen = OpenDoor(CType(strAnswer.KeyChar.ToString, Integer))
                        ok = True
                        CursorVisible = False
                    End If
                Loop

                WriteAt(1, 0, CLEARSPACE)
        End Select

        qDoorQueue = Nothing

    End Function
    <DebuggerStepThrough()> Friend Function kbClose() As String
        Dim strAnswer As ConsoleKeyInfo,
        ok As Boolean

        ' write code to autodetect door to close if only 1 adjacent 
        ' or ask "which door" if multiple doors are present
        Dim doorCount As Integer,
            qDoorQueue As New Queue

        If Level Is Nothing Then
            Return resThereIsNoDoor
        End If

        doorCount = 0
        kbClose = ""

        ' iterate through compass, looking for doors
        If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(7)
        End If

        If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(8)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(9)
        End If

        If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(4)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(6)
        End If

        If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(1)
        End If

        If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(2)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor Then
            doorCount += 1
            qDoorQueue.Enqueue(3)
        End If

        Select Case doorCount
            Case 0
                kbClose = resThereIsNoDoor
            Case 1
                kbClose = CloseDoor(CType(qDoorQueue.Dequeue, Integer))
            Case Else ' more than 1 door present
                ok = False
                Do While Not ok
                    WriteAt(1, 0, CLEARSPACE)
                    WriteAt(1, 0, resPromptWhichDirection)
                    CursorLeft = 19
                    CursorTop = 0
                    CursorVisible = True
                    strAnswer = ReadKey()
                    If IsNumeric(strAnswer) Then
                        kbClose = CloseDoor(CType(strAnswer.KeyChar.ToString, Integer))
                        ok = True
                        CursorVisible = False
                    End If
                Loop
                WriteAt(1, 0, CLEARSPACE)
        End Select

        qDoorQueue = Nothing
    End Function
    <System.Diagnostics.DebuggerStepThrough()> Friend Function kbChat() As String ' talk to someone / something
        Dim ok As Boolean,
            strAnswer As ConsoleKeyInfo

        If TheHero.Overland Then
            Return resNobodyToChat
        End If

        ' autodetect monster or npc to chat with if only 1 adjacent 
        ' or ask "which direction" if multiple creatures are present
        Dim CreatureCount As Integer,
            qCreatureQueue As New Queue

        kbChat = ""
        CreatureCount = 0

        ' iterate through compass, looking for creatures
        If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(7)
        End If

        If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(8)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(9)
        End If

        If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(4)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(6)
        End If

        If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(1)
        End If

        If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(2)
        End If

        If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
            CreatureCount += 1
            qCreatureQueue.Enqueue(3)
        End If

        Select Case CreatureCount
            Case 0
                kbChat = resNobodyToChat
            Case 1
                kbChat = ChatCreature(CType(qCreatureQueue.Dequeue, Integer)) & " "
            Case Else ' more than 1 creature present
                ok = False
                Do While Not ok
                    WriteAt(1, 0, CLEARSPACE)
                    WriteAt(1, 0, resPromptWhichDirection)
                    CursorLeft = 19
                    CursorTop = 0
                    CursorVisible = True
                    strAnswer = ReadKey()
                    If IsNumeric(strAnswer.KeyChar.ToString) Then
                        kbChat = ChatCreature(CType(strAnswer.KeyChar.ToString, Integer)) & " "
                        ok = True
                        CursorVisible = False
                    End If
                Loop

                WriteAt(1, 0, CLEARSPACE)
        End Select

        qCreatureQueue = Nothing

    End Function
    <DebuggerStepThrough()> Friend Function kbDrink() As String
        kbDrink = ShowBackpack(ActionType.Drink, Enumerations.ItemType.Potion)
        RedrawDisplay()
    End Function
    Friend Function kbDrop() As String
        kbDrop = ShowBackpack(ActionType.Drop)
        If kbDrop = "9999" Then kbDrop = ""
        RedrawDisplay()
    End Function
    Friend Function kbGet(ByVal intX As Integer,
                           ByVal intY As Integer,
                           ByVal intZ As Integer) As String

        Dim strMessage As String = "", strAnswer As ConsoleKeyInfo,
            OK As Boolean

        If Level Is Nothing Then
            Return resNothingToPickup
        End If

        If Not Level(intX, intY, intZ).items Is Nothing Then
            If Level(intX, intY, intZ).items.Count > 1 Then
                MultipleItemList(intX, intY, intZ)

                ' Use regular expressions to validate input
                Dim ValidInput As Match

                Do While Not OK
                    strAnswer = ReadKey()
                    ValidInput = Regex.Match(strAnswer.KeyChar.ToString, "[a-z]")

                    ' if they entered an appropriate value, then continue
                    If ValidInput.Success Then
                        If strAnswer.KeyChar.ToString = "z" Then
                            OK = True
                        Else
                            Dim intItemIndex As Integer = Asc(strAnswer.KeyChar.ToString) - Asc("a")

                            If intItemIndex > Level(intX, intY, intZ).itemcount - 1 Then
                                ' do nothing
                            Else
                                Dim item As ItemBase = Level(intX, intY, intZ).items(intItemIndex)
                                TheHero.Equipped.BackPack.Add(item)
                                TheHero.BackpackWeight += item.Weight

                                Level(intX, intY, intZ).items.RemoveAt(intItemIndex)
                                Level(intX, intY, intZ).itemcount -= 1
                                If Level(intX, intY, intZ).itemcount = 0 Then
                                    Level(intX, intY, intZ).itemcount = Nothing
                                    OK = True
                                Else
                                    MultipleItemList(intX, intY, intZ)
                                End If
                            End If
                        End If
                    End If
                Loop

                RedrawDisplay()

                ' there is 1 item laying here, so pick it up
            ElseIf Level(intX, intY, intZ).items.Count = 1 Then
                strMessage = resYouPickUp
                strMessage += Level(intX, intY, intZ).items(0).walkover & ". "

                Dim item As Object, AlreadyInBackpack As Boolean = False
                For Each item In TheHero.Equipped.BackPack
                    If item.walkover = Level(intX, intY, intZ).items(0).walkover Then
                        item.quantity += 1
                        AlreadyInBackpack = True
                    End If
                Next

                ' if the item we're picking up is NOT already in our pack, then add it
                If Not AlreadyInBackpack Then
                    TheHero.Equipped.BackPack.Add(Level(intX, intY, intZ).items(0))
                End If

                Level(intX, intY, intZ).items.RemoveAt(0)
                Level(intX, intY, intZ).itemcount -= 1
                If Level(intX, intY, intZ).itemcount = 0 Then Level(intX, intY, intZ).itemcount = Nothing

            Else
                strMessage = resNothingToPickup
            End If
        Else
            strMessage = resNothingToPickup
        End If
        Debug.WriteLine("strmessage = " & strMessage)

        Return strMessage
    End Function
    Friend Sub kbInventory()
        Dim strAnswer As ConsoleKeyInfo,
            OK As Boolean

        InventoryScreen()

        Do While Not OK
            strAnswer = ReadKey(True)

            Select Case strAnswer.KeyChar.ToString
                Case "a"
                    If Not TheHero.Equipped.Helmet Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Helmet.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Helmet)
                        End If

                        TheHero.Equipped.Helmet.Deactivate(TheHero)
                        TheHero.Equipped.Helmet = Nothing

                        InventoryScreen()
                    Else
                        TheHero.Equipped.Helmet = ShowBackpack(ActionType.None, ItemType.Helmet)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Helmet Is Nothing Then
                        TheHero.Equipped.Helmet.Activate(TheHero)
                    End If

                Case "b"
                    If Not TheHero.Equipped.Neck Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Neck.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Neck)
                        End If

                        TheHero.Equipped.Neck.Deactivate(TheHero)
                        TheHero.Equipped.Neck = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Neck = ShowBackpack(ActionType.None, ItemType.Neck)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Neck Is Nothing Then
                        TheHero.Equipped.Neck.Activate(TheHero)
                    End If

                Case "c"
                    If Not TheHero.Equipped.Cloak Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Cloak.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Cloak)
                        End If

                        TheHero.Equipped.Cloak.Deactivate(TheHero)
                        TheHero.Equipped.Cloak = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Cloak = ShowBackpack(ActionType.None, ItemType.Cloak)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Cloak Is Nothing Then
                        TheHero.Equipped.Cloak.Activate(TheHero)
                    End If

                Case "d"
                    If Not TheHero.Equipped.Girdle Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Girdle.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Girdle)
                        End If

                        TheHero.Equipped.Girdle.Deactivate(TheHero)
                        TheHero.Equipped.Girdle = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Girdle = ShowBackpack(ActionType.None, ItemType.Girdle)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Girdle Is Nothing Then
                        TheHero.Equipped.Girdle.Activate(TheHero)
                    End If

                Case "e"
                    If Not TheHero.Equipped.Armor Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Armor.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Armor)
                        End If

                        TheHero.Equipped.Armor.Deactivate(TheHero)
                        TheHero.Equipped.Armor = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Armor = ShowBackpack(ActionType.None, ItemType.Armor)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Armor Is Nothing Then
                        TheHero.Equipped.Armor.Activate(TheHero)
                    End If

                Case "f"
                    If Not TheHero.Equipped.LeftHand Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.LeftHand.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.LeftHand)
                        End If

                        TheHero.Equipped.LeftHand.Deactivate(TheHero)
                        TheHero.Equipped.LeftHand = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.LeftHand = ShowBackpack(ActionType.None, ItemType.Weapon)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.LeftHand Is Nothing Then
                        TheHero.Equipped.LeftHand.Activate(TheHero)
                    End If

                Case "g"
                    If Not TheHero.Equipped.RightHand Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.RightHand.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.RightHand)
                        End If

                        TheHero.Equipped.RightHand.Deactivate(TheHero)
                        TheHero.Equipped.RightHand = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.RightHand = ShowBackpack(ActionType.None, ItemType.Weapon)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.RightHand Is Nothing Then
                        TheHero.Equipped.RightHand.Activate(TheHero)
                    End If

                Case "h"
                    If Not TheHero.Equipped.LeftRing Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.LeftRing.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.LeftRing)
                        End If

                        TheHero.Equipped.LeftRing.Deactivate(TheHero)
                        TheHero.Equipped.LeftRing = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.LeftRing = ShowBackpack(ActionType.None, ItemType.Ring)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.LeftRing Is Nothing Then
                        TheHero.Equipped.LeftRing.Activate(TheHero)
                    End If

                Case "i"
                    If Not TheHero.Equipped.RightRing Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.RightRing.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.RightRing)
                        End If

                        TheHero.Equipped.RightRing.Deactivate(TheHero)
                        TheHero.Equipped.RightRing = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.RightRing = ShowBackpack(ActionType.None, ItemType.Ring)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.RightRing Is Nothing Then
                        TheHero.Equipped.RightRing.Activate(TheHero)
                    End If

                Case "j"
                    If Not TheHero.Equipped.Gloves Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Gloves.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Gloves)
                        End If

                        TheHero.Equipped.Gloves.Deactivate(TheHero)
                        TheHero.Equipped.Gloves = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Gloves = ShowBackpack(ActionType.None, ItemType.Gloves)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Gloves Is Nothing Then
                        TheHero.Equipped.Gloves.Activate(TheHero)
                    End If

                Case "k"
                    If Not TheHero.Equipped.Bracers Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Bracers.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Bracers)
                        End If

                        TheHero.Equipped.Bracers.Deactivate(TheHero)
                        TheHero.Equipped.Bracers = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Bracers = ShowBackpack(ActionType.None, ItemType.Bracers)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Bracers Is Nothing Then
                        TheHero.Equipped.Bracers.Activate(TheHero)
                    End If

                Case "l"
                    If Not TheHero.Equipped.Boots Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Boots.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Boots)
                        End If

                        TheHero.Equipped.Boots.Deactivate(TheHero)
                        TheHero.Equipped.Boots = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Boots = ShowBackpack(ActionType.None, ItemType.Boots)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Boots Is Nothing Then
                        TheHero.Equipped.Boots.Activate(TheHero)
                    End If

                Case "m"
                    If Not TheHero.Equipped.MissleWeapon Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.MissleWeapon.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.MissleWeapon)
                        End If

                        TheHero.Equipped.MissleWeapon.Deactivate(TheHero)
                        TheHero.Equipped.MissleWeapon = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.MissleWeapon = ShowBackpack(ActionType.None, ItemType.MissleWeapon)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.MissleWeapon Is Nothing Then
                        TheHero.Equipped.MissleWeapon.Activate(TheHero)
                    End If

                Case "n"
                    If Not TheHero.Equipped.Missles Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Missles.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Missles)
                        End If

                        TheHero.Equipped.Missles.Deactivate(TheHero)
                        TheHero.Equipped.Missles = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Missles = ShowBackpack(ActionType.None, ItemType.Missles)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Missles Is Nothing Then
                        TheHero.Equipped.Missles.Activate(TheHero)
                    End If

                Case "o"
                    If Not TheHero.Equipped.Tool Is Nothing Then
                        Dim item As Object, AlreadyInBackpack As Boolean = False
                        For Each item In TheHero.Equipped.BackPack
                            If item.walkover = TheHero.Equipped.Tool.walkover Then
                                item.quantity += 1
                                AlreadyInBackpack = True
                            End If
                        Next

                        If Not AlreadyInBackpack Then
                            TheHero.Equipped.BackPack.Add(TheHero.Equipped.Tool)
                        End If

                        TheHero.Equipped.Tool.Deactivate(TheHero)
                        TheHero.Equipped.Tool = Nothing
                        InventoryScreen()
                    Else
                        TheHero.Equipped.Tool = ShowBackpack(ActionType.None, ItemType.Tool)
                        InventoryScreen()
                    End If

                    If Not TheHero.Equipped.Tool Is Nothing Then
                        TheHero.Equipped.Tool.Activate(TheHero)
                    End If

                Case "v"
                    ShowBackpack("view")
                    InventoryScreen()

                Case "z"
                    OK = True

                Case Else
                    ' do nothing?
            End Select
            CalculateAC()
        Loop

        RedrawDisplay()

    End Sub
    Friend Sub kbLook()

        Dim cursorPos As coord
        cursorPos.x = TheHero.LocX
        cursorPos.y = TheHero.LocY

        MessageHandler(resUseMovementKeys)

        CursorLeft = cursorPos.x
        CursorTop = cursorPos.y
        CursorVisible = True

        Dim LookDirection As ConsoleKeyInfo, OK As Boolean = False, strCmd As String

        Do Until OK
            LookDirection = Console.ReadKey(True)
            If LookDirection.KeyChar = Nothing Then
                ' this traps arrow keys
                strCmd = LookDirection.Key.ToString
            Else
                ' this traps regular keys
                strCmd = LookDirection.KeyChar.ToString
            End If
            'strCmd = LookDirection.KeyChar.ToString

            Select Case strCmd
                Case "UpArrow", "DownArrow", "LeftArrow", "RightArrow", 1 To 9
                    Select Case strCmd
                        Case "UpArrow"
                            strCmd = 8
                        Case "DownArrow"
                            strCmd = 2
                        Case "LeftArrow"
                            strCmd = 4
                        Case "RightArrow"
                            strCmd = 6
                    End Select

                    cursorPos = UpdateCursorPos(cursorPos, CInt(strCmd))
                    MessageHandler(EvaluateTile(cursorPos))
                    CursorLeft = cursorPos.x
                    CursorTop = cursorPos.y


                    HeroLOS()
                    Dim heroPos As coord
                    heroPos.x = TheHero.LocX : heroPos.y = TheHero.LocY
                    DrawLine(DeterminePath(heroPos, cursorPos))


                Case " ", "z", "Z"
                    OK = True
                    CursorVisible = False
            End Select
        Loop

    End Sub
    <System.Diagnostics.DebuggerStepThrough()> Friend Function kbKick() As String
        Dim strAnswer As ConsoleKeyInfo,
        ok As Boolean = False
        kbKick = ""

        Do While Not ok
            WriteAt(1, 0, CLEARSPACE)
            WriteAt(1, 0, resPromptKickDirection)
            CursorLeft = 33
            CursorTop = 0
            CursorVisible = True
            strAnswer = ReadKey(True)
            If IsNumeric(strAnswer.KeyChar.ToString) Then
                '.TextColor(ConsoleForeground.White, ConsoleBackground.Black)
                '.WriteAt(1, 0, CLEARSPACE)

                Dim intKey As Integer = CType(strAnswer.KeyChar.ToString, Integer)

                If TheHero.Confused Then
                    intKey = RND.Next(9)
                End If

                If Not Level Is Nothing Then
                    kbKick = Kick(intKey)
                    ok = True
                    CursorVisible = False
                Else
                    kbKick = resNothingToKick
                End If
            End If
        Loop
    End Function
    <System.Diagnostics.DebuggerStepThrough()> Friend Function kbQuit() As Boolean
        Dim ok As Boolean,
        strAnswer As ConsoleKeyInfo

        ok = False
        Do While Not ok
            WriteAt(1, 0, CLEARSPACE)
            WriteAt(1, 0, resPromptQuit)

            strAnswer = ReadKey()
            Select Case strAnswer.KeyChar.ToString
                Case Chr(13)
                    WriteAt(1, 0, CLEARSPACE)
                    ok = True
                    Exit Select
                Case "n", "N"
                    WriteAt(1, 0, CLEARSPACE)
                    ok = True
                    Exit Select
                Case "y", "Y"
                    'TODO: develop an "exit sequence"
                    ok = True
                    Return True
            End Select
        Loop
    End Function
    <DebuggerStepThrough()> Friend Sub kbShowKeyBindings()
        Clear()

        WriteAt(20, 1, resKeybindingsTitle)

        WriteAt(2, 4, "Movement: (use number keys)", ConsoleColor.Yellow)
        WriteAt(2, 9, "Commands:", ConsoleColor.Yellow)

        WriteAt(2, 5, "7 8 9  NW N NE                            ")
        WriteAt(2, 6, "4 5 6  W  *  E         <   Ascend Stairs       @ - Show Hero Status")
        WriteAt(2, 7, "1 2 3  SW S SE         >   Descend Stairs      ? - Show Keybindings")

        WriteAt(2, 10, "a - Apply Skill ")
        WriteAt(2, 11, "c - Close Door         C - Chat w/ Someone     , - Pick up item(s)")
        WriteAt(2, 12, "d - Drop Something     D - Drink Something     i - Inventory      ")
        WriteAt(2, 13, "k - Kick Something     l - Look/Examine                           ")
        WriteAt(2, 14, "o - Open Door          O - Divine Offering     p - Pay/Purchase   ")
        WriteAt(2, 15, "q - Display Quests     Q - Quit Game           r - Read Item      ")
        WriteAt(2, 16, "s - Search Area        t - Throw item/weapon   u - Use Item       ")
        WriteAt(2, 17, "w - Auto Walk          z - Zap Wand            Z - Cast a spell   ")

        WriteAt(2, 20, "Items in           are not implemented yet.")

        ' not implemented yet
        WriteAt(25, 14, "O - Divine Offering     p - Pay/Purchase", ConsoleColor.DarkGray)
        WriteAt(2, 15, "q - Display Quests", ConsoleColor.DarkGray)
        WriteAt(49, 15, "r - Read Item", ConsoleColor.DarkGray)
        WriteAt(25, 16, "t - Throw item/weapon   u - Use Item       ", ConsoleColor.DarkGray)
        WriteAt(25, 17, "z - Zap Wand            Z - Cast a spell   ", ConsoleColor.DarkGray)

        WriteAt(11, 20, "dark gray", ConsoleColor.DarkGray)

        ' wait for a key
        WriteAt(2, 22, resPromptPressAnyKey)
        ReadKey()

        RedrawDisplay()
    End Sub
    <DebuggerStepThrough()> Friend Function kbUp() As String

        If TheHero.TerrainZoom Then
            kbUp = DoStairs(Direction.Up)
        ElseIf TheHero.InTown Then
            kbUp = DoStairs(Direction.Up)
        Else
            If Level Is Nothing Then
                kbUp = String.Empty
            ElseIf Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.StairsUp Then
                kbUp = DoStairs(Direction.Up)
            Else
                kbUp = resStairsNoUp
            End If
        End If
    End Function
    Friend Function kbDown() As String
        If OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType = TerrainType.Special _
        And TheHero.Overland = True Then
            If TheHero.OverX = 11 And TheHero.OverY = 5 Then     ' the flooded tunnel
                kbDown = resFloodedTunnel
            Else
                kbDown = DoStairs(Direction.Down)
            End If

        ElseIf OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType = TerrainType.Town _
        And TheHero.Overland = True Then
            If TheHero.OverX = 8 And TheHero.OverY = 14 Then
                kbDown = DoTown(Town.Fincastle)
            ElseIf TheHero.OverX = 58 And TheHero.OverY = 14 Then
                kbDown = DoTown(Town.Stonegate)
            Else
                kbDown = resNoVisitorsYet
            End If

        ElseIf TheHero.Overland = True Then
            ' we are overland, but not at an entrance, so zoom in on terrain.
            'TODO: zoom in on terrain type
            kbDown = DoTerrain(OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType)

        ElseIf TheHero.Overland = False AndAlso Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.StairsDn Then
            kbDown = DoStairs(Direction.Down)

        Else
            kbDown = resStairsNoDown
        End If
    End Function
    Friend Function kbSearch() As String
        kbSearch = resSearchArea

        If Not TheHero.Overland Then
            ' iterate through compass, looking for secret doors and/or undiscovered traps
            kbSearch &= SweepSearch(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ)
            kbSearch &= SweepSearch(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ)
        End If
    End Function
    Friend Function kbSkills(ByRef bolValid As Boolean) As String
        kbSkills = ""
        Clear()
        WriteAt(0, 0, resSkillsTitle, ConsoleColor.DarkYellow)
        WriteAt(2, 1, resSkillsName, ConsoleColor.White)
        WriteAt(60, 1, resSkillsRBT, ConsoleColor.White)
        Dim y As Int16 = 3
        Dim x As Int16 = 68
        Dim x2 As Int16 = 74
        Dim start As String = "a"
        Dim arrKey(TheHero.Skills.Count) As Char
        Dim intCtr As Int16 = 0
        Dim ht As New Hashtable

        With TheHero
            Dim iDict As DictionaryEntry

            For Each iDict In TheHero.Skills
                If Left(GetBase(iDict.Key).ToString, 1) = "-" Then x = 67
                If Left(iDict.Value + GetBase(iDict.Key).ToString, 1) = "-" Then x2 = 73
                ht.Add(start, iDict.Key)
                WriteAt(2, y, start, ConsoleColor.White)
                WriteAt(3, y, "] " & iDict.Key, ConsoleColor.DarkYellow)
                WriteAt(62, y, iDict.Value, ConsoleColor.Gray)
                WriteAt(x, y, GetBase(iDict.Key), ConsoleColor.Gray)
                WriteAt(x2, y, iDict.Value + GetBase(iDict.Key))
                y += 1
                x = 68
                x2 = 74
                arrKey(intCtr) = start
                start = Chr(Asc(start) + 1)
                intCtr += 1
            Next
        End With

        ' bottom border and key choices
        WriteAt(0, 21, resSkillsBottomBorder, ConsoleColor.DarkYellow)
        WriteAt(21, 23, "[a- ] " + resSkillsApply + " - [z] " + resReturnToGame, ConsoleColor.DarkYellow)

        WriteAt(22, 23, "a")
        WriteAt(24, 23, Chr(Asc(start) - 1))
        WriteAt(42, 23, "z")

        Dim strAnswer As ConsoleKeyInfo,
            OK As Boolean

        bolValid = False

        Do While Not OK
            strAnswer = ReadKey(True)
            Select Case strAnswer.KeyChar.ToString
                Case "z"
                    OK = True
                Case Else
                    If Asc(strAnswer.KeyChar.ToString) > Asc(start) - 1 Then
                        ' invalid key, just ignore
                    Else
                        Select Case ht(strAnswer.KeyChar.ToString).ToString
                            ' use DoTurnCounter(False) on skills that take a turn

                            Case "Appraise"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Awareness"
                                kbSkills = resSkillsMsgAwareness
                                OK = True
                            Case "Backstab"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Climbing"
                                kbSkills = resSkillsMsgClimbing
                                OK = True
                            Case "Concentration"
                                kbSkills = resSkillsMsgConcentration
                                OK = True
                            Case "Disable Trap"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Find Weakness"
                                kbSkills = resSkillsMsgFindWeak
                                OK = True
                            Case "First Aid"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Healing"
                                kbSkills = resSkillsMsgHealing
                                OK = True
                            Case "Hide"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Listen"
                                kbSkills = resSkillsMsgListen
                                OK = True
                            Case "Literacy"
                                kbSkills = resSkillsMsgLiteracy
                                OK = True
                            Case "Mining"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Pick Locks"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Pick Pockets"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Search"
                                RedrawDisplay()
                                kbSkills = kbSearch()
                                OK = True
                                bolValid = True
                            Case "Spellcraft"
                                kbSkills = resSkillsNotImplemented
                                OK = True
                            Case "Survival"
                                kbSkills = resSkillsMsgSurvival
                                OK = True
                            Case "Swimming"
                                kbSkills = resSkillsMsgSwimming
                                OK = True
                        End Select
                    End If
            End Select
        Loop
        RedrawDisplay()
    End Function
    Friend Function kbTarget() As String
        'TODO: Targeting System, Work in Progress

        Dim cursorPos As coord
        cursorPos.x = TheHero.LocX
        cursorPos.y = TheHero.LocY

        MessageHandler(resTargetAim)

        CursorLeft = cursorPos.x
        CursorTop = cursorPos.y
        CursorVisible = True

        Dim AimDirection As ConsoleKeyInfo, OK As Boolean = False, strCmd As String

        Do Until OK
            AimDirection = Console.ReadKey(True)
            If AimDirection.KeyChar = Nothing Then
                ' this traps arrow keys
                strCmd = AimDirection.Key.ToString
            Else
                ' this traps regular keys
                strCmd = AimDirection.KeyChar.ToString
            End If

            Select Case strCmd
                Case "UpArrow", "DownArrow", "LeftArrow", "RightArrow", 1 To 9
                    Select Case strCmd
                        Case "UpArrow"
                            strCmd = 8
                        Case "DownArrow"
                            strCmd = 2
                        Case "LeftArrow"
                            strCmd = 4
                        Case "RightArrow"
                            strCmd = 6
                    End Select

                    cursorPos = UpdateCursorPos(cursorPos, CInt(strCmd))
                    MessageHandler(EvaluateTile(cursorPos))
                    CursorLeft = cursorPos.x
                    CursorTop = cursorPos.y

                    HeroLOS()
                    Dim heroPos As coord
                    heroPos.x = TheHero.LocX : heroPos.y = TheHero.LocY

                    If heroPos.x <> cursorPos.x Or heroPos.y <> cursorPos.y Then
                        BuildLine(heroPos.x, heroPos.y, cursorPos.x, cursorPos.y, New PlotFunction(AddressOf DrawTargetLine))
                    End If

                Case " ", "z", "Z"
                    OK = True
                    CursorVisible = False
                    RedrawDisplay()

                Case "t"
                    'ToDo: Examine targeted tile
                    'ToDo: Accept/Reject targeted tile
                    'ToDo: If accepted, shoot targeted tile
                    'ToDo: If missile is intercepted, process accordingly
                    'ToDo: otherwise process at target
                    'ToDo: chance of missile breakage
                    'ToDo: if not broken, chance of drop
            End Select
        Loop

    End Function
    Friend Function kbWalk() As String

        kbWalk = ""
    End Function
    Friend Function kbSpellCast() As String

        kbSpellCast = ""
    End Function
    Friend Function kbUseWand() As String

        kbUseWand = ""
    End Function


#End Region

#Region " Helper Methods "
    <DebuggerStepThrough()> Friend Function OpenDoor(ByVal intDoorDirection As Integer) As String

        Dim DieRoll As Int16, intX As Int16, intY As Int16, intZ As Int16, DoorDC As Byte

        intX = TheHero.LocX
        intY = TheHero.LocY
        intZ = TheHero.LocZ
        OpenDoor = ""

        DoorDC = 9 + D10()
        DieRoll = D20()
        If DieRoll + AbilityMod(TheHero.EStrength) < DoorDC Then
            ' you couldn't get the door open this time
            OpenDoor = resDoorStuck
        Else
			' you opened the door
			Select Case intDoorDirection
				Case 1
					If TheHero.InTown Then
						If m_TownMap(intX - 1, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX - 1, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Door Then Level(intX - 1, intY + 1, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 2
					If TheHero.InTown Then
						If m_TownMap(intX, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX, intY + 1, intZ).FloorType = SquareType.Door Then Level(intX, intY + 1, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 3
					If TheHero.InTown Then
						If m_TownMap(intX + 1, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX + 1, intY + 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Door Then Level(intX + 1, intY + 1, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 4
					If TheHero.InTown Then
						If m_TownMap(intX - 1, intY, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX - 1, intY, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX - 1, intY, intZ).FloorType = SquareType.Door Then Level(intX - 1, intY, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 5
                    OpenDoor = resOpenDoorSelf

                Case 6
					If TheHero.InTown Then
						If m_TownMap(intX + 1, intY, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX + 1, intY, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX + 1, intY, intZ).FloorType = SquareType.Door Then Level(intX + 1, intY, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 7
					If TheHero.InTown Then
						If m_TownMap(intX - 1, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX - 1, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Door Then Level(intX - 1, intY - 1, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 8
					If TheHero.InTown Then
						If m_TownMap(intX, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX, intY - 1, intZ).FloorType = SquareType.Door Then Level(intX, intY - 1, intZ).FloorType = SquareType.OpenDoor
					End If

				Case 9
					If TheHero.InTown Then
						If m_TownMap(intX + 1, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.door Then m_TownMap(intX + 1, intY - 1, TheHero.Town).CellType = HA.Towns.CellType.opendoor
					Else
						If Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Door Then Level(intX + 1, intY - 1, intZ).FloorType = SquareType.OpenDoor
					End If
			End Select

			If OpenDoor = "" Then
                OpenDoor = resOpenDoor
                WriteAt(intX, intY, "/", ConsoleColor.DarkYellow)
			End If
		End If

	End Function
    <DebuggerStepThrough()> Friend Function CloseDoor(ByVal intDoorDirection As Integer) As String

		CloseDoor = ""
		Select Case intDoorDirection
			Case 1
				If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 2
				If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 3
				If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 4
				If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 5
                CloseDoor = resCloseDoorSelf

            Case 6
				If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 7
				If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 8
				If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

			Case 9
				If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.OpenDoor _
				And Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 Then
					Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Door
					FixFloor(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ)
                    CloseDoor = resCloseDoor
                Else
                    CloseDoor = resCloseDoorBlocked1 & m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).MonsterRace & resCloseDoorBlocked2
                End If

		End Select

	End Function

    <DebuggerStepThrough()> Friend Function ChatCreature(ByVal intCreatureDirection As Integer) As String

		ChatCreature = ""
		Select Case intCreatureDirection
			Case 1
				If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).chat
				End If

			Case 2
				If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).chat
				End If

			Case 3
				If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1).chat
				End If

			Case 4
				If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster - 1).chat
				End If

			Case 5
                ChatCreature = resChatSelf

            Case 6
				If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster - 1).chat
				End If

			Case 7
				If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).chat
				End If

			Case 8
				If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).chat
				End If

			Case 9
				If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
					ChatCreature = m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1).chat
				End If
		End Select

	End Function

	Private Function UpdateCursorPos(ByVal CursorPosition As coord, ByVal Direction As Integer) As coord
		Select Case Direction
			Case 1
				UpdateCursorPos.x = CursorPosition.x - 1
				UpdateCursorPos.y = CursorPosition.y + 1
			Case 2
				UpdateCursorPos.x = CursorPosition.x
				UpdateCursorPos.y = CursorPosition.y + 1
			Case 3
				UpdateCursorPos.x = CursorPosition.x + 1
				UpdateCursorPos.y = CursorPosition.y + 1

			Case 4
				UpdateCursorPos.x = CursorPosition.x - 1
				UpdateCursorPos.y = CursorPosition.y
			Case 5
				UpdateCursorPos.x = CursorPosition.x
				UpdateCursorPos.y = CursorPosition.y
			Case 6
				UpdateCursorPos.x = CursorPosition.x + 1
				UpdateCursorPos.y = CursorPosition.y

			Case 7
				UpdateCursorPos.x = CursorPosition.x - 1
				UpdateCursorPos.y = CursorPosition.y - 1
			Case 8
				UpdateCursorPos.x = CursorPosition.x
				UpdateCursorPos.y = CursorPosition.y - 1
			Case 9
				UpdateCursorPos.x = CursorPosition.x + 1
				UpdateCursorPos.y = CursorPosition.y - 1
		End Select

		CursorLeft = UpdateCursorPos.x
		CursorTop = UpdateCursorPos.y

	End Function
	Private Function EvaluateTile(ByVal CursorPosition As coord) As String
		' return text descriptions in this order of precedence:
		'   If the Room is Dark
		'      - "This room is dark."
		'   If tile is within Line of Sight
		'      - creature
		'      - pile of items ("There is a pile of items here.")
		'      - single item
		'      - trap
		'      - tile descriptor
		'   If tile is unknown
		'      - "You don't know anything about this place."
		'
		' TO DO STILL:
		'   If tile is beyond Line of Sight, but has been viewed before
		'      - tile descriptor only
		'
		' ISSUES:  cursor should not move past world limits
		With Level(CursorPosition.x, CursorPosition.y, TheHero.LocZ)
			If .Observed Then
				If .Illumination > IlluminationStrength.Dark Then
					If .Monster > 0 Then
						Dim ActiveMonster As Monster = DirectCast(m_arrMonster(.Monster - 1), Monster)
						Dim MonsterMsg As String = BuildMonsterStatusMsg(ActiveMonster)

						Debug.WriteLine("You just looked at a monster: " & MonsterMsg & " at these coordinates: (" & CursorPosition.x & "," & CursorPosition.y & ").")
						Return MonsterMsg

					ElseIf .itemcount > 1 Then
                        Return resLookItemsPile
                    ElseIf .itemcount = 1 Then
						Return "A " & .items(0).walkover.ToLower & " is lying here."
					ElseIf .TrapDiscovered = True AndAlso .TrapType > 0 Then
						Return TrapDescriptor(.TrapType)
					Else
						Return TileDescriptor(.FloorType)
					End If
				Else
                    Return resLookDark
                End If
			Else
                Return resLookUnknown
            End If
		End With
	End Function


	Friend Sub MultipleItemList(ByVal intx As Integer, _
								 ByVal inty As Integer, _
								 ByVal intz As Integer)

		Dim yPos As Integer = 2, _
			startletter As String = "a"

		Clear()
        WriteAt(0, 0, resItemsGround)

        Dim item As Object
		For Each item In Level(intx, inty, intz).items
			WriteAt(1, yPos, startletter & "] " & item.walkover)
			yPos += 1
			startletter = Chr(Asc(startletter) + 1)
		Next

		startletter = Chr(Asc(startletter) - 1)

        WriteAt(23, 23, resItemsPickup & " [a - " & startletter & "] " & resItemsZToExit, ConsoleColor.DarkYellow)
        WriteAt(37, 23, "a")
		WriteAt(41, 23, startletter)
		WriteAt(47, 23, "z")

	End Sub

	Friend Sub KickCoord(ByRef x As Integer, ByRef y As Integer, ByVal intkickdirection As Integer)
		Select Case intkickdirection
			Case 1
				x -= 1
				y += 1
			Case 2
				x = x
				y += 1
			Case 3
				x += 1
				y += 1
			Case 4
				x -= 1
				y = y
			Case 5

			Case 6
				x += 1
				y = y
			Case 7
				x -= 1
				y -= 1
			Case 8
				x = x
				y -= 1
			Case 9
				x += 1
				y -= 1
		End Select
	End Sub
    <DebuggerStepThrough()> Friend Function Kick(ByVal intKickDirection As Integer) As String
		Dim DC As Integer, _
			sb As New StringBuilder, _
			roll As Integer, _
			x As Integer, y As Integer, z As Integer

		x = TheHero.LocX
		y = TheHero.LocY
		z = TheHero.LocZ
		KickCoord(x, y, intKickDirection)

		If intKickDirection <> 5 Then
			Select Case Level(x, y, z).FloorType
				Case SquareType.Wall
                    sb.Append(resKickWall)
                    DC = 30
                    'ToDo: add support for (tracking) damaging walls, not just destroying them
                    roll = D20()
					If roll + AbilityMod(TheHero.EStrength) >= DC Then
                        sb.Append(resKickWallRubble)
                        Level(x, y, z).FloorType = SquareType.Floor
						FixFloor(x, y, z)
					Else
                        sb.Append(resKickWallFail)
                    End If

				Case SquareType.Floor
					' is there a monster standing where you just kicked?
					If Level(x, y, z).Monster > 0 Then
						Dim intEncounter As Integer
						intEncounter = Level(x, y, z).Monster - 1
						Dim CritterType As Type = m_arrMonster(intEncounter).GetType()

						' use a kicking attack
						roll = D20()
						If roll + AbilityMod(TheHero.EStrength) >= m_arrMonster(intEncounter).ac Then
                            'ToDo: Create ImpactVerb() to randomly pull from a list. (Replace Wham!)
                            sb.Append(resKickMonsterHit & m_arrMonster(intEncounter).MonsterRace & "! ")
                            Dim intDamage As New Integer

							' the kick hits, so calculate damage
							roll = D4()
							intDamage = roll + AbilityMod(TheHero.EStrength)
							m_arrMonster(intEncounter).currenthp -= intDamage

							' is there any knockback?
							roll = D20()
							If roll + AbilityMod(TheHero.EStrength) >= m_arrMonster(intEncounter).ac Then
								' if the hero rolled a hit again, then calculate knockback
								Dim destx As Integer = x
								Dim desty As Integer = y

								KickCoord(destx, desty, intKickDirection)

								' make sure there is nothing blocking the square past the monster
								If Level(destx, desty, z).Monster = 0 _
								And (Level(destx, desty, z).FloorType = SquareType.Floor _
									Or Level(destx, desty, z).FloorType = SquareType.OpenDoor) Then
									' knockback the critter one square

									' evacuate the "from" square
									Level(x, y, z).Monster = 0

									' if its a square we've seen before, tidy it up, otherwise dont worry about it
									If Level(x, y, z).Observed = True Then
										FixFloor(x, y, z)
									End If

									' move the monster to the new square
									m_arrMonster(intEncounter).walk(intKickDirection)

									' cleanup
									RedrawMonsters()
									HeroLOS()

									'debug message...
									Debug.WriteLine(m_arrMonster(intEncounter).monsterrace & " is knocked back by a kick!")
								End If
							End If

                            sb.Append(resArticleThe & " " & m_arrMonster(intEncounter).MonsterRace & " ")

                            ' assess the damage (Undead aren't killed or dead, so adjust accordingly)
                            Select Case m_arrMonster(intEncounter).currenthp
								Case Is <= 0
									If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
                                        ' Undead... say "destroyed"
                                        sb.Append(resMonsterDestroyed)
                                    Else
                                        sb.Append(resMonsterDead)
                                    End If
								Case 1 To 2	' critical wounds
									If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
										' Undead... say nothing
									Else
                                        sb.Append(resMonsterAboutToDie)
                                    End If
								Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.5) ' serious wounds
                                    sb.Append(resMonsterSeriousWounds)
                                Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.9) ' minor wounds
                                    sb.Append(resMonsterMinorWounds)
                                Case Is > CInt(m_arrMonster(intEncounter).HP * 0.9) ' barely scratched
                                    sb.Append(resMonsterScratches)
                            End Select

							' did we kill it?
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

						Else
                            sb.Append(resKickMiss1 & m_arrMonster(Level(x, y, z).Monster - 1).MonsterRace & resKickMiss2)
                        End If

						' no monster here, are there any items on the floor?
					ElseIf Not Level(x, y, z).items Is Nothing Then
						If Level(x, y, z).items.Count > 0 Then
                            'TODO: move items across floor when kicked
                            sb.Append(resKickMiss1 & Level(x, y, z).items(0).walkover & ". ")
                        End If

						' no monsters, no items on the floor
					Else
                        sb.Append(resKickNothing)
                    End If

				Case SquareType.OpenDoor And Level(x, y, z).Monster > 0
					' monster standing in doorway

					Dim intEncounter As Integer
					intEncounter = Level(x, y, z).Monster - 1
					Dim CritterType As Type = m_arrMonster(intEncounter).GetType()

					' use a kicking attack
					roll = D20()
					If roll + AbilityMod(TheHero.EStrength) >= m_arrMonster(intEncounter).ac Then
                        sb.Append(resKickMonsterHit & m_arrMonster(intEncounter).MonsterRace & "! ")
                        Dim intDamage As New Integer

						' the kick hits, so calculate damage
						roll = D4()
						intDamage = roll + AbilityMod(TheHero.EStrength)
						m_arrMonster(intEncounter).currenthp -= intDamage

						' is there any knockback?
						roll = D20()
						If roll + AbilityMod(TheHero.EStrength) >= m_arrMonster(intEncounter).ac Then
							' if the hero rolled a hit again, then calculate knockback
							Dim destx As Integer = x
							Dim desty As Integer = y

							KickCoord(destx, desty, intKickDirection)

							' make sure there is nothing blocking the square past the monster
							If Level(destx, desty, z).Monster = 0 _
							And (Level(destx, desty, z).FloorType = SquareType.Floor _
								Or Level(destx, desty, z).FloorType = SquareType.OpenDoor) Then
								' knockback the critter one square

								' evacuate the "from" square
								Level(x, y, z).Monster = 0

								' if its a square we've seen before, tidy it up, otherwise dont worry about it
								If Level(x, y, z).Observed = True Then
									FixFloor(x, y, z)
								End If

								' move the monster to the new square
								m_arrMonster(intEncounter).walk(intKickDirection)

								' cleanup
								RedrawMonsters()
								HeroLOS()

								'debug message...
								Debug.WriteLine(m_arrMonster(intEncounter).monsterrace & " is knocked back by a kick!")
							End If
						End If

                        sb.Append(resArticleThe & " " & m_arrMonster(intEncounter).MonsterRace & " ")

                        ' assess the damage 
                        Select Case m_arrMonster(intEncounter).currenthp
							Case Is <= 0
								If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
                                    ' Undead... say "destroyed"
                                    sb.Append(resMonsterDestroyed)
                                Else
                                    sb.Append(resMonsterDead)
                                End If
							Case 1 To 2	' critical wounds
								If Right(CritterType.BaseType.ToString, 6) = "Undead" Then
									' Undead... say nothing
								Else
                                    sb.Append(resMonsterAboutToDie)
                                End If
							Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.5) ' serious wounds
                                sb.Append(resMonsterSeriousWounds)
                            Case Is <= CInt(m_arrMonster(intEncounter).HP * 0.9) ' minor wounds
                                sb.Append(resMonsterMinorWounds)
                            Case Is > CInt(m_arrMonster(intEncounter).HP * 0.9) ' barely scratched
                                sb.Append(resMonsterScratches)
                        End Select

						' did we kill it?
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

					Else
                        sb.Append(resKickMiss1 & m_arrMonster(Level(x, y, z).Monster - 1).MonsterRace & resKickMiss2)
                    End If

				Case SquareType.Door, SquareType.OpenDoor
                    sb.Append(resKickDoor)
                    DC = 10
					roll = D20()
					If roll + AbilityMod(TheHero.EStrength) >= DC Then
                        sb.Append(resKickDoorSmash)
                        Level(x, y, z).FloorType = SquareType.Floor
						FixFloor(x, y, z)
					Else
                        sb.Append(resKickDoorNothing)
                    End If
			End Select
		Else
            sb.Append(resKickSelf)
        End If

		Kick = sb.ToString
		sb = Nothing
	End Function

	Friend Function SweepSearch(ByVal intX As Int16, ByVal intY As Int16, ByVal intZ As Int16) As String
		Dim SearchDC As Int16 = 10
		If HasSkill("Search") > -1 Then
			SearchDC -= HasSkill("Search")
		End If

		With Level(intX, intY, intZ)
			SweepSearch = ""
			Select Case .FloorType
				Case SquareType.Secret
					Dim roll As Int16 = D20()
					If roll >= SearchDC Then
						.FloorType = SquareType.Door
						FixFloor(intX, intY, intZ)
                        SweepSearch &= resSearchSecretDoor
                    End If

				Case SquareType.Trap
					Dim action As String = ""
					FixFloor(intX, intY, intZ)

					If .TrapDiscovered Then
                        SweepSearch &= resSearchTrapObserve & " " & GetTrap(.TrapType) & resTrap
                    Else
						If D20() >= SearchDC Then
							.TrapDiscovered = True
                            SweepSearch &= resSearchTrapDiscover & " " & GetTrap(.TrapType) & resTrap
                        End If
					End If

				Case Else
					SweepSearch = ""
			End Select
		End With
	End Function

	Friend Function DoStairs(ByVal intDirection As Integer) As String
		Dim arrMonsterPos As New ArrayList, strDirection As String
		Dim arrAdjacentMonsters As New ArrayList
		DoStairs = ""

		If Not TheHero.Overland Then
			' check to see if any monsters are adjacent and see if they follow you
			arrAdjacentMonsters = CheckForAdjacentMonster(arrMonsterPos)
		End If

		If intDirection = Direction.Up Then
			strDirection = "up"

			' only do this if we were in a dungeon (not in town or terrain zoom)
			If Not TheHero.InTown And Not TheHero.TerrainZoom Then
				TheHero.CurrentLevel -= 1
			End If

			If TheHero.CurrentLevel = 0 Then
				TheHero.Overland = True
				ReDrawOverlandMap()
				OverlandLOS()
				WriteAt(TheHero.OverX + 1, TheHero.OverY + 3, TheHero.Icon, TheHero.Color)
				If TheHero.TerrainZoom Then
					TheHero.TerrainZoom = False
                    DoStairs = resStairsUpOverland
                ElseIf TheHero.InTown Then
					TheHero.InTown = False
					TheHero.Town = -1
                    DoStairs = resStairsUpWilderness
                Else
                    DoStairs = resStairsUpSurface
                End If
			End If
		Else
			strDirection = "down"
			TheHero.CurrentLevel += 1
			' we're in a dungeon now, clear Overland flag
			TheHero.Overland = False
		End If

		If Not TheHero.Overland Then
			' going to a new level, so clear out the monster array
			m_arrMonster.Clear()
			' and the stairs location
			m_stairsUp = Nothing
			m_stairsDn = Nothing

			If BuildIt() Then
				' dungeon build ok, now find stairs
				FindStairs()
				' display character stats and their values
				DisplayStats()
				DisplayValues()
				DoTurnCounter()
				' place the avatar at the stairs
				PlaceAvatar(intDirection)
				' place monsters randomly on level
				PlaceMonsters(TheHero.LocZ)
				' place items/treasure randomly on level
				PlaceItems(TheHero.LocZ)

				' build the appropriate message
				Select Case arrAdjacentMonsters.Count
					Case Is > 1
                        ' multiple creatures follow you, so build a generic message
                        DoStairs = resStairsYouGo & strDirection & resStairsTheStairs & resStairsFollowSeveral
                        PlaceAdjacentMonster(arrAdjacentMonsters, arrMonsterPos)
					Case 1
                        ' 1 creature followed you, so refer to it by race
                        DoStairs = resArticleThe & arrAdjacentMonsters(0).monsterrace & resStairsFollowsYou & strDirection & resStairsTheStairs
                        PlaceAdjacentMonster(arrAdjacentMonsters, arrMonsterPos)
					Case Else
                        ' nothing followed you up
                        DoStairs = resStairsYouGo & strDirection & resStairsTheStairs
                        If intDirection = Direction.Down Then DoStairs &= resStairsDownEverEnd
                End Select

				' do a quick initial scan of the area
				HeroLOS()

				' mark the turn number at level change, for monster generation
				TheHero.TurnCountAtDungeonLevelChange = TheHero.TurnCount
			Else
				' dungeon creation failed, show error and end game
				DungeonCreationFailureScreen()
				DoStairs = ""
			End If
		End If
	End Function

	Friend Function CheckForAdjacentMonster(ByRef arrMonsterPos As ArrayList) As ArrayList
		Dim AdjacentMonster As New ArrayList

		If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("1")
		End If
		If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("2")
		End If
		If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("3")
		End If
		If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("4")
		End If
		If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("6")
		End If
		If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("7")
		End If
		If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("8")
		End If
		If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 Then
			AdjacentMonster.Add(m_arrMonster(Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster - 1))
			arrMonsterPos.Add("9")
		End If

		Return AdjacentMonster
	End Function
	Friend Function PlaceAdjacentMonster(ByVal arrAdjacentMonsters As ArrayList, ByVal arrMonsterPos As ArrayList) As Boolean
		Dim intCtr As Integer, ok As Boolean
		For intCtr = 0 To arrAdjacentMonsters.Count - 1
			' add each monster that followed the hero up to the new level
			m_arrMonster.Add(arrAdjacentMonsters(intCtr))

			ok = False
			Do While Not ok
				ok = BumpMonster(arrMonsterPos(intCtr))
				If Not ok Then
					Dim intRND As Integer = 5
					Do Until intRND <> 5
						intRND = RND.Next(1, 9)
						If intRND = 5 Then intRND = 4
						ok = BumpMonster(RND.Next(1, 9))
					Loop
				End If
			Loop
		Next
	End Function
	Friend Function BumpMonster(ByVal intLocation As Integer) As Boolean
		Select Case intLocation
			Case "1"
				If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX - 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY - 1
				Else
					Return False
				End If

			Case "2"
				If Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY - 1
				Else
					Return False
				End If

			Case "3"
				If Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX + 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY - 1
				Else
					Return False
				End If

			Case "4"
				If Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX - 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY
				Else
					Return False
				End If

			Case "6"
				If Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX + 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY
				Else
					Return False
				End If

			Case "7"
				If Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX - 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY + 1
				Else
					Return False
				End If

			Case "8"
				If Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY + 1
				Else
					Return False
				End If

			Case "9"
				If Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster = 0 _
				And Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).FloorType = SquareType.Floor Then
					Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster = m_arrMonster.Count
					m_arrMonster(m_arrMonster.Count - 1).locx = TheHero.LocX + 1
					m_arrMonster(m_arrMonster.Count - 1).locy = TheHero.LocY + 1
				Else
					Return False
				End If
		End Select

		Return True
	End Function
	Friend Function DirectionMod(PosA As coord, PosB As coord) As coord
		Dim adjustment As coord

		If PosA.x < PosB.x Then adjustment.x = 1
		If PosA.x = PosB.x Then adjustment.x = 0
		If PosA.x > PosB.x Then adjustment.x = -1

		If PosA.y < PosB.y Then adjustment.y = 1
		If PosA.y = PosB.y Then adjustment.y = 0
		If PosA.y > PosB.y Then adjustment.y = -1

		Return adjustment
	End Function

#End Region

End Module
