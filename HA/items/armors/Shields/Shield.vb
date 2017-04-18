Imports HA.Common

Public MustInherit Class Shield
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Type = ItemType.Shield
	End Sub

	Public MustOverride Overrides Sub activate(whoIsActivating As Avatar)
	Public MustOverride Overrides Sub deactivate(whoIsDeactivating As Avatar)

End Class