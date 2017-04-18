
Public Class FurBoots
	Inherits Boots
	
	Public Sub New()
		MyBase.New()
		ACBonus = 2
		Name = "fur lined boots"
		Walkover = Name
		Weight = 2

	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
		whoIsActivating.ColdResist += 5
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
		whoIsDeactivating.ColdResist -= 5
	End Sub
End Class
