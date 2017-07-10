Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Orc
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "orc"
		CR = 0.5
		Color = ColorList.Green
		Character = "o"
		Sight = 5
		Strength = 15
		Intelligence = 9
		Wisdom = 8
		Dexterity = 10
		Constitution = 11
		Charisma = 8

		Chat = "Bree Yark!"

		HitDieType = 8
		HP = D8()
		CurrentHP = HP

		AC = 14
		Initiative = 0
		Attacks = 1

		Dim greataxe As MonsterAttackType
		greataxe.Name = "greataxe"
		greataxe.Verb = "guts"
		greataxe.Bonus = 3
		greataxe.MinDamage = 4
		greataxe.MaxDamage = 15

		AttackList.Add(greataxe)
	End Sub
End Class
