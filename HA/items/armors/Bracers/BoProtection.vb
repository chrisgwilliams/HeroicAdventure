Public Class BoProtection
	Inherits Bracers


	Public Sub New()
		MyBase.New()

		Name = "bracers of protection"
		Walkover = "brass bracers"
		ACBonus = 2
		StatBonus = 0

		Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
