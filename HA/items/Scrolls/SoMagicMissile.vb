Imports HA.Common

Public Class SoMagicMissile
	Inherits Scroll

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of suql ziwpaegh"
		Name = "scroll of magic missile"
		MagicType = MagicType.Arcane

	End Sub

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read a Magic Missile scroll

		Return ""
	End Function

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Magic Missile

		Return ""
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Magic Missile

		Return ""
	End Function

	Public Overrides Property MagicType As MagicType
End Class
