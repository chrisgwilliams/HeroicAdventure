Imports HA
Imports HA.Common

Public Class LizardCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Green
        Walkover = "lizard corpse"
        Weight = 0.8
        Name = "lizard corpse"
        Nutrition = 70
        LifeSpan = 50

        Race = "lizard"
        Description = "A dead lizard, about the size of a small scaly cat. "
        CorpseEffect = "It tastes like raw chicken. Maybe try cooking the next one. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Not bad. It tastes like chicken. "
            LifeSpan += 20
        End If

        ' check ItemState for blessed/cursed/uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed
                    Nutrition = Nutrition * (1 + (D4() * 0.1))

                Case DivineState.Normal

                Case DivineState.Cursed
                    Nutrition = Nutrition * (1 - (D4() * 0.1))
            End Select
        End With

        Return CorpseEffect
    End Function
End Class
