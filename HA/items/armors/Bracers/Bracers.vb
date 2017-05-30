Imports HA.Common

Public MustInherit Class Bracers
	Inherits Armor

	Public Sub New()
		MyBase.New()

        Type = ItemType.Bracers
        ACBonus = 1
	End Sub

	Public MustOverride Overrides Sub activate(whoIsActivating As Avatar)
	Public MustOverride Overrides Sub deactivate(whoIsDeactivating As Avatar)

End Class