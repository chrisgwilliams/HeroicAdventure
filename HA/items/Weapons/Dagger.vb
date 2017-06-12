Imports HA.Common

Public Class Dagger
    Inherits Weapon

    Public Sub New()
        Name = "dagger"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 4
        Price = 2
        Critical = 19
        CritMultiplier = 2
        Weight = 1
        Mode = AttackType.Piercing
        Damage = "1d4"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



