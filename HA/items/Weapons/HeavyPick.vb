Imports HA.Common
Public Class HeavyPick
    Inherits Weapon

    Public Sub New()
        Name = "heavy pick"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 8
        Price = 8
        Critical = 20
        CritMultiplier = 4
        Weight = 6
        Mode = AttackType.Piercing
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



