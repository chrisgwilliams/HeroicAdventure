Imports HA.Common
Public Class MorningStar
    Inherits Weapon

    Public Sub New()
        Name = "morningstar"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 8
        Price = 8
        Critical = 20
        CritMultiplier = 2
        Weight = 6
        Mode = AttackType.Bludgeoning
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



