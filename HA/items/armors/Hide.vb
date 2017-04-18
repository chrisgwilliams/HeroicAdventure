Public Class Hide
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "hide armor"
		ACBonus = 3
		Walkover = Name
		Weight = 15
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
