Imports HA.Common

Public Class AoStrength
	Inherits Amulet
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Cyan
		Walkover = "sapphire amulet"
		Name = "amulet of strength"

		StatBonus = 1
		'TODO: add blessed / cursed / uncursed support
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.StrMods += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.StrMods -= StatBonus
	End Sub
End Class
