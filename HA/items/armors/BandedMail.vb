Public Class BandedMail
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "banded mail"
		ACBonus = 6
		Walkover = Name
		Weight = 30
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
