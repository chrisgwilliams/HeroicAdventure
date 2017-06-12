Imports HA.Common
Public Class LongSword
    Inherits Weapon

    Public Sub New()
        Name = "longsword"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 8
        Price = 15
        Critical = 19
        CritMultiplier = 2
        Weight = 4
        Mode = AttackType.SlashPierce
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



