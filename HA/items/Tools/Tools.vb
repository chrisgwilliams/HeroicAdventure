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

#End Region
