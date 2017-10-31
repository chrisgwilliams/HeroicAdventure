Imports HA.Common

Public Class LargeRation
    Inherits FoodBase

    Public Sub New()
        MyBase.New()

        Color = ColorList.Olive
        Walkover = "large ration"
        Weight = 1.0
        Name = "large ration"
        Nutrition = 200
        LifeSpan = 10000

        Cooked = True
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        'TODO: eat a ration

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        ' check ItemState for blessed/cursed/uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed

                Case DivineState.Normal

                Case DivineState.Cursed

            End Select
        End With

        Return ""

    End Function
End Class

