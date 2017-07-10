Imports HA.Common

Public Class TrollCorpse
	Inherits Corpse
	
	Public Overrides Property CorpseEffect As String
	Public Overrides Property Description As String
	Public Overrides Property Race As String

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.DarkGray
		Walkover = "troll corpse"
		Weight = 0.1
		Name = "troll corpse"
		Nutrition = 200
		LifeSpan = 50

		Description = "It's a dead troll. "
		CorpseEffect = "It's really tough, like chewing on rocks. "
		Race = "troll"

		Rotten = False
		Cooked = False
	End Sub

    Public Overrides Function eat(whoIsEating As Avatar) As String
        ' TODO: eat a Troll Corpse
        ' Check for blessed / cursed / uncursed status
        ' check for rotten status
        ' TODO: pull Hero healing rate into property so it can be modified by external factors (like eating a troll)

        Return CorpseEffect
    End Function

End Class
