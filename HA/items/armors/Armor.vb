Imports HA.Common
Imports HA.Common.Helper

Public MustInherit Class Armor
	Inherits ItemBase
	Implements iEquippableItem

	Public Sub New()
		Type = ItemType.Armor
		Color = ColorList.LightGray
		Symbol = "["
		IsBreakable = False
		Tool = False
		Missle = False
		Quantity = 1
	End Sub

	Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
	Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate
End Class

