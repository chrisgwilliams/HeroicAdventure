Public Class DwarvenPlate
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "dwarven plate"
		ACBonus = 8
		Walkover = Name
		Weight = 50
	End Sub

	'TODO: setup abilities for Dwarven Plate

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
		whoIsActivating.MagicResist += 10
		whoIsActivating.PoisonResist += 10
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
		whoIsDeactivating.MagicResist -= 10
		whoIsDeactivating.PoisonResist -= 10
	End Sub
End Class

