Imports HA.Common

Public Class LeatherGirdle
	Inherits Girdle

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Olive
		Name = "leather girdle"
		Walkover = Name
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		' TODO: Equip Leather Girdle

	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		' TODO: Unequip Leather Girdle

	End Sub
End Class
