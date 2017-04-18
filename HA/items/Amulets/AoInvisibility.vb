Imports HA.Common

Public Class AoInvisibility
	Inherits Amulet

	Public Sub New()
		MyBase.New()

		Color = ColorList.Red
		Walkover = "ruby amulet"
		Name = "amulet of invisibility"

		StatBonus = 0
		'TODO: add blessed / cursed / uncursed support
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.Invisible = True
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.Invisible = False
	End Sub
End Class