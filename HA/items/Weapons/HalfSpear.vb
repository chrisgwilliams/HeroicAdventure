Imports HA.Common
Public Class HalfSpear
    Inherits Weapon

    Public Sub New()
        Name = "half-spear"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 1
        Critical = 20
        CritMultiplier = 3
        Weight = 3
        Mode = AttackType.Piercing
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



