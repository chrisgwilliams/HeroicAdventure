Imports HA.Common

Public Class AoIntellect
	Inherits Amulet

	Public Sub New()
		MyBase.New()

		Color = ColorList.Red
		Walkover = "garnet amulet"
		Name = "amulet of intellect"

		StatBonus = 1
		'TODO: add blessed / cursed / uncursed support
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.IntMods += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.IntMods -= StatBonus
	End Sub
End Class
