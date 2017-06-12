Imports HA.Common

Public Class Rock
    Inherits Missile

    Public Sub New()
        MyBase.New()

        Name = "rock"
        Walkover = Name
        Symbol = "*"
        Color = ColorList.DarkGray
        MinDamage = 1
        MaxDamage = 4
        Price = 0
        Critical = 20
        CritMultiplier = 2
        Damage = "1d4"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class


