Imports HA.Common

#Region " -- Wand Class and Inherited Subclasses "

Public MustInherit Class Wand
	Inherits ItemBase

	Public Sub New()
		Type = ItemType.Wand
		Symbol = "/"
		IsBreakable = True
		Missle = False
		Weight = 0.5
		Quantity = 1
	End Sub

	Public Property Bounce As Boolean
	Public Property Range As Integer
	Public Property RayColor As ColorList
End Class



#End Region
