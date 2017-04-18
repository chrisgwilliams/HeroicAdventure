Public Class LeatherGauntlets
	Inherits Gloves

	Public Sub New()
		MyBase.New()

		Name = "leather gauntlets"
		Walkover = Name
		ACBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
