Imports HA.Common
Public Class ShortSpear
    Inherits Weapon

    Public Sub New()
        Name = "short-spear"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 8
        Price = 2
        Critical = 20
        CritMultiplier = 3
        Weight = 5
        Mode = AttackType.Piercing
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class


