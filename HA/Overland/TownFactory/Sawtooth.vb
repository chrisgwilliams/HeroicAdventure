Partial Public Class TownFactory

    Public Shared Sub Sawtooth()
        Dim intXctr As Int16, intYctr As Int16

        ' Sawtooth
        For intXctr = 0 To 78
            For intYctr = 0 To 17
				m_TownMap(intXctr, intYctr, Town.Sawtooth).CellType = Towns.CellType.grass
				m_TownMap(intXctr, intYctr, Town.Sawtooth).items = New ArrayList()
            Next
        Next

    End Sub

End Class
