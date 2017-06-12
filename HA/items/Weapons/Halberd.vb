Imports HA.Common
Public Class Halberd
    Inherits Weapon

    Public Sub New()
        Name = "halberd"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 10
        Price = 10
        Critical = 20
        CritMultiplier = 3
        Weight = 12
        Mode = AttackType.Slashing
        Damage = "1d10"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



