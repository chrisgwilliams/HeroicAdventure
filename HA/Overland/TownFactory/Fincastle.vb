Imports HA.Common

Partial Public Class TownFactory

    Public Shared Sub Fincastle()
        Dim intXctr As Int16, intYctr As Int16

        ' Fincastle
        For intXctr = 0 To 78
            For intYctr = 0 To 17
                m_TownMap(intXctr, intYctr, Town.Fincastle).CellType = Towns.CellType.grass
                m_TownMap(intXctr, intYctr, Town.Fincastle).items = New ArrayList()
            Next
        Next
        GenericBuilding(4, 4, 8, 5, Heading.East, DoorState.none, Town.Fincastle)
        GenericBuilding(9, 11, 5, 5, Heading.North, DoorState.none, Town.Fincastle)
        GenericBuilding(18, 3, 11, 5, Heading.South, DoorState.none, Town.Fincastle)
        GenericBuilding(33, 4, 5, 4, Heading.South, DoorState.none, Town.Fincastle)

        ' composite building
        GenericBuilding(27, 10, 5, 5, Heading.North, DoorState.none, Town.Fincastle)
        GenericBuilding(31, 10, 5, 5, Heading.West, DoorState.none, Town.Fincastle)

        ' bridge
        For intXctr = 42 To 48
            m_TownMap(intXctr, 7, Town.Fincastle).CellType = Towns.CellType.wall
            m_TownMap(intXctr, 10, Town.Fincastle).CellType = Towns.CellType.wall
        Next

        ' east town
        For intXctr = 52 To 74
            m_TownMap(intXctr, 2, Town.Fincastle).CellType = Towns.CellType.wall
            m_TownMap(intXctr, 15, Town.Fincastle).CellType = Towns.CellType.wall
        Next
        For intYctr = 2 To 15
            m_TownMap(74, intYctr, Town.Fincastle).CellType = Towns.CellType.wall
            m_TownMap(52, intYctr, Town.Fincastle).CellType = Towns.CellType.wall
        Next
        m_TownMap(52, 8, Town.Fincastle).CellType = Towns.CellType.grass
        m_TownMap(52, 9, Town.Fincastle).CellType = Towns.CellType.grass
        GenericBuilding(60, 4, 7, 5, Heading.South, DoorState.none, Town.Fincastle)
        GenericBuilding(55, 4, 6, 5, Heading.East, DoorState.none, Town.Fincastle)
        GenericBuilding(66, 4, 6, 5, Heading.West, DoorState.none, Town.Fincastle)
        GenericBuilding(60, 10, 7, 4, Heading.North, DoorState.none, Town.Fincastle)

        ' creek
        For intYctr = 0 To 6
            m_TownMap(44, intYctr, Town.Fincastle).CellType = Towns.CellType.water
            m_TownMap(45, intYctr, Town.Fincastle).CellType = Towns.CellType.water
            m_TownMap(46, intYctr, Town.Fincastle).CellType = Towns.CellType.water
        Next
        For intYctr = 11 To 17
            m_TownMap(44, intYctr, Town.Fincastle).CellType = Towns.CellType.water
            m_TownMap(45, intYctr, Town.Fincastle).CellType = Towns.CellType.water
            m_TownMap(46, intYctr, Town.Fincastle).CellType = Towns.CellType.water
        Next

        ' trees
        m_TownMap(2, 2, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(3, 12, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(4, 13, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(14, 5, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(15, 3, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(32, 2, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(40, 15, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(41, 14, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(42, 3, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(42, 2, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(48, 4, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(49, 13, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(55, 11, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(56, 12, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(70, 12, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(71, 13, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(71, 10, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(76, 8, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(77, 10, Town.Fincastle).CellType = Towns.CellType.tree

        ' lake
        m_TownMap(18, 12, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(19, 12, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(20, 12, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(17, 13, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(18, 13, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(19, 13, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(20, 13, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(18, 14, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(19, 14, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(20, 14, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(21, 14, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(17, 15, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(18, 15, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(19, 15, Town.Fincastle).CellType = Towns.CellType.water
        m_TownMap(17, 14, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(18, 11, Town.Fincastle).CellType = Towns.CellType.tree
        m_TownMap(21, 13, Town.Fincastle).CellType = Towns.CellType.tree

        'TODO: add Village Elder NPC to Fincastle initializer

    End Sub

End Class
