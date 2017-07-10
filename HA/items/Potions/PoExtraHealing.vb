Imports HA.Common

Public Class PoExtraHealing
	Inherits Potion
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.White
		PType = PotionType.ExtraHealing
		Message = "glug glug... you feel much better."
		Walkover = "white potion"
		Name = "potion of extra healing"
	End Sub

	Public Overrides Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
		Dim strMessage As String = ""

		' is it paper? Destroy it.
		If WhatIsBeingDipped.IsPaper Then strMessage = WhatIsBeingDipped.Destroy(Me)

		' is it another liquid? Mix it.
		If WhatIsBeingDipped.IsLiquid Then strMessage = WhatIsBeingDipped.Mix(Me)

		Return strMessage
	End Function

	Public Overrides Function drink(WhoIsDrinking As Avatar) As String
        ' TODO: Add Luck Factor to Extra Healing result
        ' TODO: If CurrentHP = HP before drinking !oXH (and !oXH is not Cursed), add a point to HP total
        With WhoIsDrinking
			Select Case ItemState
				Case DivineState.Blessed
					.CurrentHP += D12() + D12()
				Case DivineState.Normal
					.CurrentHP += D8() + D8()
				Case DivineState.Cursed
					.CurrentHP += D4() + D4()
			End Select

			If .CurrentHP > .HP Then .CurrentHP = .HP
		End With

		Return Message
	End Function
End Class
