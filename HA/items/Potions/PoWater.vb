
Imports HA.Common

Public Class PoWater
	Inherits Potion
	
	Public Sub New()
		MyBase.New()

		Color = ColorList.Blue
		PType = PotionType.Water
		Message = "ahh... cool refreshing water. "
		Walkover = "blue potion"
		Name = "water"
	End Sub

	Public Overrides Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
		Dim strMessage As String = ""

		' is it paper? Destroy it.
		If WhatIsBeingDipped.IsPaper Then strMessage = WhatIsBeingDipped.Destroy(Me)

		' is it metal AND NOT BLESSED? Rust it.
		If WhatIsBeingDipped.IsRustable And WhatIsBeingDipped.ItemState <> DivineState.Blessed Then strMessage = WhatIsBeingDipped.Rust(Me)

		' is it another liquid? Mix it.
		If WhatIsBeingDipped.IsLiquid Then strMessage = WhatIsBeingDipped.Mix(Me)

		' is it anything else? Clean it.
		If WhatIsBeingDipped.IsDirty Then strMessage = WhatIsBeingDipped.Clean(Me)

		' is this water blessed/cursed? Bless/curse the item.
		If WhatIsBeingDipped.ItemState = DivineState.Blessed _
		   And Me.ItemState = DivineState.Blessed _
		Then WhatIsBeingDipped.ItemState = DivineState.Blessed

		If WhatIsBeingDipped.ItemState = DivineState.Normal _
		   And Me.ItemState = DivineState.Blessed _
		Then WhatIsBeingDipped.ItemState = DivineState.Blessed

		If WhatIsBeingDipped.ItemState = DivineState.Cursed _
		   And Me.ItemState = DivineState.Blessed _
		Then WhatIsBeingDipped.ItemState = DivineState.Normal

		If WhatIsBeingDipped.ItemState = DivineState.Blessed _
		   And Me.ItemState = DivineState.Cursed _
		Then WhatIsBeingDipped.ItemState = DivineState.Normal

		If WhatIsBeingDipped.ItemState = DivineState.Normal _
		   And Me.ItemState = DivineState.Cursed _
		Then WhatIsBeingDipped.ItemState = DivineState.Cursed

		If WhatIsBeingDipped.ItemState = DivineState.Cursed _
		   And Me.ItemState = DivineState.Cursed _
		Then WhatIsBeingDipped.ItemState = DivineState.Cursed

		Return strMessage
	End Function

	Public Overrides Function drink(WhoIsDrinking As Avatar) As String
		'TODO: add benefits/penalties of drinking b/c water

		Select Case Me.ItemState
			Case DivineState.Blessed

			Case DivineState.Normal

			Case DivineState.Cursed

		End Select

		Return Me.Message

	End Function
End Class
