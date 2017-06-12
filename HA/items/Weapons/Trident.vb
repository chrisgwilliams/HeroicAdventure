Imports HA.Common
Public Class Trident
    Inherits Weapon

    Public Sub New()
        Name = "trident"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 8
        Price = 15
        Critical = 20
        CritMultiplier = 2
        Weight = 4
        Mode = AttackType.Piercing
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



