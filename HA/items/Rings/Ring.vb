Imports HA.Common

Public MustInherit Class Ring
	Inherits ItemBase
	Implements iEquippableItem

	Public Sub New()
		Type = ItemType.Ring

		Symbol = "="
		Weight = 0.5
		Quantity = 1

		Dim statChance As Integer = D100() + TheHero.Luck + TheHero.CurrentLevel
		Select Case statChance
			Case 1 To 59
				StatBonus = 1
			Case 60 To 89
				StatBonus = 2
			Case 90 To 97
				Name = "greater " + Name
				StatBonus = 3
			Case Is >= 98
				Name = "major " + Name
				StatBonus = 4
		End Select
	End Sub

	Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
	Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate

End Class
