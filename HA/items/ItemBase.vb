Imports HA.Common.Enumerations

#Region " Item Base Class "

Public MustInherit Class ItemBase

    'TODO: Add code to increase / decrease weight in equippedweight and backpackweight whenever an equippable item is activated / deactivated
    Private intType As ItemType
    Private intColor As ConsoleColor
    Private strSymbol As String
	Private bolBreakable As Boolean
	Private bolPaper As Boolean
	Private bolRustable As Boolean
	Private bolLiquid As Boolean
	Private bolPoisonable As Boolean
	Private bolMissle As Boolean
	Private bolTool As Boolean
	Private intState As DivineState
	Private bolIdentified As Boolean
	Private bolHasCharges As Boolean
	Private bolFlammable As Boolean
	Private bolDirty As Boolean
	Private sglWeight As Single
	Private intQuantity As Integer
	Private strWalkoverText As String
	Private strDamage As String
	Private strName As String
	Private intStatBonus As Integer
	'Private intStatAffected As Integer
	Private intCharges As Integer
	Private intMarketValue As Integer
	Private intACBonus As Integer
	Private intAtkBonus As Integer

    Public Property Type() As ItemType
        Get
            Type = intType
        End Get
        Set(ByVal Value As ItemType)
            intType = Value
        End Set
    End Property
    Public Property Color() As ConsoleColor
        Get
            Color = intColor
        End Get
        Set(ByVal Value As ConsoleColor)
            intColor = Value
        End Set
    End Property
    Public Property Symbol() As String
		Get
			Symbol = strSymbol
		End Get
		Set(ByVal value As String)
			strSymbol = value
		End Set
	End Property

	Public Property IsBreakable() As Boolean
		Get
			Return bolBreakable
		End Get
		Set(ByVal value As Boolean)
			bolBreakable = value
		End Set
	End Property
	Public Property IsPaper() As Boolean
		Get
			Return bolPaper
		End Get
		Set(ByVal value As Boolean)
			bolPaper = value
		End Set
	End Property
	Public Property IsRustable() As Boolean
		Get
			Return bolRustable
		End Get
		Set(ByVal value As Boolean)
			bolRustable = value
		End Set
	End Property
	Public Property IsLiquid() As Boolean
		Get
			Return bolLiquid
		End Get
		Set(ByVal value As Boolean)
			bolLiquid = value
		End Set
	End Property
	Public Property IsPoisonable() As Boolean
		Get
			Return bolPoisonable
		End Get
		Set(value As Boolean)
			bolPoisonable = value
		End Set
	End Property
	Public Property IsDirty() As Boolean
		Get
			Return bolDirty
		End Get
		Set(ByVal value As Boolean)
			bolDirty = value
		End Set
	End Property
	Public Property Missle() As Boolean
		Get
			Missle = bolMissle
		End Get
		Set(ByVal Value As Boolean)
			bolMissle = Value
		End Set
	End Property
	Public Property Tool() As Boolean
		Get
			Tool = bolTool
		End Get
		Set(ByVal Value As Boolean)
			bolTool = Value
		End Set
	End Property
	Public Property Weight() As Single
		Get
			Weight = sglWeight
		End Get
		Set(ByVal Value As Single)
			sglWeight = Value
		End Set
	End Property
	Public Property Quantity() As Integer
		Get
			Quantity = intQuantity
		End Get
		Set(ByVal Value As Integer)
			intQuantity = Value
		End Set
	End Property
	Public Property Walkover() As String
		Get
			Walkover = strWalkoverText
		End Get
		Set(ByVal Value As String)
			strWalkoverText = Value
		End Set
	End Property
	Public Property Damage() As String
		Get
			Damage = strDamage
		End Get
		Set(ByVal Value As String)
			strDamage = Value
		End Set
	End Property
	Public Property Name() As String
		Get
			Name = strName
		End Get
		Set(ByVal Value As String)
			strName = Value
		End Set
	End Property
	Public Property Identified() As Boolean
		Get
			Identified = bolIdentified
		End Get
		Set(ByVal Value As Boolean)
			bolIdentified = Value
		End Set
	End Property
	Public Property StatBonus() As Integer
		Get
			StatBonus = intStatBonus
		End Get
		Set(ByVal Value As Integer)
			intStatBonus = Value
		End Set
	End Property
	'Public Property StatAffected() As Integer
	'	Get
	'		StatAffected = intStatAffected
	'	End Get
	'	Set(ByVal Value As Integer)
	'		intStatAffected = Value
	'	End Set
	'End Property
	Public Property HasCharges() As Boolean
		Get
			HasCharges = bolHasCharges
		End Get
		Set(ByVal Value As Boolean)
			bolHasCharges = Value
		End Set
	End Property
	Public Property Charges() As Integer
		Get
			Charges = intCharges
		End Get
		Set(ByVal Value As Integer)
			intCharges = Value
		End Set
	End Property
	Public Property Flammable() As Boolean
		Get
			Flammable = bolFlammable
		End Get
		Set(ByVal Value As Boolean)
			bolFlammable = Value
		End Set
	End Property
	Public Property MarketValue() As Integer
		Get
			MarketValue = intMarketValue
		End Get
		Set(ByVal Value As Integer)
			intMarketValue = Value
		End Set
	End Property
	Public Property ACBonus() As Integer
		Get
			ACBonus = intACBonus
		End Get
		Set(ByVal Value As Integer)
			intACBonus = Value
		End Set
	End Property
	Public Property AtkBonus() As Integer
		Get
			AtkBonus = intAtkBonus
		End Get
		Set(ByVal value As Integer)
			intAtkBonus = value
		End Set
	End Property
	Public Property ItemState() As DivineState
		Get
			Return intState
		End Get
		Set(ByVal value As DivineState)
			intState = value
		End Set
	End Property

	Public Function Bless(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		Dim secondaryMessage As String = ""

		If DippedInto.ItemState = DivineState.Blessed AndAlso DippedInto.Name = "water" Then

			Select Case Me.ItemState
				Case DivineState.Normal
					Me.ItemState = DivineState.Blessed
					secondaryMessage = "You feel good about the " + Me.Name + ". You feel as though your god is pleased with you. "
				'TODO: implement piety change for adding a blessing to an item.

				Case DivineState.Cursed
					Me.ItemState = DivineState.Normal
					secondaryMessage = "You feel better about the " + Me.Name + ". "

				Case DivineState.Blessed
					Me.ItemState = DivineState.Blessed
					secondaryMessage = "You don't feel any different about the " + Me.Name + ". "

			End Select
		End If

		If DippedInto.ItemState = DivineState.Cursed AndAlso DippedInto.Name = "water" Then

			Select Case Me.ItemState
				Case DivineState.Normal
					Me.ItemState = DivineState.Cursed
					secondaryMessage = "You have a bad feeling about the " + Me.Name + ". "

				Case DivineState.Cursed
					Me.ItemState = DivineState.Cursed
					secondaryMessage = "You don't feel any different about the " + Me.Name + ". "

				Case DivineState.Blessed
					Me.ItemState = DivineState.Normal
					secondaryMessage = "The " + DippedInto.Name + " begins to bubble and hiss. You feel as if your god is disappointed in you. "
					'TODO: implement piety change for removing a blessing from an item.
			End Select
		End If


		strMessage += "You dip the " + Me.Name + " into the " + DippedInto.Name + ". " + secondaryMessage

		Return strMessage

		'TODO: Destroy potion after dipping something into it.
	End Function

	Public Function Destroy(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		strMessage &= String.Format("The {0} is destroyed when you dip it into the {1}.", Name, DippedInto.Name)

		'''If itemArray(intItemIndex).quantity = 1 Then
		'''    TheHero.Equipment.BackPack.Remove(itemArray(intItemIndex))
		'''    itemArray.RemoveAt(intItemIndex)

		'''ElseIf itemArray(intItemIndex).quantity > 1 Then
		'''    TheHero.Equipment.BackPack.Remove(itemArray(intItemIndex))
		'''    itemArray(intItemIndex).quantity -= 1
		'''    TheHero.Equipment.BackPack.Add(itemArray(intItemIndex))
		'''End If

		'TODO: Fix item destruction when dipped into liquid

		Return strMessage
	End Function

	Public Function Rust(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		' TODO: Rust an item
		' TODO: Hero luck should affect rust chance
		Return strMessage
	End Function

	Public Function Mix(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		' TODO: Mix potions
		Return strMessage
	End Function

	Public Function Clean(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		' TODO: Clean something
		Return strMessage
	End Function

	Public Function ApplyPoison(ByVal DippedInto As Potion, Optional ByVal strMessage As String = "") As String
		' TODO: Make something poisoned
		Return strMessage
	End Function

	Public Enum DivineState As Integer
		Cursed = -1
		Normal = 0
		Blessed = 1
	End Enum

End Class

#End Region


