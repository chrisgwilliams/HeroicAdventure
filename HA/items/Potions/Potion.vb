Imports HA.Common

Public MustInherit Class Potion
	Inherits ItemBase
	Implements iLiquidItem

	Private intPType As Int16
	Private strPMessage As String
	Private intDuration As Int16

	Public Property PType() As Int16
		Get
			PType = intPType
		End Get
		Set(ByVal Value As Int16)
			intPType = Value
		End Set
	End Property
	Public Property Message() As String
		Get
			Message = strPMessage
		End Get
		Set(ByVal Value As String)
			strPMessage = Value
		End Set
	End Property
	Public Property Duration() As Int16
		Get
			Duration = intDuration
		End Get
		Set(ByVal Value As Int16)
			intDuration = Value
		End Set
	End Property

	Public Sub New()
		Type = Enumerations.ItemType.Potion
		Symbol = "!"
		IsBreakable = True
		Missle = True
		Tool = False
		Quantity = 1
	End Sub

	Public MustOverride Function dip(ByRef WhatIsBeingDipped As ItemBase) As String Implements iLiquidItem.dip
	Public MustOverride Function drink(WhoIsDrinking As Avatar) As String Implements iLiquidItem.drink
End Class

