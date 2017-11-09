Imports HA.Common
Imports HA

Public Class TrollCorpse
	Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.DarkGray
        Walkover = "troll corpse"
        Weight = 1.0
        Name = "troll corpse"
        Nutrition = 200
        LifeSpan = 50

        Race = "troll"
        Description = "It's a dead troll. "
        CorpseEffect = "It's really tough, like chewing on greasy rocks. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        ' TODO: eat a Troll Corpse

        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Not Cooked Then
            whoIsEating.Sick = True
            whoIsEating.SickDuration = 50 - (ItemState * D12() + whoIsEating.ConMod)
        End If

        ' Check ItemState for blessed / cursed / uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed
                    ' TODO: 1in6 chance of small regenerative boost for eating Blessed Troll Corpse

                    Nutrition = Nutrition * (1 + (D4() * 0.1))

                Case DivineState.Normal
                    ' TODO: 1in20 chance of small regenerative boost for eating regular Troll Corpse

                Case DivineState.Cursed
                    ' TODO: small regenerative penalty for eating Cursed Troll Corpse

                    Nutrition = Nutrition * (1 - (D4() * 0.1))

            End Select
        End With

        ' TODO: pull Hero healing rate into property so it can be modified by external factors (like eating a troll)

        Return CorpseEffect
    End Function

End Class
