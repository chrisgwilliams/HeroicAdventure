Public Class PlateHelm
    Inherits Helmet

    Public Sub New()
        MyBase.New()

        Name = "plate helmet"
        Walkover = Name
        ACBonus = 2
        Weight = 8
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
