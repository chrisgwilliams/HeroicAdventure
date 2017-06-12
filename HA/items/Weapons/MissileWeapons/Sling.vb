Public Class Sling
    Inherits MissleWeapon

    Public Sub New()
        MyBase.New()

        Name = "sling"
        Walkover = Name
        Price = 1
        Range = 3
        Weight = 0.5
        Critical = 20
        CritMultiplier = 2
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



