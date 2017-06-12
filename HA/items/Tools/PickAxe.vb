Imports HA.Common

#Region " -- Tool Inherited Subclass "

Public Class PickAxe
    Inherits Tool

    Public Sub New()
        MyBase.New()

        Quantity = 1
        Color = ColorList.DarkGray
        Symbol = ")"
        Walkover = "pickaxe"
        Weight = 2
        Name = "pickaxe"
    End Sub

    Public Overrides Sub activate(whoIsActivating As Avatar)
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
    End Sub
End Class

#End Region