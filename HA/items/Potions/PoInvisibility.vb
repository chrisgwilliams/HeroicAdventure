Imports HA.Common

Public Class PoInvisibility
	Inherits Potion

	Public Sub New()
		MyBase.New()

		Color = ColorList.Cyan
		PType = PotionType.Invisibility
		Message = "glug glug... you turn invisible."
		Walkover = "light blue potion"
		Name = "potion of invisibility"
	End Sub

	Public Overrides Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
		Dim strMessage As String = ""

		' is it paper? Destroy it.
		If WhatIsBeingDipped.IsPaper Then strMessage = WhatIsBeingDipped.Destroy(Me)

		' is it another liquid? Mix it.
		If WhatIsBeingDipped.IsLiquid Then strMessage = WhatIsBeingDipped.Mix(Me)

		'is it any other type of item? Make it invisible for a few turns
		'TODO: Make Item invisible.

		Return strMessage
	End Function

	Public Overrides Function drink(WhoIsDrinking As Avatar) As String
		' TODO: Drink Invisibility Potion
		WhoIsDrinking.Invisible = True
		WhoIsDrinking.InvisibilityDuration = D4() * D10()

		Return Me.Message
	End Function
End Class
