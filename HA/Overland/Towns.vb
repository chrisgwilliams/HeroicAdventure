Imports RND = DBuild.MersenneTwister
Imports System.Console
Imports HA.Common

Public Module Towns
#Region " Module Level Variables "
    Friend m_TownMap(79, 18, 5) As TownCell

#End Region

#Region " Buildings "

    Public Sub GenericBuilding(ByVal startX As Int16, ByVal startY As Int16, _
                               ByVal width As Int16, ByVal height As Int16, _
                               ByVal door As Heading, ByVal doorstate As DoorState, _
                               ByVal town As Town)

        'TODO: modify GenericBuilding() to allow overlapping walls without overwriting doors

        '.......   
        '.#####.   creates a square or rectangular building
        '.#...#.   starting at the top left (startX & startY)
        '.#...#.   and going to (width - 1) and (height - 1) 
        '.#...#.   door placement is passed in as heading (NSEW)
        '.#####.   
        '.......   town indicates the Z axis the map is stored on

        Dim intCtr As Int16

        ' N wall
        For intCtr = 0 To width - 1
            m_TownMap(startX + intCtr, startY, town).CellType = CellType.wall
        Next
        ' W wall
        For intCtr = 1 To height - 2
            m_TownMap(startX, startY + intCtr, town).CellType = CellType.wall
        Next
        ' S wall
        For intCtr = 0 To width - 1
            m_TownMap(startX + intCtr, startY + (height - 1), town).CellType = CellType.wall
        Next
        ' E wall
        For intCtr = 1 To height - 2
            m_TownMap(startX + (width - 1), startY + intCtr, town).CellType = CellType.wall
        Next
        ' door
        Select Case door
            Case Heading.North
                Select Case doorstate
                    Case Towns.DoorState.none
                        m_TownMap(startX + (width \ 2), startY, town).CellType = CellType.grass
                    Case Towns.DoorState.closed, Towns.DoorState.locked
                        m_TownMap(startX + (width \ 2), startY, town).CellType = CellType.door
                        m_TownMap(startX + (width \ 2), startY, town).DoorState = doorstate
                    Case Towns.DoorState.open
                        m_TownMap(startX + (width \ 2), startY, town).CellType = CellType.opendoor
                        m_TownMap(startX + (width \ 2), startY, town).DoorState = doorstate
                End Select

            Case Heading.West
                Select Case doorstate
                    Case Towns.DoorState.none
                        m_TownMap(startX, startY + (height \ 2), town).CellType = CellType.grass
                    Case Towns.DoorState.closed, Towns.DoorState.locked
                        m_TownMap(startX, startY + (height \ 2), town).CellType = CellType.door
                        m_TownMap(startX, startY + (height \ 2), town).DoorState = doorstate
                    Case Towns.DoorState.open
                        m_TownMap(startX, startY + (height \ 2), town).CellType = CellType.opendoor
                        m_TownMap(startX, startY + (height \ 2), town).DoorState = doorstate
                End Select

            Case Heading.South
                Select Case doorstate
                    Case Towns.DoorState.none
                        m_TownMap(startX + (width \ 2), startY + height - 1, town).CellType = CellType.grass
                    Case Towns.DoorState.closed, Towns.DoorState.locked
                        m_TownMap(startX + (width \ 2), startY + height - 1, town).CellType = CellType.door
                        m_TownMap(startX + (width \ 2), startY + height - 1, town).DoorState = doorstate
                    Case Towns.DoorState.open
                        m_TownMap(startX + (width \ 2), startY + height - 1, town).CellType = CellType.opendoor
                        m_TownMap(startX + (width \ 2), startY + height - 1, town).DoorState = doorstate
                End Select

            Case Heading.East
                Select Case doorstate
                    Case Towns.DoorState.none
                        m_TownMap(startX + width - 1, startY + (height \ 2), town).CellType = CellType.grass
                    Case Towns.DoorState.closed, Towns.DoorState.locked
                        m_TownMap(startX + width - 1, startY + (height \ 2), town).CellType = CellType.door
                        m_TownMap(startX + width - 1, startY + (height \ 2), town).DoorState = doorstate
                    Case Towns.DoorState.open
                        m_TownMap(startX + width - 1, startY + (height \ 2), town).CellType = CellType.opendoor
                        m_TownMap(startX + width - 1, startY + (height \ 2), town).DoorState = doorstate
                End Select
        End Select

    End Sub

#End Region

#Region " Town Subs and Functions "

    Friend Function DoTown(ByVal intTown As Int16) As String
        DoTown = ""

        Clear()
        CursorVisible = False
        TheHero.LocX = 3
        TheHero.LocY = 12
        TheHero.Town = intTown
        TheHero.InTown = True

        ReDrawTown(intTown)
        TownLOS(intTown)

		WriteAt(TheHero.LocX + 1, TheHero.LocY + 3, TheHero.Icon, TheHero.Color)

    End Function

    Friend Sub ReDrawTown(ByVal town As Town)
        Dim intXctr As Int16, intYctr As Int16

        For intXctr = 0 To 78
            For intYctr = 0 To 17
                With m_TownMap(intXctr, intYctr, town)
                    Select Case .CellType
                        Case CellType.Void
                            ' do nothing, black/black is the default
                        Case CellType.grass
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, ".", ConsoleColor.Green)
                            End If

                        Case CellType.wall
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, "#", ConsoleColor.DarkGray)
                            End If

                        Case CellType.door
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, "+", ConsoleColor.DarkYellow)
                            End If

                        Case CellType.opendoor
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, "/", ConsoleColor.DarkYellow)
                            End If

                        Case CellType.tree
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, "T", ConsoleColor.DarkGreen)
                            End If

                        Case CellType.water
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, "=", ConsoleColor.Blue)
                            End If

                        Case CellType.floor
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, ".", ConsoleColor.Gray)
                            End If

                        Case CellType.road
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, ".", ConsoleColor.DarkYellow)
                            End If

                        Case CellType.StairsDn
                            If .Observed Then
                                WriteAt(intXctr + 1, intYctr + 3, ">", ConsoleColor.Gray)
                            End If

                    End Select
                End With
            Next
        Next

    End Sub

    Friend Sub TownLOS(ByVal town As Town)
        Dim MinX As Int16, MinY As Int16, _
            MaxX As Int16, MaxY As Int16, _
            xCtr As Int16, yCtr As Int16

        ' adjust sight radius for when we get to close to the edges
        MinX = TheHero.LocX - (TheHero.Sight + 3)
        If MinX < 0 Then MinX = 0
        MaxX = TheHero.LocX + TheHero.Sight + 3
        If MaxX > 78 Then MaxX = 78

        MinY = TheHero.LocY - (TheHero.Sight + 2)
        If MinY < 0 Then MinY = 0
        MaxY = TheHero.LocY + TheHero.Sight + 2
        If MaxY > 17 Then MaxY = 17

        For yCtr = MinY + 1 To MaxY - 1
            For xCtr = MinX To MaxX
                With m_TownMap(xCtr, yCtr, town)
                    If .Observed = False Then
                        .Observed = True
                    End If
                    If xCtr = TheHero.LocX AndAlso yCtr = TheHero.LocY Then
                        ' skip this tile, it's the hero
                    Else
                        FixTown(xCtr, yCtr)
                    End If
                End With
            Next
        Next

        For xCtr = MinX + 1 To MaxX - 1
            For yCtr = MinY To MaxY
                With m_TownMap(xCtr, yCtr, town)
                    If .Observed = False Then
                        .Observed = True
                    End If
                    If xCtr = TheHero.LocX AndAlso yCtr = TheHero.LocY Then
                        ' skip this tile, it's the hero
                    Else
                        FixTown(xCtr, yCtr)
                    End If
                End With
            Next
        Next

    End Sub

    Public Sub FixTown(ByVal x As Int16, ByVal y As Int16)
        Dim town As Int16 = TheHero.Town
        With m_TownMap(x, y, town)
            Select Case .CellType
                Case CellType.Void
                    ' do nothing, black/black is the default
                Case CellType.grass
                    WriteAt(x + 1, y + 3, ".", ConsoleColor.Green)
                Case CellType.wall
                    WriteAt(x + 1, y + 3, "#", ConsoleColor.DarkGray)
                Case CellType.door
                    WriteAt(x + 1, y + 3, "+", ConsoleColor.DarkYellow)
                Case CellType.opendoor
                    WriteAt(x + 1, y + 3, "/", ConsoleColor.DarkYellow)
                Case CellType.tree
                    WriteAt(x + 1, y + 3, "T", ConsoleColor.DarkGreen)
                Case CellType.water
                    WriteAt(x + 1, y + 3, "=", ConsoleColor.Blue)
                Case CellType.floor
                    WriteAt(x + 1, y + 3, ".", ConsoleColor.Gray)
                Case CellType.road
                    WriteAt(x + 1, y + 3, ".", ConsoleColor.DarkYellow)
                Case CellType.StairsDn
                    WriteAt(x + 1, y + 3, ">", ConsoleColor.Gray)
            End Select
        End With
    End Sub

#End Region

#Region " Town Structures and Enums "

    Public Enum CellType
        Void = 0            '    Unreachable Void
        grass = 1           ' .  grass (green)
        wall = 2            ' #  wall
        door = 3            ' +  door
        tree = 4            ' T  tree
        water = 5           ' =  water
        floor = 6           ' .  floor (white)
        road = 7            ' .  road (brown)
        StairsDn = 8        ' >  stairs down
        opendoor = 9        ' /  open door
    End Enum

    Public Structure TownCell
        Dim CellType As CellType
        Dim Observed As Boolean
		Dim DoorState As DoorState
		Dim NPC As Integer
		Dim TrapDiscovered As Boolean
		Dim items As ArrayList
    End Structure

    Public Enum DoorState
        none = 0
        open = 1
        closed = 2
        locked = 3
    End Enum

    Public Enum Town
        Fincastle = 0
        Abandoned = 1
        Lakeside = 2
        Sawtooth = 3
        Stonegate = 4
    End Enum
#End Region

End Module
