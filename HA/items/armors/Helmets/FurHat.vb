
Public Class FurHat
    Inherits Helmet

    Public Sub New()
        MyBase.New()

        Name = "fur hat"
        Walkover = Name
        ACBonus = 0
        Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
