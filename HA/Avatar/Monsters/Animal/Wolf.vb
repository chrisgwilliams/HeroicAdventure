' Also part of the Dog Family, but broken out separately for classification purposes
Imports HA.Common

<System.Diagnostics.DebuggerStepThrough()> Public Class Wolf
	Inherits Dog

	Public Sub New()
		MyBase.New()

		MonsterRace = "wolf"
		MonsterRacePlural = "wolves"
		CR = 1
		Color = ColorList.White
		Strength = 13
		Chat = "grrrrrrrr! awooooo!"

		AC = 14

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "huge teeth"
		bite.Verb = "bites"
		bite.Bonus = 3
		bite.MinDamage = 2
		bite.MaxDamage = 7

		AttackList.Add(bite)

	End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()> Public Class DireWolf
	Inherits Wolf

	Public Sub New()
		MyBase.New()

		MonsterRace = "dire wolf"
		MonsterRacePlural = "dire wolves"
		CR = 3
		Color = ColorList.LightGray
		Strength = 25
		Constitution = 17
		Charisma = 10

		Chat = "grrrrrrrrowwwwl!!"

		HitDieType = 8 'd8
		HP = D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += 18
		CurrentHP = HP

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "vicious fangs"
		bite.Verb = "bites"
		bite.Bonus = 10
		bite.MinDamage = 11
		bite.MaxDamage = 18

		AttackList.Add(bite)
	End Sub
End Class
