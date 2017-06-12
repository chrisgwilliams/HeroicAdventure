Imports HA.Common
Public Class Falchion
    Inherits Weapon

    Public Sub New()
        Name = "falchion"
        Walkover = Name
        MinDamage = 2
        MaxDamage = 8
        Price = 75
        Critical = 18
        CritMultiplier = 2
        Weight = 8
        Mode = AttackType.Slashing
        Damage = "2d4"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



