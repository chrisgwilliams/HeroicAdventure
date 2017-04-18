Public Class BreastPlate
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "breastplate"
		ACBonus = 5
		Walkover = Name
		Weight = 20
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
