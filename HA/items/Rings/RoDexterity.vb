Imports HA.Common

Public Class RoDexterity
	Inherits Ring

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.DarkGreen
		Walkover = "green ring"
		Name = "ring of dexterity"

		StatBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.DexMods += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.DexMods -= StatBonus
	End Sub
End Class