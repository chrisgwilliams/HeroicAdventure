Imports HA.Common

Public Class BattleAxe
    Inherits Weapon

    Public Sub New()
        Name = "battleaxe"
        Walkover = Name
        MinDamage = 1
        MaxDamage = 6
        Price = 10
        Critical = 19
        CritMultiplier = 2
        Weight = 6
        Mode = AttackType.Slashing
        Damage = "1d6"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class


