Public Class ScaleHelm
    Inherits Helmet

    Public Sub New()
        MyBase.New()

        Name = "scale helmet"
        Walkover = Name
        Weight = 7
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
