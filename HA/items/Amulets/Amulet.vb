Imports HA.Common

#Region " -- Amulet Class and Inherited Subclasses "

Public MustInherit Class Amulet
	Inherits ItemBase
	Implements iEquippableItem

	Public Sub New()
		Type = Enumerations.ItemType.Neck
		Symbol = "'"
		IsBreakable = False
		Missle = False
		Tool = False
		Weight = 0.5
		Quantity = 1
	End Sub

	Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
	Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate

End Class
Public Class GoldAmulet
	Inherits Amulet
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Yellow
		Walkover = "gold amulet"
		Name = "gold amulet"
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)

	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)

	End Sub
End Class

#End Region
