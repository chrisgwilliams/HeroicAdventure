Imports HA.Common

Public Class WoLight
	Inherits Wand

	Public Sub New()
		MyBase.New()

		Name = "wand of light"
		Walkover = "crooked wand"
		HasCharges = True
		Color = Enumerations.ColorList.Yellow
		Bounce = False
		Range = 3
		RayColor = Enumerations.ColorList.White

		Charges = D8() + 6
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		' Does Nothing
	End Sub
	
	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		' Does Nothing
	End Sub

	Public Overrides Property Bounce As Boolean
	Public Overrides Property Range As Integer
	Public Overrides Property RayColor As ColorList
End Class
