Public Class ChainGauntlets
	Inherits Gloves

	Public Sub New()
		MyBase.New()

		Name = "chain gauntlets"
		Walkover = Name
		ACBonus = 2
		Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
