Public Class Arrow
    Inherits Missile

    Public Sub New()
        MyBase.New()

        Name = "arrow"
        Walkover = Name
        Symbol = "/"
        MinDamage = 1
        MaxDamage = 8
        Price = 0.05
        Critical = 20
        CritMultiplier = 2
        Damage = "1d8"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



