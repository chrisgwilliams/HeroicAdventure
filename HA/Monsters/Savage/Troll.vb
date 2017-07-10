Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Troll
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "troll"
		CR = 5
		Color = ColorList.DarkGray
		Character = "T"
		Sight = 4
		DarkVision = 9

		Strength = 23
		Intelligence = 6
		Wisdom = 9
		Dexterity = 14
		Constitution = 23
		Charisma = 6

		Chat = "Raaarrgh! Food not talk!"

		HitDieType = 8 ' 6d8 + 36
		Dim intCtr As Integer
		For intCtr = 1 To 6
			HP += D8()
		Next
		HP += 36
		CurrentHP = HP

		AC = 18
		Initiative = 2
		Attacks = 3

		Dim claw As MonsterAttackType
		claw.Name = "claw"
		claw.Verb = "slashes"
		claw.Bonus = 9
		claw.MinDamage = 7
		claw.MaxDamage = 12

        Dim bite As MonsterAttackType
        bite.Name = "nasty teeth"
		bite.Verb = "bites"
		bite.Bonus = 4
		bite.MinDamage = 4
		bite.MaxDamage = 9

		AttackList.Add(claw)
		AttackList.Add(claw)
		AttackList.Add(bite)
	End Sub
End Class
