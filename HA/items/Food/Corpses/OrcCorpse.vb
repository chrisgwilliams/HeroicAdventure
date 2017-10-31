Imports HA.Common

Public Class OrcCorpse
	Inherits Corpse

    Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Green
		Walkover = "orc corpse"
        Weight = 0.8
        Name = "orc corpse"
		Nutrition = 100
        LifeSpan = 50

        Race = "orc"
        Description = "It's a dead orc. "
		CorpseEffect = "Not bad. Could use a little salt. "

        Rotten = False
		Cooked = False
	End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        'TODO: eat an orc corpse

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Not Cooked Then
        End If

        ' Check ItemState for blessed / cursed / uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed

                Case DivineState.Normal

                Case DivineState.Cursed

            End Select
        End With

        Return CorpseEffect
    End Function


End Class
