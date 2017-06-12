Imports HA.Common
Public Class LongBow
    Inherits MissleWeapon

    Public Sub New()
        MyBase.New()

        Name = "longbow"
        Walkover = Name
        Price = 20
        Range = 6
        Weight = 8
        RequiredMissle = MissleType.arrow
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class



