Public Class SplintMail
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "splint mail"
		ACBonus = 6
		Walkover = Name
		Weight = 25
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
