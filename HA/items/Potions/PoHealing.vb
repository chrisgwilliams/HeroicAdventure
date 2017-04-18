Imports HA.Common

Public Class PoHealing
	Inherits Potion

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		PType = PotionType.Healing
		Message = "glug glug... you feel better. "
		Walkover = "white potion"
		Name = "potion of healing"
	End Sub

	Public Overrides Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
		'TODO: Dip into Healing Potion
		Return ""
	End Function

	Public Overrides Function drink(WhoIsDrinking As Avatar) As String
		' TODO: Add Luck Factor to Healing result
		With WhoIsDrinking
			Select Case ItemState
				Case DivineState.Blessed
					.CurrentHP += D12()
				Case DivineState.Normal
					.CurrentHP += D8()
				Case DivineState.Cursed
					.CurrentHP += D4()
			End Select

			If .CurrentHP > .HP Then .CurrentHP = .HP
		End With

		Return Message
	End Function
End Class
