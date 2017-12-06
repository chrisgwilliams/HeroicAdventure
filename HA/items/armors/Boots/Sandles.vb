
Public Class Sandles
	Inherits Boots
	
	Public Sub New()
		MyBase.New()

		Weight = 0.2
		Name = "sandles"
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
