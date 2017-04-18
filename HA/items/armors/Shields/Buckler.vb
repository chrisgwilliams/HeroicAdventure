
Public Class Buckler
    Inherits Shield

    Public Sub New()
        MyBase.New()

        ACBonus = 1
        Name = "buckler"
        Walkover = Name
        Weight = 4
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
