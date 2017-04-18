Imports HA.Common

Public Class BlankScroll
	Inherits Scroll
	
	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "blank scroll"
		Name = "blank scroll"

	End Sub

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		Return "You attempt to read the scroll but it is blank. There is nothing here to cast. "
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		Return "You attempt to read the scroll but it is blank. There is nothing here to learn. "
	End Function

	Public Overrides Property MagicType As MagicType

	Public Overrides Function read(WhoIsReading As Avatar) As String
		Return "You attempt to read the scroll but it is blank. "
	End Function
End Class
