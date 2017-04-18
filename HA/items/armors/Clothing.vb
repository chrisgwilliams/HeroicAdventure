Public Class Clothing
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "clothing"
		ACBonus = 0
		Walkover = Name
		Weight = 5
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
