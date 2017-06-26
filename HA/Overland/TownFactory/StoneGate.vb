Imports HA.Common

Partial Public Class TownFactory

    Public Shared Sub StoneGate()
        Dim intXctr As Int16, intYctr As Int16

        ' Stonegate
        For intXctr = 0 To 78
            For intYctr = 0 To 17
                m_TownMap(intXctr, intYctr, Town.Stonegate).CellType = Towns.CellType.grass
                m_TownMap(intXctr, intYctr, Town.Stonegate).items = New ArrayList()
            Next
        Next
        GenericBuilding(6, 2, 5, 4, Heading.South, DoorState.closed, Town.Stonegate) ' 1

        ' composite 2 & 3
        GenericBuilding(13, 2, 17, 4, Heading.South, DoorState.none, Town.Stonegate) ' 2
        GenericBuilding(13, 5, 5, 5, Heading.North, DoorState.none, Town.Stonegate) ' 3
        m_TownMap(13, 7, Town.Stonegate).CellType = Towns.CellType.grass

        GenericBuilding(5, 11, 15, 4, Heading.North, DoorState.none, Town.Stonegate) ' 4
        GenericBuilding(21, 12, 7, 4, Heading.North, DoorState.open, Town.Stonegate) ' 5
        GenericBuilding(22, 7, 7, 4, Heading.South, DoorState.none, Town.Stonegate) ' 6

        ' composite 7 & 8
        GenericBuilding(32, 3, 6, 7, Heading.West, DoorState.closed, Town.Stonegate)
        GenericBuilding(37, 6, 5, 4, Heading.South, DoorState.none, Town.Stonegate)

        GenericBuilding(31, 11, 13, 4, Heading.North, DoorState.none, Town.Stonegate) ' 9
        GenericBuilding(41, 2, 7, 3, Heading.South, DoorState.none, Town.Stonegate) ' 10

        ' composite 11 & 12
        GenericBuilding(46, 6, 5, 5, Heading.West, DoorState.none, Town.Stonegate)
        GenericBuilding(44, 6, 3, 3, Heading.West, DoorState.closed, Town.Stonegate)
        m_TownMap(46, 7, Town.Stonegate).CellType = Towns.CellType.grass

        GenericBuilding(53, 2, 7, 7, Heading.West, DoorState.none, Town.Stonegate)  ' 13
        GenericBuilding(61, 4, 5, 4, Heading.South, DoorState.none, Town.Stonegate) ' 14

        ' composite 15 & 16
        GenericBuilding(52, 11, 5, 5, Heading.North, DoorState.open, Town.Stonegate)
        GenericBuilding(46, 13, 7, 3, Heading.East, DoorState.closed, Town.Stonegate)
        m_TownMap(46, 14, Town.Stonegate).CellType = Towns.CellType.door
        m_TownMap(46, 14, Town.Stonegate).DoorState = DoorState.closed

        GenericBuilding(60, 10, 5, 5, Heading.North, DoorState.none, Town.Stonegate) ' 17

        ' special building at end of town (18)
        GenericBuilding(72, 6, 5, 6, Heading.North, DoorState.none, Town.Stonegate)
        m_TownMap(74, 6, Town.Stonegate).CellType = Towns.CellType.wall
        m_TownMap(72, 8, Town.Stonegate).CellType = Towns.CellType.door
        m_TownMap(72, 8, Town.Stonegate).DoorState = DoorState.open
        GenericBuilding(70, 4, 3, 3, Heading.West, DoorState.closed, Town.Stonegate)
        GenericBuilding(70, 11, 3, 3, Heading.West, DoorState.closed, Town.Stonegate)
        GenericBuilding(67, 2, 4, 3, Heading.West, DoorState.none, Town.Stonegate)
        m_TownMap(67, 3, Town.Stonegate).CellType = Towns.CellType.wall
        m_TownMap(67, 4, Town.Stonegate).CellType = Towns.CellType.grass
        m_TownMap(68, 4, Town.Stonegate).CellType = Towns.CellType.grass
        m_TownMap(69, 4, Town.Stonegate).CellType = Towns.CellType.grass
        GenericBuilding(67, 13, 4, 3, Heading.North, DoorState.none, Town.Stonegate)
        m_TownMap(67, 14, Town.Stonegate).CellType = Towns.CellType.wall
        m_TownMap(67, 13, Town.Stonegate).CellType = Towns.CellType.grass
        m_TownMap(68, 13, Town.Stonegate).CellType = Towns.CellType.grass
        m_TownMap(69, 13, Town.Stonegate).CellType = Towns.CellType.grass
        m_TownMap(74, 9, Town.Stonegate).CellType = Towns.CellType.StairsDn

        ' trees and water
        m_TownMap(4, 7, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(7, 8, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(4, 9, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(5, 7, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(6, 7, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(4, 8, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(5, 8, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(6, 8, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(5, 9, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(6, 9, Town.Stonegate).CellType = Towns.CellType.water
        m_TownMap(19, 8, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(20, 9, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(39, 4, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(44, 9, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(50, 3, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(50, 12, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(55, 9, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(58, 13, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(63, 2, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(67, 9, Town.Stonegate).CellType = Towns.CellType.tree
        m_TownMap(69, 8, Town.Stonegate).CellType = Towns.CellType.tree

    End Sub
End Class
