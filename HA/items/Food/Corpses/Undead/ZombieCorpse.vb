Imports HA
Imports HA.Common

Public Class ZombieCorpse
    Inherits Corpse

    Public Sub New()
        MyBase.New()

        Color = Enumerations.ColorList.LightGray
        Walkover = "zombie corpse"
        Weight = 0.8
        Name = "zombie corpse"
        Nutrition = 10
        LifeSpan = 500

        Race = "zombie"
        Description = "It's a zombie corpse. To call it dead seems redundant, but it's not moving anymore. "
        CorpseEffect = "This is truly disgusting, rotten meat. The stench is overpowering. "

        Rotten = True
        Cooked = False
    End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        Throw New NotImplementedException()

        If Rotten Then
        End If

        If Not Cooked Then
        End If

        ' check ItemState for blessed/cursed/uncursed status
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
