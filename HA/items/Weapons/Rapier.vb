Imports HA.Common
Public Class Rapier
    Inherits Weapon

    Public Sub New()
        Name = "rapier"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 20
        Critical = 18
        CritMultiplier = 2
        Weight = 2
        Mode = AttackType.SlashPierce
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



