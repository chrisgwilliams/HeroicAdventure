Imports HA.Common

#Region " -- Gold Class "

Public Class Gold
	Inherits ItemBase

	Public Sub New()
		Type = ItemType.Gold
		Symbol = "$"
		Color = ColorList.Yellow
		IsBreakable = False
		Missle = True
		Tool = False
		Walkover = "pile of gold"
		Name = "gold coins"
		Weight = 0.001
	End Sub
End Class

#End Region

#Region " -- Gem Class "

Public Class Gem
    Inherits ItemBase

    Public Sub New()
        Type = ItemType.Gem
        Symbol = "*"
        IsBreakable = True
        Missle = True
        Tool = True
        Weight = 0.1

        Dim rnd As New Random
        Color = rnd.Next(1, 101)
        Select Case Color
            Case 1 To 50
                Color = ColorList.Purple
                MarketValue = 50
                Walkover = "small purple gem"
            Case 51 To 75
                Color = ColorList.Red
                MarketValue = 100
                Walkover = "small red gem"
            Case 76 To 90
                Color = ColorList.Green
                MarketValue = 150
                Walkover = "small green gem"
            Case 91 To 98
                Color = ColorList.Blue
                MarketValue = 250
                Walkover = "small blue gem"
            Case 99 To 100
                Color = ColorList.White
                MarketValue = 500
                Walkover = "small white gem"
        End Select
        Name = Walkover
    End Sub
End Class

#End Region
