Imports RND = DBuild.MersenneTwister
Imports System.Console
Imports HA.Common

Module Wilderness
#Region " Module Level Variables "
    Friend m_TerrainMap(79, 18, 9) As TerrainCell

#End Region

#Region " Wilderness Subs and Functions "
    Friend Function DoTerrain(ByVal intTerrain As Integer) As String
        DoTerrain = ""

        Clear()
        CursorVisible = False
        TheHero.LocX = 40
        TheHero.LocY = 9
        TheHero.TerrainZoom = True

        ' populate map
        RenderTerrain(intTerrain)

        ReDrawTerrain(intTerrain)
        TerrainLOS(intTerrain)

		WriteAt(TheHero.LocX + 1, TheHero.LocY + 3, TheHero.Icon, TheHero.Color)

    End Function

    Friend Sub RenderTerrain(ByVal terrain As TerrainType)
        Dim intXCtr, intYCtr As Integer

        ' clear the observed state & cover the field with grass before adding any special tiles
        For intXCtr = 0 To 78
            For intYCtr = 0 To 17
                m_TerrainMap(intXCtr, intYCtr, terrain).Observed = False
                m_TerrainMap(intXCtr, intYCtr, terrain).CellType = CellType.grass
                m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.Green
            Next
        Next

        Select Case terrain
            Case TerrainType.Road
                Dim RoadDirection As RoadAlignment

                If (OverlandMap(TheHero.OverX - 1, TheHero.OverY).TerrainType = TerrainType.Road Or OverlandMap(TheHero.OverX - 1, TheHero.OverY).TerrainType = TerrainType.Mountain) _
                    And OverlandMap(TheHero.OverX + 1, TheHero.OverY).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX, TheHero.OverY + 1).TerrainType <> TerrainType.Road Then
                    RoadDirection = RoadAlignment.EastWest
                    Dim yBase As Integer
                    intYCtr = 0
                    For intXCtr = 0 To 78
                        intYCtr = RND.Next(intYCtr - 1, intYCtr + 1)
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intXCtr += 1
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intYCtr > 2 Then intYCtr = 2
                        If intYCtr < -2 Then intYCtr = -2
                    Next

                ElseIf OverlandMap(TheHero.OverX, TheHero.OverY - 1).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX, TheHero.OverY + 1).TerrainType = TerrainType.Road Then
                    RoadDirection = RoadAlignment.NorthSouth

                    Dim Xbase As Integer
                    intXCtr = 0
                    For intYCtr = 0 To 17
                        intXCtr = RND.Next(intXCtr - 1, intXCtr + 1)
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intYCtr += 1
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intXCtr > 2 Then intXCtr = 2
                        If intXCtr < -2 Then intXCtr = -2
                    Next

                ElseIf OverlandMap(TheHero.OverX, TheHero.OverY - 1).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX + 1, TheHero.OverY).TerrainType = TerrainType.Road Then
                    RoadDirection = RoadAlignment.NorthEast

                    Dim Xbase As Integer
                    intXCtr = 0
                    For intYCtr = 0 To 8
                        intXCtr = RND.Next(intXCtr - 1, intXCtr + 1)
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intYCtr += 1
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intXCtr > 2 Then intXCtr = 2
                        If intXCtr < -2 Then intXCtr = -2
                    Next
                    Dim yBase As Integer
                    intYCtr = 0
                    For intXCtr = 39 To 78
                        intYCtr = RND.Next(intYCtr - 1, intYCtr + 1)
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intXCtr += 1
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intYCtr > 2 Then intYCtr = 2
                        If intYCtr < -2 Then intYCtr = -2
                    Next

                ElseIf OverlandMap(TheHero.OverX, TheHero.OverY - 1).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX - 1, TheHero.OverY).TerrainType = TerrainType.Road Then
                    RoadDirection = RoadAlignment.NorthWest

                    Dim Xbase As Integer
                    intXCtr = 0
                    For intYCtr = 0 To 8
                        intXCtr = RND.Next(intXCtr - 1, intXCtr + 1)
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intYCtr += 1
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intXCtr > 2 Then intXCtr = 2
                        If intXCtr < -2 Then intXCtr = -2
                    Next

                    Dim yBase As Integer
                    Dim xStop As Integer = Xbase + intXCtr
                    intYCtr = 0
                    For intXCtr = 0 To xStop
                        intYCtr = RND.Next(intYCtr - 1, intYCtr + 1)
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intXCtr += 1
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intYCtr > 2 Then intYCtr = 2
                        If intYCtr < -2 Then intYCtr = -2
                    Next

                ElseIf OverlandMap(TheHero.OverX, TheHero.OverY + 1).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX + 1, TheHero.OverY).TerrainType = TerrainType.Road Then
                    RoadDirection = RoadAlignment.SouthEast

                    Dim yBase As Integer
                    intYCtr = 0
                    For intXCtr = 39 To 78
                        intYCtr = RND.Next(intYCtr - 1, intYCtr + 1)
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intXCtr += 1
                        For yBase = 7 To 10
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(intXCtr, yBase + intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intYCtr > 2 Then intYCtr = 2
                        If intYCtr < -2 Then intYCtr = -2
                    Next

                    Dim Xbase As Integer
                    intXCtr = 0
                    For intYCtr = 8 To 17
                        intXCtr = RND.Next(intXCtr - 1, intXCtr + 1)
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        intYCtr += 1
                        For Xbase = 37 To 42
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).CellType = CellType.road
                            m_TerrainMap(Xbase + intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                        Next
                        If intXCtr > 2 Then intXCtr = 2
                        If intXCtr < -2 Then intXCtr = -2
                    Next


                ElseIf OverlandMap(TheHero.OverX, TheHero.OverY + 1).TerrainType = TerrainType.Road _
                    And OverlandMap(TheHero.OverX - 1, TheHero.OverY).TerrainType = TerrainType.Road Then
                    RoadDirection = RoadAlignment.SouthWest
                End If

                ' now that the road has been mapped, place random trees on the rest of the map
                ' trees should be placed at least 1 square away from the road, not adjacent
                Dim MaxTrees As Integer = RND.Next(60, 175)
                Dim TreeCtr As Integer

                For TreeCtr = 1 To MaxTrees
                    intXCtr = RND.Next(1, 77)
                    intYCtr = RND.Next(1, 16)

                    If m_TerrainMap(intXCtr, intYCtr, terrain).CellType = CellType.grass _
                    And m_TerrainMap(intXCtr - 1, intYCtr - 1, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr, intYCtr - 1, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr + 1, intYCtr - 1, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr - 1, intYCtr, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr + 1, intYCtr, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr - 1, intYCtr + 1, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr, intYCtr + 1, terrain).CellType <> CellType.road _
                    And m_TerrainMap(intXCtr + 1, intYCtr + 1, terrain).CellType <> CellType.road Then
                        m_TerrainMap(intXCtr, intYCtr, terrain).CellType = CellType.tree
                        If D4() < 3 Then
                            m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.Green
                        Else
                            m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkGreen
                        End If
                    End If
                Next

            Case TerrainType.Forest
                Dim MaxTrees As Integer = RND.Next(500, 700)
                Dim TreeCtr As Integer

                For TreeCtr = 1 To MaxTrees
                    intXCtr = RND.Next(1, 77)
                    intYCtr = RND.Next(1, 16)

                    m_TerrainMap(intXCtr, intYCtr, terrain).CellType = CellType.tree
                    If D4() < 3 Then
                        m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.Green
                    Else
                        m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkGreen
                    End If
                Next

            Case TerrainType.Plains
                ' do nothing, we already have grass

            Case TerrainType.Hills
                Dim MaxHills As Integer = RND.Next(500, 700)
                Dim HillCtr As Integer

                For HillCtr = 1 To MaxHills
                    intXCtr = RND.Next(1, 77)
                    intYCtr = RND.Next(1, 16)

                    m_TerrainMap(intXCtr, intYCtr, terrain).CellType = CellType.Hill
                    m_TerrainMap(intXCtr, intYCtr, terrain).Color = ConsoleColor.DarkYellow
                Next


            Case TerrainType.Water
                'TODO: Water terrain not implemented yet

        End Select
    End Sub

    Friend Sub ReDrawTerrain(ByVal terrain As TerrainType)
        Dim intXctr As Int16, intYctr As Int16

        For intXctr = 0 To 78
            For intYctr = 0 To 17
                With m_TerrainMap(intXctr, intYctr, terrain)
                    Select Case .CellType
                        Case CellType.Void
                            ' do nothing, black/black is the default
                        Case CellType.grass
                            If .Observed Then
                                ' ConsoleColor.Green
                                WriteAt(intXctr + 1, intYctr + 3, ".", m_TerrainMap(intXctr, intYctr, terrain).Color)
                            End If

                        Case CellType.tree
                            If .Observed Then
                                ' ConsoleColor.DarkGreen
                                WriteAt(intXctr + 1, intYctr + 3, "T", m_TerrainMap(intXctr, intYctr, terrain).Color)
                            End If

                        Case CellType.water
                            If .Observed Then
                                ' ConsoleColor.Blue
                                WriteAt(intXctr + 1, intYctr + 3, "=", m_TerrainMap(intXctr, intYctr, terrain).Color)
                            End If

                        Case CellType.road
                            If .Observed Then
                                ' ConsoleColor.DarkYellow
                                WriteAt(intXctr + 1, intYctr + 3, ".", m_TerrainMap(intXctr, intYctr, terrain).Color)
                            End If

                        Case CellType.hill
                            If .Observed Then
                                ' ConsoleColor.DarkYellow
                                WriteAt(intXctr + 1, intYctr + 3, "~", m_TerrainMap(intXctr, intYctr, terrain).Color)
                            End If

                    End Select
                End With
            Next
        Next
    End Sub

    Friend Sub TerrainLOS(ByVal terrain As TerrainType)
        Dim MinX As Int16, MinY As Int16, _
            MaxX As Int16, MaxY As Int16, _
            xCtr As Int16, yCtr As Int16

        ' we can see farther on the overland map, might adjust by race eventually
        Dim overSight As Integer = TheHero.Sight + 6

        ' adjust sight radius for when we get to close to the edges
        MinX = (TheHero.LocX - overSight) - 2
        If MinX < 0 Then MinX = 0
        MaxX = (TheHero.LocX + overSight) + 3
        If MaxX > 78 Then MaxX = 78

        MinY = (TheHero.LocY - overSight) - 2
        If MinY < 0 Then MinY = 0
        MaxY = (TheHero.LocY + overSight) + 2
        If MaxY > 17 Then MaxY = 17

        For yCtr = MinY + 1 To MaxY - 1
            For xCtr = MinX To MaxX
                With m_TerrainMap(xCtr, yCtr, terrain)
                    If .Observed = False Then
                        .Observed = True
                    End If
                    If xCtr = TheHero.LocX AndAlso yCtr = TheHero.LocY Then
                        ' skip this tile, it's the hero
                    Else
                        FixTerrain(xCtr, yCtr, terrain)
                    End If
                End With
            Next
        Next

        For xCtr = MinX + 1 To MaxX - 1
            For yCtr = MinY To MaxY
                With m_TerrainMap(xCtr, yCtr, terrain)
                    If .Observed = False Then
                        .Observed = True
                    End If
                    If xCtr = TheHero.LocX AndAlso yCtr = TheHero.LocY Then
                        ' skip this tile, it's the hero
                    Else
                        FixTerrain(xCtr, yCtr, terrain)
                    End If
                End With
            Next
        Next
    End Sub

    Public Sub FixTerrain(ByVal x As Int16, ByVal y As Int16, ByVal terrain As TerrainType)
        With m_TerrainMap(x, y, terrain)
            Select Case .CellType
                Case CellType.Void
                    ' do nothing, black/black is the default
                Case CellType.grass
                    WriteAt(x + 1, y + 3, ".", m_TerrainMap(x, y, terrain).Color)
                Case CellType.tree
                    WriteAt(x + 1, y + 3, "T", m_TerrainMap(x, y, terrain).Color)
                Case CellType.water
                    WriteAt(x + 1, y + 3, "=", m_TerrainMap(x, y, terrain).Color)
                Case CellType.road
                    WriteAt(x + 1, y + 3, ".", m_TerrainMap(x, y, terrain).Color)
                Case CellType.hill
                    WriteAt(x + 1, y + 3, "~", m_TerrainMap(x, y, terrain).Color)
            End Select
        End With
    End Sub


#End Region

#Region " Terrain Structures and Enums "

    Public Enum Heading
        North = 1
        NorthEast = 2   '8 1 2
        East = 3        '7 x 3
        SouthEast = 4   '6 5 4
        South = 5
        SouthWest = 6
        West = 7
        NorthWest = 8
    End Enum

    Public Enum CellType
        Void = 0            '    Unreachable Void
        grass = 1           ' .  grass (green)
        tree = 2            ' T  tree (brown or green)
        water = 3           ' =  water
        road = 4            ' .  road (brown)
        hill = 5            ' ~  hill (brown)
    End Enum

    Public Structure TerrainCell
        Dim CellType As Integer
        Dim Observed As Boolean
        Dim Color As ConsoleColor
    End Structure

    Public Enum RoadAlignment
        EastWest
        NorthSouth
        NorthEast
        NorthWest
        SouthEast
        SouthWest
    End Enum

#End Region

End Module
