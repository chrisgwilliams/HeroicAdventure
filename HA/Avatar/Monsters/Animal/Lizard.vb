Imports HA.Common

#Region " Lizard Family "

<System.Diagnostics.DebuggerStepThrough()> Public Class Lizard
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "lizard"
		CR = 0.33
		Color = ColorList.Green
		Character = "l"
		Sight = 4
		Strength = 15
		Intelligence = 2
		Wisdom = 2
		Dexterity = 15
		Constitution = 15
		Charisma = 2

		Chat = "hisssss!"

		HitDieType = 4 'd4+2
		HP = D4()
		HP += 2
		CurrentHP = HP

		HasHands = False

		AC = 16
		Initiative = 2
		Attacks = 1

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "teeth"
		bite.Verb = "bites"
		bite.Bonus = 3
		bite.MinDamage = 4
		bite.MaxDamage = 9

		AttackList.Add(bite)
	End Sub
End Class

Public Class BabyLizard
	Inherits Lizard

	Public Sub New()
		MyBase.New()

		MonsterRace = "baby lizard"
		CR = 0.22
		Color = ColorList.DarkGreen
		Strength = 5
		Constitution = 8

		HitDieType = 4 'd4
		HP = D4()
		CurrentHP = HP

		AC = 10

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "tiny teeth"
		bite.Verb = "bites"
		bite.Bonus = 2
		bite.MinDamage = 1
		bite.MaxDamage = 3

		AttackList.Add(bite)
	End Sub

End Class
#End Region
