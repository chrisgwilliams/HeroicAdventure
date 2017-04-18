Public Class PlateGauntlets
	Inherits Gloves

	Public Sub New()
		MyBase.New()

		Name = "plate gauntlets"
		Walkover = Name
		ACBonus = 3
		Weight = 5
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
