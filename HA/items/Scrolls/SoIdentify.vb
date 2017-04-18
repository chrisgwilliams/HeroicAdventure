Imports HA.Common

Public Class SoIdentify
	Inherits Scroll

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of yskeiawl"
		Name = "scroll of identify"

	End Sub

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Identify

		Return ""
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Identify

		Return ""
	End Function

	Public Overrides Property MagicType As MagicType

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read an Identify scroll

		Return ""

	End Function
End Class