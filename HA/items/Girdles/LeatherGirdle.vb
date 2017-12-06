Imports HA.Common

Public Class LeatherGirdle
	Inherits Girdle

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Olive
		Name = "leather girdle"
        Walkover = Name
        ACBonus = 1
    End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
        whoIsActivating.MiscACMod += ACBonus
    End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        whoIsDeactivating.MiscACMod -= ACBonus
    End Sub
End Class
