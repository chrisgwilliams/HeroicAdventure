Imports HA.Common

Public Class PoPoison
	Inherits Potion

    Public Property PoisonType As PoisonType
    Public Property DMGMultiple As Int16

    Public Sub New()
        MyBase.New()

        Color = ColorList.Green
        PType = PotionType.Poison

        'TODO: Modify drink message based on poison type
        Message = "urk... Poison! You throw up."

        'TODO: adjust poison walkover to reflect ItemFactory value
        Walkover = "green potion"
        Name = "poison"
    End Sub

    Public Sub New(PoisonType As PoisonType)
        Me.New()
        Me.PoisonType = PoisonType

        Select Case Me.PoisonType
            Case PoisonType.mild
                DMGMultiple = 1
            Case PoisonType.medium
                DMGMultiple = 2
            Case PoisonType.strong
                DMGMultiple = 3
        End Select
    End Sub

    Public Overrides Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
		Dim strMessage As String = ""

		' is it paper? Destroy it.
		If WhatIsBeingDipped.IsPaper Then strMessage = WhatIsBeingDipped.Destroy(Me)

		' is it another liquid? Mix it.
		If WhatIsBeingDipped.IsLiquid Then strMessage = WhatIsBeingDipped.Mix(Me)

		If WhatIsBeingDipped.IsPoisonable Then strMessage = WhatIsBeingDipped.ApplyPoison(Me)

		Return strMessage
	End Function

	Public Overrides Function drink(WhoIsDrinking As Avatar) As String
        'TODO: Drink Poison

        WhoIsDrinking.Poisoned = True
        WhoIsDrinking.CurrentHP -= RND.Next(1, WhoIsDrinking.CurrentHP \ 2)
        If WhoIsDrinking.CurrentHP <= 0 Then WhoIsDrinking.Dead = True
        WhoIsDrinking.PoisonDuration = TheHero.TurnCount + D4() + D4()

        Return Message
    End Function
End Class
