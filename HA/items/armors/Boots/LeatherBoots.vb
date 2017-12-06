
Imports HA.Common

Public Class LeatherBoots
	Inherits Boots
	
	Public Sub New()
		MyBase.New()

		Name = "leather boots"
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
