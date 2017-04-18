Imports HA.Common

Public Class RoInvisibility
	Inherits Ring
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Red
		Walkover = "red ring"
		Name = "ring of invisibility"

		StatBonus = 1
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.Invisible = True
		' TODO: enhance stealth when wearing ring of invisibility
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.Invisible = False
		' TODO: restore original stealth when removing ring of invisibility
	End Sub
End Class
