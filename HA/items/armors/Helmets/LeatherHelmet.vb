Imports HA.Common

Public Class LeatherHelmet
	Inherits Helmet

	Public Sub New()
		MyBase.New()

		Name = "leather helmet"
		Walkover = Name
		Color = ColorList.Olive
		Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
