Public Class Padded
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "padded armor"
		ACBonus = 1
		Walkover = Name
		Weight = 10
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
