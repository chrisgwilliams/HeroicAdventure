Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Goblin
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "goblin"
		CR = 0.25
		Color = ColorList.Red
		Character = "g"
		Sight = 4
		Strength = 8
		Intelligence = 10
		Wisdom = 11
		Dexterity = 13
		Constitution = 11
		Charisma = 8

		Chat = "Got any gold?"

		HitDieType = 8
		HP = D8()
		CurrentHP = HP

		AC = 15
		Initiative = 1
		Attacks = 1

        Dim morningstar As MonsterAttackType
        morningstar.Name = "morningstar"
		morningstar.Verb = "bashes"
		morningstar.Bonus = 1
		morningstar.MinDamage = 0
		morningstar.MaxDamage = 7

		AttackList.Add(morningstar)
	End Sub
End Class

