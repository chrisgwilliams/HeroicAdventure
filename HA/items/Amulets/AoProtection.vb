Imports HA.Common

Public Class AoProtection
	Inherits Amulet

	Public Sub New()
		MyBase.New()

		Color = ColorList.Green
		Walkover = "peridot amulet"
		Name = "amulet of protection +" & ACBonus
		ACBonus = 2
		StatBonus = 0
		'TODO: add blessed / cursed / uncursed support
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
