Imports HA.Common
Public Class GreatAxe
    Inherits Weapon

    Public Sub New()
        Name = "greataxe"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 12
        Price = 20
        Critical = 20
        CritMultiplier = 3
        Weight = 12
        Mode = AttackType.Slashing
        Damage = "1d12"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



