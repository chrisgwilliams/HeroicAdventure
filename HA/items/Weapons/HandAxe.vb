Imports HA.Common
Public Class HandAxe
    Inherits Weapon

    Public Sub New()
        Name = "handaxe"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 6
        Critical = 20
        CritMultiplier = 3
        Weight = 3
        Mode = AttackType.Slashing
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



