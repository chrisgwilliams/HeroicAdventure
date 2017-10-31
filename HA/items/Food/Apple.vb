Imports HA.Common

Public Class Apple
	Inherits FoodBase

    Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Green
		Walkover = "apple"
		Weight = 0.1
		Name = "apple"
		Nutrition = 20
        LifeSpan = 100

    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        'TODO eat an apple

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

        Return ""

    End Function
End Class
