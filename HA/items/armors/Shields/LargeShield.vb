
Public Class LargeShield
    Inherits Shield

    Public Sub New()
        MyBase.New()

        ACBonus = 2
        Name = "large shield"
        Walkover = Name
        Weight = 10
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
