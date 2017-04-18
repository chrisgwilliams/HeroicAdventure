
Imports HA.Common

Public Class Ration
	Inherits FoodBase
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Olive
		Walkover = "ration"
		Weight = 0.1
		Name = "ration"
		Nutrition = 100
		LifeSpan = -1

		Cooked = True
	End Sub

	Public Overrides Function eat(WhoIsEating As Avatar) As String
		'TODO: eat a ration
		' check for blessed/cursed/uncursed status

		Return ""

	End Function
End Class
