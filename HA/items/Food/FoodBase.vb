Imports HA.Common

Public MustInherit Class FoodBase
	Inherits ItemBase
	Implements iEdibleItem

	Public Property Nutrition As Int16
    Public Property LifeSpan As Int16
    Public Property DecayLevel As Int16
    Public Property Cooked As CookState
    Public Property Rotten As Boolean
    Public Property DecayMessage As String
    Public Property CookMessage As String

    Public Sub New()
		Type = ItemType.Food

		IsBreakable = False
		Tool = False
		Missle = True

		Symbol = "%"
		Quantity = 1

        Cooked = False

        DecayLevel = 0
        DecayMessage = "Something in your pack has rotted away. "
    End Sub

    Public MustOverride Function eat(whoIsEating As Avatar) As String Implements iEdibleItem.eat

    Public Function bless(Optional ByVal silent As Boolean = True) As String Implements iEdibleItem.bless
        Dim blessMod As Integer = D4()

        LifeSpan = LifeSpan * (1 + (blessMod * 0.1))
        DecayLevel = DecayLevel * (1 - (blessMod * 0.1))

        If silent Then
            Return ""
        Else
            Return "The " + Me.Name + " glows white for a second. You feel better about this meal. "
        End If
    End Function

    Public Function curse(Optional ByVal silent As Boolean = True) As String Implements iEdibleItem.curse
        Dim curseMod As Integer = D4()

        LifeSpan = LifeSpan * (1 - (curseMod * 0.1))
        DecayLevel = DecayLevel * (1 + (curseMod * 0.1))

        If silent Then
            Return ""
        Else
            Return "The " + Me.Name + " glows black for a second. You feel worse about this meal. "
        End If
    End Function

    Public Function decay(whoIsCarrying As Avatar) As String Implements iEdibleItem.decay
        'TODO: Add modifier for Food Preservation or Survival Skill
        DecayLevel += 1

        If DecayLevel >= LifeSpan * 0.9 Then
            Rotten = True
        End If

        If DecayLevel > LifeSpan Then
            Return ""
        Else
            'TODO: remove from inventory

            Return DecayMessage
        End If
    End Function

    Public Function cook(Optional ByVal silent As Boolean = True) As String Implements iEdibleItem.cook
        Select Case Cooked
            Case CookState.Raw
                Cooked = CookState.Cooked
                If CookMessage = "" Then CookMessage = "You cook the " + Me.Name + ". "
            Case CookState.Cooked
                Cooked = CookState.Burnt
                If CookMessage = "" Then CookMessage = "You burn the " + Me.Name + ". "
            Case CookState.Burnt
                Cooked = CookState.Inedible
                If CookMessage = "" Then CookMessage = "You cook the burnt " + Me.Name + " further. It is now completely inedible."
            Case CookState.Inedible
                If CookMessage = "" Then CookMessage = "The " + Me.Name + " is reduced to ash. "
                'TODO: Remove items "reduced to ash" from inventory
        End Select

        If silent Then
            Return ""
        Else
            Return CookMessage
        End If
    End Function
End Class
