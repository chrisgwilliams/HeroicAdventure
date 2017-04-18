Public Class BoStrength
	Inherits Bracers

	Public Sub New()
		MyBase.New()

		Name = "bracers of strength"
		Walkover = "leather bracers"
		ACBonus = 1
		StatBonus = 2

		Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.StrMods += StatBonus
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.StrMods -= StatBonus
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class

