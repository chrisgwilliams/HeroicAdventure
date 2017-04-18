Public Class HalfPlate
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "half plate"
		ACBonus = 7
		Walkover = Name
		Weight = 50
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
