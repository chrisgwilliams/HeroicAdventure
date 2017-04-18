Imports HA.Common

Public Class RoIntellect
	Inherits Ring

	Public Sub New()
		MyBase.New()

		Color = ColorList.Yellow
		Walkover = "bronze ring"
		Name = "ring of intellect"

		StatBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.IntMods += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.IntMods -= StatBonus
	End Sub
End Class
