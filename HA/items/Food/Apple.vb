
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
		LifeSpan = 200

	End Sub

	Public Overrides Function eat(WhoIsEating As Avatar) As String
		'TODO eat an apple

		' check for blessed/cursed/uncursed status
		Return ""

	End Function
End Class
