Imports HA.Common

Public MustInherit Class Wand
	Inherits ItemBase
	Implements iEquippableItem

	Public Sub New()
		Type = ItemType.Wand
		Symbol = "/"
		IsBreakable = True
		Missle = False
		Weight = 0.5
		Quantity = 1
	End Sub

	Public MustOverride Property Bounce As Boolean
	Public MustOverride Property Range As Integer
	Public MustOverride Property RayColor As ColorList

	Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
	Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate

End Class
