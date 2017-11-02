Imports HA
Imports HA.Common

Public Class DogCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Olive
        Walkover = "dog corpse"
        Weight = 0.8
        Name = "dog corpse"
        Nutrition = 70
        LifeSpan = 50

        Race = "dog"
        Description = "This once muscular dog looks to have been hungry for some time. Probably somebody's pet, now somebody's dinner. "
        CorpseEffect = "It tastes rather boring. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Needs something. Maybe salt. "
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
