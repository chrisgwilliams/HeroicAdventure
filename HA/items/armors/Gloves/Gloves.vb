Imports HA.Common

Public MustInherit Class Gloves
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Type = ItemType.Gloves
		Weight = 0.2
	End Sub

	Public MustOverride Overrides Sub activate(whoIsActivating As Avatar)
	Public MustOverride Overrides Sub deactivate(whoIsDeactivating As Avatar)
End Class