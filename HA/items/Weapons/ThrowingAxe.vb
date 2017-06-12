Imports HA.Common
Public Class ThrowingAxe
    Inherits Weapon

    Public Sub New()
        Name = "throwing axe"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 8
        Critical = 20
        CritMultiplier = 2
        Weight = 2
        Mode = AttackType.Piercing
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



