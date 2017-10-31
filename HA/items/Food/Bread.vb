Imports HA.Common

Public Class Bread
	Inherits FoodBase
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Olive
		Walkover = "loaf of bread"
		Weight = 0.1
		Name = "loaf of bread"
		Nutrition = 50
		LifeSpan = 100

		Cooked = True
	End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        'ToDo: eat Bread

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
