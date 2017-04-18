Imports HA.Common

Public Class SoAmnesia
	Inherits Scroll


	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of ebanimsa"
		Name = "scroll of amnesia"

	End Sub

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Amnesia

		Return ""
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Amnesia

		Return ""
	End Function

	Public Overrides Property MagicType As MagicType

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read an Amnesia scroll

		Return ""

	End Function
End Class