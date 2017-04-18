Imports HA.Common

#Region " -- Tool Class and Inherited Subclasses "

Public Class Tool
	Inherits ItemBase

	Public Sub New()
		Type = ItemType.Tool

		IsBreakable = True
		Tool = True
		Missle = False

	End Sub
End Class

Public Class PickAxe
	Inherits Tool

	Public Sub New()
		MyBase.New()

		Quantity = 1
		Color = ColorList.DarkGray
		Symbol = ")"
		Walkover = "pickaxe"
		Weight = 2
		Name = "pickaxe"
	End Sub
End Class

#End Region
