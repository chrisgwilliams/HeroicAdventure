Imports HA.Common
Public Class Scimitar
    Inherits Weapon

    Public Sub New()
        Name = "scimitar"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 15
        Critical = 18
        CritMultiplier = 2
        Weight = 4
        Mode = AttackType.SlashPierce
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



