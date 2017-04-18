Imports HA.Common

Public MustInherit Class Girdle
	Inherits ItemBase
	Implements iEquippableItem

	Public Sub New()
		Type = ItemType.Girdle
		Symbol = "["
		IsBreakable = False
		Missle = False
		Weight = 2
		Quantity = 1
	End Sub

	Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
	Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate

End Class

