Imports HA.Common

#Region "    -- Rat Subclass and Inherited Subclasses "

<System.Diagnostics.DebuggerStepThrough()> Public Class Rat
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "rat"
		CR = 0.125
		Color = ColorList.Olive
		Character = "r"
		Sight = 6
		Strength = 2
		Intelligence = 2
		Wisdom = 12
		Dexterity = 15
		Constitution = 10
		Charisma = 2

		Chat = "squeak squeeeeeeak! squeak!"

		HitDieType = 2 '1/4 d8
		HP = RND.Next(1, HitDieType)
		CurrentHP = HP

		HasHands = False

		AC = 14
		Initiative = 2
		Attacks = 1

		Dim bite As MonsterAttackType
		bite.Name = "pointy teeth"
		bite.Verb = "bites"
		bite.Bonus = 4
		bite.MinDamage = 1
		bite.MaxDamage = 2

		AttackList.Add(bite)
	End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()> Public Class DireRat
	Inherits Rat

	Public Sub New()
		MyBase.New()

		MonsterRace = "dire rat"
		CR = 0.33
		Color = ColorList.Maroon

		Strength = 10
		Intelligence = 1
		Dexterity = 17
		Constitution = 12
		Charisma = 4

		Chat = "SQUEEEEAK! The foul breath nearly knocks you over."

		HitDieType = 8 'd8+1
		HP = D8() + 1
		CurrentHP = HP

		AC = 15
		Initiative = 3

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "filthy teeth"
		bite.Verb = "bites"
		bite.Bonus = 4
		bite.MinDamage = 1
		bite.MaxDamage = 4

		AttackList.Add(bite)
	End Sub
End Class

#End Region
