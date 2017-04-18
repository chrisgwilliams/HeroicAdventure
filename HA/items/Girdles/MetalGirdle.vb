Imports HA.Common

Public Class MetalGirdle
	Inherits Girdle
	
	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.LightGray
		Name = "metal girdle"
		Walkover = Name
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		'TODO: Equip Metal Girdle

	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		'TODO: UnEquip Metal Girdle

	End Sub
End Class
