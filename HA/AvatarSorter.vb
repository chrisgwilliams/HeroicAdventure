'********************************************************************************************************
'Cullen's changes for sorting characters by initiative

Public Class AvatarSorter
    Implements Generic.IComparer(Of Avatar)

    'In our custom compare routine, we're comparing the avatars by their TotalInit values.  Higher inits get put
    'at the front of the line
    Friend Function Compare(ByVal x As Avatar, ByVal y As Avatar) As Integer Implements Generic.IComparer(Of HA.Avatar).Compare

        If x.TotalInitForRound > y.TotalInitForRound Then
            Return -1
        ElseIf x.TotalInitForRound = y.TotalInitForRound Then
            Return 0
        Else
            Return 1
        End If

    End Function
End Class
'end Cullen's changes for sorting characters by initiative
'********************************************************************************************************
