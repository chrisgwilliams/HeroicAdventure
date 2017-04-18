Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Kobold
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "kobold"
		CR = 0.166
		Color = ColorList.Green
		Character = "k"
		Sight = 4
		DarkVision = 6

		Strength = 6
		Intelligence = 10
		Wisdom = 10
		Dexterity = 13
		Constitution = 11
		Charisma = 10

		Chat = "Bark! Hiss! The kobold spits angrily!"

		HitDie = 4
		HP += D4()
		CurrentHP = HP

		AC = 15
		Initiative = 1
		Attacks = 1

		Dim halfspear As Structures.MonsterAttackType
		halfspear.Name = "halfspear"
		halfspear.Verb = "stabs"
		halfspear.Bonus = -1
		halfspear.MinDamage = 0
		halfspear.MaxDamage = 4

		AttackList.Add(halfspear)
	End Sub
End Class
