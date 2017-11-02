Imports HA
Imports HA.Common

Public Class SnakeCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Green
        Walkover = "snake corpse"
        Weight = 0.2
        Name = "snake corpse"
        Nutrition = 30
        LifeSpan = 50

        Race = "snake"
        Description = "It's a dead snake. It looks like a limp, green stick. "
        CorpseEffect = "It's not very appetizing, but you eat it anyway. Tingles a bit. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Tastes a little bit like chicken. "
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
