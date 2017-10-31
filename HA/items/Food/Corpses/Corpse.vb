Public MustInherit Class Corpse
	Inherits FoodBase

	' eating some corpses will have special effects on Hero.
	' could be good, could be bad...

	'TODO: Set Hero.Cannibal = true if Hero eats corpse of same race.

	Public Sub New()
		MyBase.New()
	End Sub

    Public Property Race As String
    Public Property Description As String
    Public Property CorpseEffect As String

    Public MustOverride Overrides Function eat(whoIsEating As Avatar) As String

End Class
