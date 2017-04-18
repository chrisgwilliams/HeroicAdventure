Imports HA.Common

Public Class BlankScroll
	Inherits Scroll

	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "blank scroll"
		Name = "blank scroll"

	End Sub
End Class
