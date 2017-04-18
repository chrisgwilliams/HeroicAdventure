Imports DBuild.DunGen3
Imports System.console
Imports System.Text
Imports HA.Common.Helper

Namespace Screens
    Module m_Screens

#Region " Title Screens etc  "

        Friend Sub TitleScreen()
            Dim intCtr As Integer, intCtr2 As Integer
            Dim LongPoint As New ArrayList ' This is for the "drip"

            Clear()

            ' make random crappy stalactites
            For intCtr = 1 To 78
                For intCtr2 = 0 To RND.Next(0, 3)
                    If intCtr2 <= 1 Then WriteAt(intCtr, intCtr2, "*", ConsoleColor.DarkGray)
                    If intCtr2 = 2 Then WriteAt(intCtr, intCtr2, "*", ConsoleColor.Gray)
                    If intCtr2 = 3 Then
                        WriteAt(intCtr, intCtr2, "*", ConsoleColor.White)

                        ' add "long" stalactites to either side of the text block, for the random "drip"
                        If intCtr < 15 OrElse intCtr > 66 Then
                            LongPoint.Add(intCtr)
                        End If
                    End If
                Next
            Next

            For intCtr = 26 To 54 Step 2
                WriteAt(intCtr, 5, "*", ConsoleColor.DarkRed)
            Next
            WriteAt(27, 5, ".", ConsoleColor.Red)
            WriteAt(29, 5, ".", ConsoleColor.Red)
            WriteAt(51, 5, ".", ConsoleColor.Red)
            WriteAt(53, 5, ".", ConsoleColor.Red)
            WriteAt(32, 5, "Heroic Adventure!", ConsoleColor.Yellow)

            WriteAt(22, 6, "Against the Dark Lord  ", ConsoleColor.Red)
            WriteAt(45, 6, "Version " & m_version.ToString)

            For intCtr = 15 To 66 Step 2
                WriteAt(intCtr, 8, "~", ConsoleColor.DarkRed)
            Next
            For intCtr = 16 To 65 Step 2
                WriteAt(intCtr, 8, "~", ConsoleColor.Red)
            Next

            WriteAt(19, 9, "Created and maintained by: ", ConsoleColor.Gray)
            WriteAt(46, 9, "Chris G. Williams")

            WriteAt(16, 11, "Eternally grateful and dedicated to", ConsoleColor.Gray)
            WriteAt(52, 11, "E. Gary Gygax")
            WriteAt(65, 11, ",", ConsoleColor.Gray)

            WriteAt(21, 12, "Dave Arneson")
            WriteAt(33, 12, ", and ", ConsoleColor.Gray)
            WriteAt(39, 12, "Charles (Pop) Williams")

            WriteAt(15, 14, "For additional information:", ConsoleColor.Gray)
            WriteAt(43, 14, "www.fb.me/heroicadventure")

            For intCtr = 15 To 66 Step 2
                WriteAt(intCtr, 15, "~", ConsoleColor.DarkRed)
            Next
            For intCtr = 16 To 65 Step 2
                WriteAt(intCtr, 15, "~", ConsoleColor.Red)
            Next

            WriteAt(24, 16, "Released to Open Source in 2017", ConsoleColor.Gray)
            WriteAt(22, 17, "Use it. Learn from it. Keep it free.", ConsoleColor.Gray)


            ' Now pick a Random Drip from one of the long Stalactites before we build the Stalagmites
            Dim DripStartX As Integer = LongPoint(RND.Next(0, LongPoint.Count - 1))
            Dim DripStartY As Integer = 4
            Dim DripStopY As Integer

            ' make random stalagmites, and measure the top of the one that lines up with our drippy Stalactite
            For intCtr = 1 To 78
                For intCtr2 = 0 To RND.Next(0, 5)
                    If intCtr2 <= 2 Then WriteAt(intCtr, 24 - intCtr2, "*", ConsoleColor.DarkGray)
                    If intCtr2 >= 3 Then WriteAt(intCtr, 24 - intCtr2, "*", ConsoleColor.Gray)
                    If intCtr2 = 5 Then WriteAt(intCtr, 24 - intCtr2, "*", ConsoleColor.White)
                Next

                If intCtr = DripStartX Then DripStopY = 24 - intCtr2
            Next


            ' Animate the drip
            Dim inc As Integer
            For inc = 0 To DripStopY - DripStartY
                System.Threading.Thread.Sleep(80)
                WriteAt(DripStartX, DripStartY + inc, ".")
                ' after 1st position is drawn
                If inc > 0 Then WriteAt(DripStartX, DripStartY + inc - 1, " ")
            Next

            ' finally do the little splash
            WriteAt(DripStartX, DripStartY + inc - 1, "o") : System.Threading.Thread.Sleep(20)
            WriteAt(DripStartX, DripStartY + inc - 1, "*") : System.Threading.Thread.Sleep(20)
            WriteAt(DripStartX, DripStartY + inc - 1, "o") : System.Threading.Thread.Sleep(20)
            WriteAt(DripStartX, DripStartY + inc - 1, " ") : System.Threading.Thread.Sleep(20)

            ' ok enough fun for now, start the game.
            PressAKey()

        End Sub

        <DebuggerStepThrough()> Friend Sub DeathScreen()
            Dim strName As String,
            strClass As String,
            strRace As String,
            strPronoun As String,
            strCapPronoun As String,
            strKilledBy As String,
            strArticle As String,
            intClassColor As Integer,
            intTurns As Integer,
            intKills As Integer,
            intDeathLevel As Integer,
            strSleep As String = ""

            With TheHero
                strName = .Name
                strPronoun = GetPronoun(.Gender)
                strCapPronoun = UCase(Left(GetPronoun(.Gender), 1)) & Mid(GetPronoun(.Gender), 2)
                strKilledBy = .KilledBy

                strClass = GetClass(.HeroClass)
                If Len(strClass) > 0 Then strClass = " the " + strClass

                strRace = GetRace(.HeroRace)
                intClassColor = .Color
                intTurns = .TurnCount
                intKills = .TotalKills
                intDeathLevel = .CurrentLevel

                If .Sleeping Then strSleep = " while sleeping"
            End With

            Clear()

            strArticle = "a"
            If Left(strKilledBy.ToLower, 1) = "a" _
        Or Left(strKilledBy.ToLower, 1) = "e" _
        Or Left(strKilledBy.ToLower, 1) = "i" _
        Or Left(strKilledBy.ToLower, 1) = "o" _
        Or Left(strKilledBy.ToLower, 1) = "u" Then strArticle += "n"

            WriteAt(2, 2, String.Format("Here lies {0}{1} killed{2} on level {3} by {4} {5}.", strName, strClass, strSleep, intDeathLevel, strArticle, strKilledBy))
            WriteAt(2, 4, "------------------------------------------------------------------------")
            WriteAt(2, 5, String.Format("Most folks agreed, {0} was a pretty decent {1}!", strName, strRace))
            WriteAt(2, 6, String.Format("{0} lived for {1} turns and killed {2} monsters,", strCapPronoun, intTurns, intKills))

            Select Case intKills
                Case 0
                    WriteAt(2, 7, "before slinking off to the afterlife.")
                    WriteAt(2, 8, String.Format("Sadly, {0} won't be missed!", strPronoun))
                Case 1 To 40
                    WriteAt(2, 7, "before shuffling off to the afterlife.")
                    WriteAt(2, 8, String.Format("Who knows? {0} might be missed!", strCapPronoun))
                Case 41 To 180
                    WriteAt(2, 7, "before marching off to the afterlife.")
                    WriteAt(2, 8, strCapPronoun & " will be sorely missed!")
                Case 181 To 500
                    WriteAt(2, 7, "before storming off to the afterlife. The heavens cried for a hundred days.")
                    WriteAt(2, 8, strCapPronoun & " will be mourned for decades!")
                Case 501 To 1000
                    WriteAt(2, 7, "before deciding to conquer the afterlife. Even death could not slow this noble warrior.")
                    WriteAt(2, 8, strCapPronoun & " will live on in song and story for many generations!")
                Case Is > 1000
                    WriteAt(2, 7, "before challenging the god of death himself.")
                    WriteAt(2, 8, String.Format("Some say {0} still fights on in hell!", strCapPronoun))
            End Select

            WriteAt(2, 9, "------------------------------------------------------------------------")

            WriteAt(20, 12, "________", ConsoleColor.DarkGray)
            WriteAt(19, 13, "/        \", ConsoleColor.DarkGray)
            WriteAt(19, 14, "| R.I.P. |", ConsoleColor.DarkGray)
            WriteAt(19, 15, "|        |", ConsoleColor.DarkGray)
            WriteAt(19, 16, "|  /||\  |", ConsoleColor.DarkGray)
            WriteAt(19, 17, "|  ----  |", ConsoleColor.DarkGray)
            WriteAt(19, 18, "|  \||/  |", ConsoleColor.DarkGray)
            WriteAt(19, 19, "|        |", ConsoleColor.DarkGray)

            WriteAt(31, 19, "\|/", ConsoleColor.Green)

            WriteAt(16, 20, "~~~~~~~~~~~~~~~~~~~~~~", ConsoleColor.DarkYellow)

            WriteAt(30, 19, "o", ConsoleColor.Gray)
            WriteAt(34, 19, "o", ConsoleColor.Gray)

            WriteAt(32, 18, "@")

            PressAKey()
        End Sub

        Friend Sub HighScoreScreen()
            'TODO: high score screen
        End Sub

        Friend Sub ShowMessageBuffer()
            Dim strAnswer As ConsoleKeyInfo,
            showBuffer As Boolean,
            ok As Boolean

            Do While Not ok
                WriteAt(1, 0, CLEARSPACE)
                WriteAt(1, 1, CLEARSPACE)
                WriteAt(1, 0, "Do you want to review the final messages? (Y/n)")

                strAnswer = ReadKey()
                Select Case strAnswer.KeyChar.ToString
                    Case Chr(13)
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showBuffer = True
                    Case "n", "N"
                        ok = True
                    Case "y", "Y"
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showBuffer = True
                End Select
            Loop

            If showBuffer Then
                Console.Clear()
                WriteAt(20, 0, "*~*~* Heroic Adventure Message Log *~*~*")

                Dim arrMessage() As String, sbr As New StringBuilder,
                intWordCtr As Int16,
                intLastWord As Int16, intTotalWords As Int16,
                intStartWord As Int16 = 0, yPos As Int16 = 2

                Do Until m_qMessage.Count = 0
                    ' break the message up into words
                    arrMessage = Split(m_qMessage.Dequeue)

                    ' get a word count
                    intTotalWords = arrMessage.Length - 1
                    intLastWord = 0

                    ' build the 1st line
                    For intWordCtr = intStartWord To intTotalWords
                        If sbr.Length + arrMessage(intWordCtr).Length < 80 Then
                            sbr.Append(arrMessage(intWordCtr))
                            sbr.Append(" ")
                            intLastWord = intWordCtr
                        Else
                            sbr.AppendLine()
                            intLastWord = intWordCtr
                            Exit For
                        End If
                    Next

                    ' show the first line
                    WriteAt(1, yPos, sbr.ToString)
                    sbr.Remove(0, sbr.Length)

                    ' build the remaining lines
                    Do While intLastWord < intTotalWords
                        yPos += 1
                        For intWordCtr = intLastWord To intTotalWords
                            If sbr.Length + arrMessage(intWordCtr).Length <= 80 Then
                                sbr.Append(arrMessage(intWordCtr))
                                sbr.Append(" ")
                                intLastWord = intWordCtr
                            Else
                                intLastWord = intWordCtr
                                intStartWord = intLastWord
                                Exit For
                            End If
                        Next

                        ' show the next line
                        WriteAt(1, yPos, sbr.ToString)
                        sbr.Remove(0, sbr.Length)
                    Loop
                    yPos += 1
                Loop

                sbr = Nothing

            End If

            PressAKey()
        End Sub

        Friend Sub ShowFullOverlandMap()
            Dim strAnswer As ConsoleKeyInfo,
            showworld As Boolean,
            ok As Boolean

            Do While Not ok
                WriteAt(1, 0, CLEARSPACE)
                WriteAt(1, 1, CLEARSPACE)
                WriteAt(1, 0, "Do you want to see a map of the entire world? (Y/n)")

                strAnswer = ReadKey()

                Select Case strAnswer.KeyChar.ToString
                    Case Chr(13)
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showworld = True
                    Case "n", "N"
                        ok = True
                    Case "y", "Y"
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showworld = True
                End Select
            Loop

            If showworld Then
                Clear()

                Dim intCtrX As Integer, intCtrY As Integer

                ' go through the array of the level and build a string
                For intCtrY = 0 To 17
                    For intCtrX = 0 To 72
                        Select Case OverlandMap(intCtrX, intCtrY).TerrainType
                            Case Overland.TerrainType.Void
                            ' Do nothing, this area is just black.

                            Case Overland.TerrainType.Impassable
                                WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.Gray)

                            Case Overland.TerrainType.Mountain
                                WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.DarkGray)

                            Case Overland.TerrainType.Volcano
                                WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.Red)

                            Case Overland.TerrainType.Hills
                                WriteAt(intCtrX + 1, intCtrY + 3, "~", ConsoleColor.DarkYellow)

                            Case Overland.TerrainType.Plains
                                WriteAt(intCtrX + 1, intCtrY + 3, Chr(34), ConsoleColor.Green)

                            Case Overland.TerrainType.Road
                                WriteAt(intCtrX + 1, intCtrY + 3, ".", ConsoleColor.DarkYellow)

                            Case Overland.TerrainType.Forest
                                WriteAt(intCtrX + 1, intCtrY + 3, "&", ConsoleColor.DarkGreen)

                            Case Overland.TerrainType.Special
                                WriteAt(intCtrX + 1, intCtrY + 3, "*", ConsoleColor.DarkGray)

                            Case Overland.TerrainType.Town
                                WriteAt(intCtrX + 1, intCtrY + 3, "o", ConsoleColor.DarkYellow)

                            Case Overland.TerrainType.Water
                                WriteAt(intCtrX + 1, intCtrY + 3, "=", ConsoleColor.Blue)

                        End Select
                    Next
                Next
            End If
        End Sub

        <DebuggerStepThrough()> Friend Sub ShowFullMapScreen()
            Dim strAnswer As ConsoleKeyInfo,
            showlevel As Boolean,
            ok As Boolean

            Do While Not ok
                WriteAt(1, 0, CLEARSPACE)
                WriteAt(1, 1, CLEARSPACE)
                WriteAt(1, 0, "Do you want to see a map of the entire level? (Y/n)")

                strAnswer = ReadKey()

                Select Case strAnswer.KeyChar.ToString
                    Case Chr(13)
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showlevel = True
                    Case "n", "N"
                        ok = True
                    Case "y", "Y"
                        WriteAt(1, 0, CLEARSPACE)
                        ok = True
                        showlevel = True
                End Select
            Loop

            If showlevel Then
                Dim intCtrX As Integer, intCtrY As Integer, Z As Integer

                ' set Z to the correct level
                Z = TheHero.LocZ

                ' go through the array of the level and build a string
                For intCtrY = 0 To CInt(22)
                    For intCtrX = 0 To CInt(60)
                        Select Case Level(intCtrX, intCtrY, Z).FloorType
                            Case SquareType.Rock
                            ' do nothing, black/black is the default
                            Case SquareType.Trap
                                WriteAt(intCtrX, intCtrY, "^", Level(intCtrX, intCtrY, Z).TrapType)
                            Case SquareType.Wall
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
                            Case SquareType.NWCorner
                                ' Eventually replace with appropriate corner tile
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
                            Case SquareType.NECorner
                                ' Eventually replace with appropriate corner tile
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
                            Case SquareType.SECorner
                                ' Eventually replace with appropriate corner tile
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)
                            Case SquareType.SWCorner
                                ' Eventually replace with appropriate corner tile
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkGray)

                            Case SquareType.Floor
                                WriteAt(intCtrX, intCtrY, ".", ConsoleColor.Gray)

                            Case SquareType.Door
                                WriteAt(intCtrX, intCtrY, "+", ConsoleColor.DarkYellow)

                            Case SquareType.Secret
                                WriteAt(intCtrX, intCtrY, "#", ConsoleColor.DarkYellow)

                            Case SquareType.OpenDoor
                                WriteAt(intCtrX, intCtrY, "/", ConsoleColor.DarkYellow)

                            Case SquareType.StairsUp
                                WriteAt(intCtrX, intCtrY, "<", ConsoleColor.Gray)

                            Case SquareType.StairsDn
                                WriteAt(intCtrX, intCtrY, ">", ConsoleColor.Gray)
                        End Select
                    Next
                Next

                RedrawItems()
                RedrawMonsters(True)
            End If
        End Sub

        <System.Diagnostics.DebuggerStepThrough()> Friend Sub DungeonCreationFailureScreen()
            Dim ver As String = "v" & m_version.ToString

            Clear()

            WriteAt(5, 10, "Elminster, we have a problem...")

            WriteAt(5, 12, "If you would like to help, please copy this:")
            WriteAt(5, 13, "DunGen3Error " & ver & " - failtocomplete", ConsoleColor.Yellow)
            WriteAt(5, 14, "and send it to:      chrisgwilliams@gmail.com")

            WriteAt(5, 17, "Sorry for any inconvenience, this is a pretty")
            WriteAt(5, 18, "uncommon error, so it probably won't re-occur")
            WriteAt(5, 19, "if you restart the game.")

            PressAKey()
        End Sub

#End Region

    End Module
End Namespace