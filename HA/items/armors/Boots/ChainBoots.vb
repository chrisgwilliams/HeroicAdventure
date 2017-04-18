
Public Class ChainBoots
	Inherits Boots

	Public Sub New()
		MyBase.New()

		Name = "chain boots"
		Walkover = Name
		ACBonus = 2
		Weight = 4
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class

