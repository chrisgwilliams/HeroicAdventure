Public MustInherit Class Corpse
	Inherits FoodBase

	' eating some corpses will have special effects on Hero.
	' could be good, could be bad...

	'TODO: Set Hero.Cannibal = true if Hero eats corpse of same race.

	Public Sub New()
		MyBase.New()
	End Sub

	Public MustOverride Property Race As String
	Public MustOverride Property Description As String
	Public MustOverride Property CorpseEffect As String

	Public MustOverride Overrides Function eat(WhoIsEating As Avatar) As String

End Class
