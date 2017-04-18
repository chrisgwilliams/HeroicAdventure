Partial Public Class TownFactory
    Public Shared Sub AbandonedVillage()
        Dim intXctr As Int16, intYctr As Int16

        ' Abandoned Village
        For intXctr = 0 To 78
            For intYctr = 0 To 17
                m_TownMap(intXctr, intYctr, Town.Abandoned).CellType = Towns.CellType.grass
            Next
        Next

    End Sub
End Class
