Imports HA.Common

#Region " Snake Family "

' inherits from monster

Public MustInherit Class Snake
	Inherits Monster

	Public Sub New()
		MyBase.New()

		Character = "s"
		Sight = 5
		Intelligence = 1
		Wisdom = 12
		Dexterity = 17
		Constitution = 11
		Charisma = 2

		Chat = "Hissssssssssssss!"

		HitDieType = 8
		HasHands = False
		HasFeet = False
		Initiative = 3
		Attacks = 1
	End Sub

End Class

' these all inherit from Snake
Public Class Viper
	Inherits Snake

	Public Sub New()
		MyBase.New()

		MonsterRace = "viper"
		CR = 2
		Color = ColorList.Green

		Strength = 8

		' 2d8 HD
		Dim intCtr As Integer
		For intCtr = 1 To 2
			HP += D8()
		Next
		CurrentHP = HP

		AC = 12

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "sharp fangs"
		bite.Verb = "bites"
		bite.Bonus = 4
		bite.MinDamage = 4
		bite.MaxDamage = 11
		bite.Poison = True

		AttackList.Add(bite)
	End Sub
End Class

#End Region
