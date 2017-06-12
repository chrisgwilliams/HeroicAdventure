Imports HA.Common
Public Class Scythe
    Inherits Weapon

    Public Sub New()
        Name = "scythe"
        Walkover = Name
        MinDamage = 2
        MaxDamage = 8
        Price = 18
        Critical = 20
        CritMultiplier = 4
        Weight = 10
        Mode = AttackType.Slashing
        Damage = "2d4"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



