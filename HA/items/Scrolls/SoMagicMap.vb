Imports HA.Common

Public Class SoMagicMap
	Inherits Scroll

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of pam sreduaram"
		Name = "scroll of magic mapping"

	End Sub

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Magic Map

		Return ""

	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Magic Map

		Return ""

	End Function

	Public Overrides Property MagicType As MagicType

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read a Magic Map scroll

		Return ""

	End Function
End Class
