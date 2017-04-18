Public Class FullPlate
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "full plate"
		ACBonus = 8
		Walkover = Name
		Weight = 80
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
