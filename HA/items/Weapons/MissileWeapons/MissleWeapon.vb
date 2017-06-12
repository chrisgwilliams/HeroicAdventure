Imports HA.Common

Public MustInherit Class MissleWeapon
    Inherits Weapon

    Public Property Range() As Integer
    Public Property RequiredMissle() As Integer

    Public Sub New()
        MyBase.New()

        Color = ColorList.Olive
        Type = ItemType.MissleWeapon
        Symbol = "}"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



