Imports HA.Common

Public Class SoCureLight
	Inherits Scroll

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of higal monarin"
		Name = "scroll of minor healing"

	End Sub

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read a Cure Light scroll

		Return ""
	End Function

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Cure Light Wounds

		Return ""
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Cure Light Wounds

		Return ""
	End Function

	Public Overrides Property MagicType As MagicType

End Class
