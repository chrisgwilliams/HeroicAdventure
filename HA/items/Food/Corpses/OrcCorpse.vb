Imports HA.Common

Public Class OrcCorpse
	Inherits Corpse
	
	Public Overrides Property CorpseEffect As String
	Public Overrides Property Description As String
	Public Overrides Property Race As String

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.Green
		Walkover = "orc corpse"
		Weight = 0.1
		Name = "orc corpse"
		Nutrition = 100
		LifeSpan = 50

		Description = "It's a dead orc. "
		CorpseEffect = "Not bad. Could use a little salt. "
		Race = "orc"

		Rotten = False
		Cooked = False
	End Sub

	Public Overrides Function eat(WhoIsEating As Avatar) As String
		'TODO: eat an orc corpse
		' Check for blessed / cursed / uncursed status
		' check for rotten status

		Return CorpseEffect
	End Function


End Class
