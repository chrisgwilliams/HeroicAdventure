Imports HA.Common

Public MustInherit Class FoodBase
	Inherits ItemBase
	Implements iEdibleItem

	Public Property Nutrition As Int16
	Public Property LifeSpan As Int16
	Public Property Cooked As Boolean
	Public Property Rotten As Boolean

	Public Sub New()
		Type = ItemType.Food

		IsBreakable = False
		Tool = False
		Missle = True

		Symbol = "%"
		Quantity = 1

		Cooked = False
	End Sub

	Public MustOverride Function eat(WhoIsEating As Avatar) As String Implements iEdibleItem.eat

End Class
