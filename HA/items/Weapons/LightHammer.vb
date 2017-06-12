Imports HA.Common
Public Class LightHammer
    Inherits Weapon

    Public Sub New()
        Name = "light hammer"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 4
        Price = 1
        Critical = 20
        CritMultiplier = 2
        Weight = 2
        Mode = AttackType.Bludgeoning
        Damage = "1d4"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



