Imports HA
Imports HA.Common

Public Class WolfCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Olive
        Walkover = "wolf corpse"
        Weight = 0.7
        Name = "wolf corpse"
        Nutrition = 50
        LifeSpan = 50

        Race = "wolf"
        Description = "It's a dead wolf. "
        CorpseEffect = "Tastes a bit gamey. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Tough and stringy, but edible. "
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
