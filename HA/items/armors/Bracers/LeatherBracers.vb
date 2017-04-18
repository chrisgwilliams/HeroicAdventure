Public Class LeatherBracers
	Inherits Bracers

	Public Sub New()
		MyBase.New()

		Name = "leather bracers"
		Walkover = "leather bracers"
		ACBonus = 1
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
