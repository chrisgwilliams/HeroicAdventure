
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

	Public Overrides Function eat(WhoIsEating As Avatar) As String
		'ToDo: eat Bread
		' check for blessed/cursed/uncursed status

		Return ""

	End Function
End Class
