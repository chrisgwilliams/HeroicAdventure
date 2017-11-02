Imports HA
Imports HA.Common

Public Class RatCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Olive
        Walkover = "rat corpse"
        Weight = 0.2
        Name = "rat corpse"
        Nutrition = 10
        LifeSpan = 50

        Race = "rat"
        Description = "It's a dead rat. "
        CorpseEffect = "Ugh. Gross. You can barely choke this down. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Mmmm... crispy rat. "
            LifeSpan += 20
            Nutrition = 40
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
