Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Skeleton
	Inherits Undead

	Public Sub New()
		MyBase.New()

		MonsterRace = "skeleton"
		CR = 0.5
		Color = ColorList.White
		Character = "z"
		Sight = 6
		Strength = 17
		Intelligence = 1
		Wisdom = 1
		Dexterity = 15
		Constitution = 20
		Charisma = 1

		Chat = "Rattle me bones, it's freezing in here."

		HitDie = 12
		HP = D12()
		CurrentHP = HP

		AC = 13
		Initiative = 5
		Attacks = 2

		AttackList.Clear()

		Dim claw As Structures.MonsterAttackType
		claw.Name = "claw"
		claw.Verb = "tears"
		claw.Bonus = 0
		claw.MinDamage = 1
		claw.MaxDamage = 4

		AttackList.Add(claw)
		AttackList.Add(claw)
	End Sub
End Class

