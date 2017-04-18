Imports HA.Common

Public Class WoMagicMap
	Inherits Wand

	Public Sub New()
		MyBase.New()

		Name = "wand of magic map"
		Walkover = "twisted wand"
		HasCharges = True
		Color = ColorList.Cyan
		Bounce = False
		Range = 0
		RayColor = ColorList.White

		Charges = D6() + 4
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
	End Sub

	Public Overrides Property Bounce As Boolean
	Public Overrides Property Range As Integer
	Public Overrides Property RayColor As ColorList
End Class
