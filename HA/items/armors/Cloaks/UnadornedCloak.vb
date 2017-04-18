
Imports HA.Common

Public Class UnadornedCloak
	Inherits Cloak
	
	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Olive
		Name = "unadorned cloak"
		Walkover = Name
		ACBonus = 0
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
