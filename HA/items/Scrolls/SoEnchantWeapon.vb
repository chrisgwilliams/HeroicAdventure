Imports HA.Common

Public Class SoEnchantWeapon
	Inherits Scroll
	
	Public Sub New()
		MyBase.New()

		Color = Enumerations.ColorList.White
		Walkover = "scroll of amrif ser"
		Name = "scroll of enchant weapon"

	End Sub

	Public Overrides Function read(WhoIsReading As Avatar) As String
		' TODO: read an Enchant Weapon scroll

		Return ""
	End Function

	Public Overrides Function cast(WhoIsCasting As Avatar) As String
		' TODO: Cast Enchant Weapon

		Return ""
	End Function

	Public Overrides Function learn(WhoIsLearning As Avatar) As String
		' TODO: Learn Enchant Weapon

		Return ""
	End Function

	Public Overrides Property MagicType As MagicType

End Class
