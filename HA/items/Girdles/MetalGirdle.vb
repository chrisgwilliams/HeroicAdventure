Imports HA.Common

Public Class MetalGirdle
	Inherits Girdle
	
	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.LightGray
		Name = "metal girdle"
        Walkover = Name
        ACBonus = 2
    End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
        whoIsActivating.MiscACMod += ACBonus
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        whoIsDeactivating.MiscACMod -= ACBonus
    End Sub
End Class
