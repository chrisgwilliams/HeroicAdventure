Imports HA.Common

Public Class Missile
    Inherits Weapon

    Public Sub New()
        MyBase.New()

        Type = ItemType.Missiles
        Missle = True
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class


