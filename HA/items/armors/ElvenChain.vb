Public Class ElvenChain
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "elven chain"
		ACBonus = 6
		Walkover = Name
		Weight = 15
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
		whoIsActivating.MagicResist += 10
		whoIsActivating.SleepResist += 10
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
		whoIsDeactivating.MagicResist -= 10
		whoIsDeactivating.SleepResist -= 10
	End Sub
End Class
