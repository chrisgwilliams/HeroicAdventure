Imports HA.Common

#Region " -- Tool Class and Inherited Subclasses "

Public MustInherit Class Tool
    Inherits ItemBase
    Implements iEquippableItem

    Public Sub New()
        Type = ItemType.Tool

        IsBreakable = True
        Tool = True
        Missle = False

    End Sub

    Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
    Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate
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

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class

#End Region
