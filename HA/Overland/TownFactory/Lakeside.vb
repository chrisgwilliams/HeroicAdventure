Partial Public Class TownFactory

    Public Shared Sub Lakeside()
        Dim intXctr As Int16, intYctr As Int16

        ' Lakeside
        For intXctr = 0 To 78
            For intYctr = 0 To 17
				m_TownMap(intXctr, intYctr, Town.Lakeside).CellType = Towns.CellType.grass
				m_TownMap(intXctr, intYctr, Town.Lakeside).items = New ArrayList()
            Next
        Next

    End Sub

End Class
