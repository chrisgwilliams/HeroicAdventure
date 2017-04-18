Imports HA.Common

Public Class RoStrength
	Inherits Ring
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Blue
		Walkover = "blue ring"
		Name = "ring of strength"

		StatBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.StrMods += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.StrMods -= StatBonus
	End Sub
End Class

