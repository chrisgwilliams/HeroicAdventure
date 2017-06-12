Imports HA.Common
Public Class Greatsword
    Inherits Weapon

    Public Sub New()
        Name = "greatsword"
        Walkover = Name
        MinDamage = 2
        MaxDamage = 12
        Price = 50
        Critical = 19
        CritMultiplier = 2
        Weight = 8
        Mode = AttackType.SlashPierce
        Damage = "2d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



