Public Class SmallShield
    Inherits Shield

    Public Sub New()
        MyBase.New()

        ACBonus = 1
        Name = "small shield"
        Walkover = Name
        Weight = 6
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
