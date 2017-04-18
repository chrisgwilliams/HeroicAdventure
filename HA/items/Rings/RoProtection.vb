Imports HA.Common

Public Class RoProtection
	Inherits Ring
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.LightGray
		Walkover = "silver ring"
		Name = "ring of protection"

		StatBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += StatBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= StatBonus
	End Sub
End Class
