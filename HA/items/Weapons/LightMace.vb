Imports HA.Common
Public Class LightMace
    Inherits Weapon

    Public Sub New()
        Name = "light mace"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 5
        Critical = 20
        CritMultiplier = 2
        Weight = 4
        Mode = AttackType.Bludgeoning
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



