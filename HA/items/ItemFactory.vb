Imports System.Collections.Generic
Imports HA.Common

Public Class ItemFactory
    Friend Shared Sub InitializeItems()

        'Potions are identified by color 
        Dim TotalColors As Int16 = [Enum].GetValues(GetType(ColorList)).Length
        Dim arrColors() As Int16 = GetRandomListOfUniqueInts(TotalColors)
        ItemDetails.Potion.ExtraHealing = arrColors(0)
        ItemDetails.Potion.Healing = arrColors(1)
        ItemDetails.Potion.Invisibility = arrColors(2)
        ItemDetails.Potion.Poison = arrColors(3)
        ItemDetails.Potion.Water = arrColors(4)

        'Scrolls are identified by gibberish names
        ItemDetails.Scroll.Amnesia = Scramble(New SoAmnesia().Name)
        ItemDetails.Scroll.Blank = New BlankScroll().Name
        ItemDetails.Scroll.CureLight = Scramble(New SoCureLight().Name)
        ItemDetails.Scroll.EnchantWpn = Scramble(New SoEnchantWeapon().Name)
        ItemDetails.Scroll.Identify = Scramble(New SoIdentify().Name)
        ItemDetails.Scroll.MagicMap = Scramble(New SoMagicMap().Name)
        ItemDetails.Scroll.MagicMissile = Scramble(New SoMagicMissile().Name)

        'Wands are identified by material type
        Dim TotalWands As Int16 = [Enum].GetValues(GetType(WandList)).Length
        Dim arrWands() As Int16 = GetRandomListOfUniqueInts(TotalWands)
        ItemDetails.Wand.Light = arrWands(0)
        ItemDetails.Wand.MagicMap = arrWands(1)
        ItemDetails.Wand.MagicMissile = arrWands(2)

        'Rings are identified by metal
        Dim TotalMetals As Int16 = [Enum].GetValues(GetType(MetalList)).Length
        Dim arrMetals() As Int16 = GetRandomListOfUniqueInts(TotalMetals)
        ItemDetails.Ring.Dexterity = arrMetals(0)
        ItemDetails.Ring.Intellect = arrMetals(1)
        ItemDetails.Ring.Invisibility = arrMetals(2)
        ItemDetails.Ring.Protection = arrMetals(3)
        ItemDetails.Ring.Strength = arrMetals(4)

        'Amulets are identified by gem
        Dim TotalGems As Int16 = [Enum].GetValues(GetType(GemList)).Length
        Dim arrGems() As Int16 = GetRandomListOfUniqueInts(TotalGems)
        ItemDetails.Amulet.Intellect = arrGems(0)
        ItemDetails.Amulet.Invisibility = arrGems(1)
        ItemDetails.Amulet.Protection = arrGems(2)
        ItemDetails.Amulet.Strength = arrGems(3)
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

End Class
