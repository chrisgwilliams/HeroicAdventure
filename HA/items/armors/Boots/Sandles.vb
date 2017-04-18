
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
		' TODO: equip sandles
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		' TODO: unequip sandles
	End Sub
End Class
