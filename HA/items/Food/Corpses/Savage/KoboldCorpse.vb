Imports HA
Imports HA.Common

Public Class KoboldCorpse
    Inherits Corpse

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Not Cooked Then
        End If

        ' check ItemState for blessed/cursed/uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed

                Case DivineState.Normal

                Case DivineState.Cursed

            End Select
        End With

        Return CorpseEffect
    End Function
End Class
