Imports HA.Common

Public MustInherit Class Scroll
	Inherits ItemBase
	Implements iPaperItem
	Implements iMagicSpell
	
	Public Sub New()
		Type = Enumerations.ItemType.Scroll

		Symbol = "~"
		Flammable = True
		Weight = 0.1
		Quantity = 1
		IsPaper = True
	End Sub

	Public MustOverride Function read(WhoIsReading As Avatar) As String Implements iPaperItem.read

	Public MustOverride Function cast(WhoIsCasting As Avatar) As String Implements iMagicSpell.cast

	Public MustOverride Function learn(WhoIsLearning As Avatar) As String Implements iMagicSpell.learn

	Public MustOverride Property MagicType As MagicType Implements iMagicSpell.MagicType
End Class
