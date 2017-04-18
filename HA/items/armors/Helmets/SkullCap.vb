Imports HA.Common

Public Class SkullCap
	Inherits Helmet

	Public Sub New()
		MyBase.New()

		Name = "skullcap"
		Walkover = Name
		Color = Enumerations.ColorList.White
		Weight = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
