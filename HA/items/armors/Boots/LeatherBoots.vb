
Imports HA.Common

Public Class LeatherBoots
	Inherits Boots
	
	Public Sub New()
		MyBase.New()

		Name = "leather boots"
		Walkover = Name
		Color = ColorList.Olive
		Weight = 2
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		' TODO: equip leather boots
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		' TODO: unequip leather boots
	End Sub
End Class
