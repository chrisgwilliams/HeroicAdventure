Imports HA.Common

Public Class ItemFactory
	Friend Shared Sub InitializeItems()
		'TODO: this is where we randomly assign item names/colors/descriptions at world generation
		'TODO: Scrub For Dupes

		'Potions are identified by color 
		ItemDetails.Potion.ExtraHealing = D20(15)
		ItemDetails.Potion.Healing = D20(15)
		ItemDetails.Potion.Invisibility = D20(15)
		ItemDetails.Potion.Poison = D20(15)
		ItemDetails.Potion.Water = D20(15)

		'Scrolls are identified by gibberish names
		ItemDetails.Scroll.Amnesia = Scramble(New SoAmnesia().Name)
		ItemDetails.Scroll.Blank = New BlankScroll().Name
		ItemDetails.Scroll.CureLight = Scramble(New SoCureLight().Name)
		ItemDetails.Scroll.EnchantWpn = Scramble(New SoEnchantWeapon().Name)
		ItemDetails.Scroll.Identify = Scramble(New SoIdentify().Name)
		ItemDetails.Scroll.MagicMap = Scramble(New SoMagicMap().Name)
		ItemDetails.Scroll.MagicMissile = Scramble(New SoMagicMissile().Name)

		'Wands are identified by wood type
		'TODO:	list of available wood types
		ItemDetails.Wand.Light = D6()
		ItemDetails.Wand.MagicMap = D6()
		ItemDetails.Wand.MagicMissile = D6()
		'	assign at random

		'Rings are identified by metal
		ItemDetails.Ring.Dexterity = D10()
		ItemDetails.Ring.Intellect = D10()
		ItemDetails.Ring.Invisibility = D10()
		ItemDetails.Ring.Protection = D10()
		ItemDetails.Ring.Strength = D10()

		'Amulets are identified by gem
		'TODO:	list of amulet types
		'	assign at random
		ItemDetails.Amulet.Intellect = D4()
		ItemDetails.Amulet.Invisibility = D4()
		ItemDetails.Amulet.Protection = D4()
		ItemDetails.Amulet.Strength = D4()
	End Sub

	Public Class ItemDetails
		Public Class Potion
			Public Shared ExtraHealing As ColorList
			Public Shared Healing As ColorList
			Public Shared Invisibility As ColorList
			Public Shared Poison As ColorList
			Public Shared Water As ColorList
		End Class

		Public Class Ring
			Public Shared Dexterity As MetalList
			Public Shared Intellect As MetalList
			Public Shared Invisibility As MetalList
			Public Shared Protection As MetalList
			Public Shared Strength As MetalList
		End Class

		Public Class Scroll
			Public Shared Amnesia As String
			Public Shared Blank As String
			Public Shared CureLight As String
			Public Shared EnchantWpn As String
			Public Shared Identify As String
			Public Shared MagicMap As String
			Public Shared MagicMissile As String
		End Class

		Public Class Amulet
			Public Shared Intellect As GemList
			Public Shared Invisibility As GemList
			Public Shared Strength As GemList
			Public Shared Protection As GemList
		End Class

		Public Class Wand
			Public Shared Light As WandList
			Public Shared MagicMap As WandList
			Public Shared MagicMissile As WandList
		End Class
	End Class

	Private Shared Function GetUnique(ByVal list As [Enum]) As Object
		Dim newlist
	End Function
End Class
