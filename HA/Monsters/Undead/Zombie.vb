Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Zombie
	Inherits Undead

	Public Sub New()
		MyBase.New()

		MonsterRace = "zombie"
		CR = 0.5
		Color = ColorList.LightGray
		Character = "z"
		Sight = 6
		Strength = 13
		Intelligence = 0
		Wisdom = 10
		Dexterity = 8
		Constitution = 0
		Charisma = 1

		Chat = "braaaaaains!"

		HitDie = 12	' 2d12+3
		HP = D12()
		HP += D12()
		HP += 3
		CurrentHP = HP

		AC = 11
		Initiative = -1
		Attacks = 1

		AttackList.Clear()

        Dim slam As MonsterAttackType
        slam.Name = "powerful fists"
		slam.Verb = "beats"
		slam.Bonus = 2
		slam.MinDamage = 2
		slam.MaxDamage = 7

		AttackList.Add(slam)
	End Sub
End Class

