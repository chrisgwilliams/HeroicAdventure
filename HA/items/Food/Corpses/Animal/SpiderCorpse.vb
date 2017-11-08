Imports HA
Imports HA.Common

' Cooking spiders increases their nutritional value, and neutralizes 
' their poison, which also cancels the Poison Resistance Perk. If
' the spider is cooked, but cursed, it will poison without the perk.

Public Class SpiderCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.Purple
        Walkover = "spider corpse"
        Weight = 0.2
        Name = "spider corpse"
        Nutrition = D20() + 40
        LifeSpan = 50

        Race = "spider"
        Description = "It's a dead spider. Still scary though. "
        CorpseEffect = "Yuck. Poison. Your throat burns when you eat this. "

        Rotten = False
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        If Rotten Then
            Nutrition = Nutrition * 0.1
        End If

        If Cooked Then
            CorpseEffect = "Crispy spider. Not bad in a pinch. "
            Nutrition += 30
            LifeSpan += 20
        End If

        ' check ItemState for blessed/cursed/uncursed status
        With whoIsEating
            Select Case ItemState
                Case DivineState.Blessed
                    Nutrition = Nutrition * (1 + (D4() * 0.1))

                    If Not Cooked Then
                        .PoisonResist += D8()
                    End If

                Case DivineState.Normal
                    If Not Cooked Then
                        .Poisoned = True
                        .PoisonDuration += (D10() + 5)
                        .PoisonResist += D6()
                    End If

                Case DivineState.Cursed
                    Nutrition = Nutrition * (1 - (D4() * 0.1))

                    If Not Cooked Then
                        .Poisoned = True
                        .PoisonDuration += (D20() + 20)
                        .PoisonResist += D6() + D6()
                    Else
                        .Poisoned = True
                        .PoisonDuration += D10()
                        .PoisonResist += D4()
                    End If
            End Select

            'TODO: add hunger level to Avatar
        End With

        Return CorpseEffect
    End Function
End Class
