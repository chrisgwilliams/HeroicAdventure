Imports HA.Common

Public MustInherit Class Cloak
	Inherits Armor

	Public Sub New()
		MyBase.New()
		Type = Enumerations.ItemType.Cloak
		IsBreakable = False
		Missle = False
		Weight = 2
	End Sub

	Public MustOverride Overrides Sub activate(whoIsActivating As Avatar)
	Public MustOverride Overrides Sub deactivate(whoIsDeactivating As Avatar)
End Class