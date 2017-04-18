Imports HA.Common

Public Class Leather
	Inherits Armor

	Public Sub New()
		MyBase.New()

		Name = "leather armor"
		ACBonus = 2
		Color = Enumerations.ColorList.Olive
		Walkover = Name
		Weight = 15
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
