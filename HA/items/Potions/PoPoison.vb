Imports HA.Common

Public Class PoPoison
	Inherits Potion
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Green
		PType = PotionType.Poison
		Message = "urk... Poison! You throw up."
		Walkover = "green potion"
		Name = "poison"
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
