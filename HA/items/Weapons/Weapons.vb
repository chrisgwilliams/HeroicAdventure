Imports HA
Imports HA.Common

#Region " Weapon Base Class "

Public MustInherit Class Weapon
    Inherits ItemBase
    Implements iEquippableItem

    Public Property MinDamage() As Integer
    Public Property MaxDamage() As Integer
    Public Property Price() As Integer
    Public Property Critical() As Integer
    Public Property CritMultiplier() As Integer
    Public Property Mode() As Integer

    Public Sub New()
        Type = ItemType.Weapon
        Color = ColorList.LightGray
        Symbol = "("
        IsBreakable = True
        Tool = False
        Missle = False ' may be overridden of course
        Quantity = 1
    End Sub

    Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
    Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate
End Class

#End Region

