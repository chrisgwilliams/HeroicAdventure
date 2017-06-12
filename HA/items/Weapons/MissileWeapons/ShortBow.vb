Imports HA.Common
Public Class ShortBow
    Inherits MissleWeapon

    Public Sub New()
        MyBase.New()

        Name = "shortbow"
        Walkover = Name
        Price = 10
        Range = 4
        Weight = 6
        RequiredMissle = MissleType.arrow
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



