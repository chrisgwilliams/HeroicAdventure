Imports HA.Common

#Region " Dog Family "

<System.Diagnostics.DebuggerStepThrough()> Public Class Dog
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "dog"
		CR = 1
		Color = ColorList.Olive
		Character = "d"
		Sight = 6
		Strength = 15
		Intelligence = 2
		Wisdom = 12
		Dexterity = 15
		Constitution = 15
		Charisma = 6

		Chat = "woof woof! woof!"

		HitDie = 8 'd8
		HP = D8()
		HP += D8()
		HP += 4
		CurrentHP = HP

		HasHands = False

		AC = 16
		Initiative = 2
		Attacks = 1

		Dim bite As MonsterAttackType
		bite.Name = "sharp teeth"
		bite.Verb = "bites"
		bite.Bonus = 3
		bite.MinDamage = 4
		bite.MaxDamage = 9

		AttackList.Add(bite)
	End Sub
End Class

Public Class Puppy
	Inherits Dog

	Public Sub New()
		MyBase.New()

		MonsterRace = "feral puppy"
		MonsterRacePlural = "feral puppies"
		CR = 0.33
		Color = ColorList.Yellow
		Character = "d"
		Sight = 5
		Strength = 5
		Intelligence = 2
		Wisdom = 5
		Dexterity = 15
		Constitution = 10
		Charisma = 14

		Chat = "yip yap yip!"

		HitDie = 4 'd4
		HP = D4()
		HP += D4()
		CurrentHP = HP

		HasHands = False

		AC = 12
		Initiative = 4
		Attacks = 1

		Dim bite As MonsterAttackType
		bite.Name = "tiny teeth"
		bite.Verb = "bites"
		bite.Bonus = 2
		bite.MinDamage = 1
		bite.MaxDamage = 2

		AttackList.Add(bite)
	End Sub
End Class

#End Region
