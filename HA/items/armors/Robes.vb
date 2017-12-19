Imports HA.Common.Helper

Public Class Robes
    Inherits Armor

    Public Sub New()
        MyBase.New()

        Dim rnd As New System.Random
        Dim intColor As Integer = rnd.Next(1, 16)
        Color = intColor

        Name = GetColor(Color).ToLower & " robe"
        ACBonus = 0
        Walkover = Name
        Weight = 5
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
		whoIsActivating.MiscACMod += ACBonus
	End Sub

	Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
		whoIsDeactivating.MiscACMod -= ACBonus
	End Sub
End Class
