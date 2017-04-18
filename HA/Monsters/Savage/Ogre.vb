Imports HA.Common

#Region " Ogre Family "

<System.Diagnostics.DebuggerStepThrough()> Public Class Ogre
	Inherits Monster

	Public Sub New()
		MyBase.New()

		MonsterRace = "ogre"
		CR = 2
		Color = ColorList.Olive
		Character = "O"
		Sight = 3
		Strength = 21
		Intelligence = 6
		Wisdom = 10
		Dexterity = 8
		Constitution = 15
		Charisma = 7

		Chat = "*grunt* You smell funny! Hahaha"

		HitDie = 8 '4d8+8
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += 8
		CurrentHP = HP

		AC = 16
		Initiative = -1
		Attacks = 1

		Dim hugegreatclub As MonsterAttackType
		hugegreatclub.Name = "huge greatclub"
		hugegreatclub.Verb = "smashes"
		hugegreatclub.Bonus = 8
		hugegreatclub.MinDamage = 9
		hugegreatclub.MaxDamage = 19

		AttackList.Add(hugegreatclub)
	End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()> Public Class OgreMage
	Inherits Ogre

	Public Sub New()
		MyBase.New()

		MonsterRace = "ogre mage"
		CR = 8
		Color = ColorList.Cyan
		Sight += 1

		Intelligence += 8
		Wisdom += 4
		Dexterity += 2
		Constitution += 2
		Charisma += 10

		Chat = "Insignificant fool, you are not worthy of my conversation."

		HitDie = 8 '5d8+15
		HP = D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += 15
		CurrentHP = HP

		AC += 2
		Initiative += 5
		Attacks = 1

		AttackList.Clear()

		Dim hugegreatsword As MonsterAttackType
		hugegreatsword.Name = "huge greatsword"
		hugegreatsword.Verb = "strikes"
		hugegreatsword.Bonus = 7
		hugegreatsword.MinDamage = 9
		hugegreatsword.MaxDamage = 23

		AttackList.Add(hugegreatsword)
	End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()> Public Class OgrePrince
	Inherits Ogre

	Public Sub New()
		MyBase.New()

		MonsterRace = "ogre prince"
		CR = 12
		Color = ColorList.Cyan
		Sight += 2

		Strength += 3
		Intelligence += 6
		Wisdom += 4
		Dexterity += 4
		Constitution += 3
		Charisma += 4

		Chat = "You dare speak to me? For that you shall die!"

		HitDie = 8 '7d8+20
		HP = D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += D8()
		HP += 20
		CurrentHP = HP

		AC += 4
		Initiative += 7
		Attacks = 2

		AttackList.Clear()

		Dim hugegreataxe As Structures.MonsterAttackType
		hugegreataxe.Name = "huge greataxe"
		hugegreataxe.Verb = "cleaves"
		hugegreataxe.Bonus = 7
		hugegreataxe.MinDamage = 9
		hugegreataxe.MaxDamage = 24

		AttackList.Add(hugegreataxe)
		AttackList.Add(hugegreataxe)
	End Sub
End Class

#End Region

