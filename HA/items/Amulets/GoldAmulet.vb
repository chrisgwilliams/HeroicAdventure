Imports HA.Common

Public Class GoldAmulet
    Inherits Amulet

    Public Sub New()
        MyBase.New()

        Color = ColorList.Yellow

        Walkover = "gold amulet"

        Name = "gold amulet"

    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class
