Imports HA.Common

Public Class WoMagicMissile
	Inherits Wand

	Public Sub New()
		MyBase.New()

		Name = "wand of magic missiles"
		Walkover = "curved wand"
		HasCharges = True
		Color = ColorList.Navy
		Bounce = False
		Range = 3
		RayColor = ColorList.Red

		Charges = D8() + 6
		ItemState = RND.Next(3)
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)

	End Sub
	
	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)

	End Sub

	Public Overrides Property Bounce As Boolean
	Public Overrides Property Range As Integer
	Public Overrides Property RayColor As ColorList
End Class
