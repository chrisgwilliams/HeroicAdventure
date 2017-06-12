Imports HA.Common
Public Class GreatClub
    Inherits Weapon

    Public Sub New()
        Name = "greatclub"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 10
        Price = 5
        Critical = 20
        CritMultiplier = 2
        Weight = 8
        Mode = AttackType.Bludgeoning
        Damage = "1d10"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



