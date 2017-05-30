' Yes I know Spiders aren't technically animals, but it's close enough.

Imports HA.Common

#Region "    -- Spider Subclass and Inherited Subclasses "

' inherits from monster

Public MustInherit Class Spider
	Inherits Monster

	Protected Sub New()
		MyBase.New()

		Character = "S"
		Sight = 3
		Intelligence = 0
		Wisdom = 10
		Dexterity = 17
		Constitution = 12
		Charisma = 2

		Chat = "The spider glares at you."

		HitDie = 8
		HasHands = False
		Initiative = 3
		Attacks = 1
	End Sub
End Class

' these all inherit from Spider
Public Class LargeSpider
	Inherits Spider

	Public Sub New()
		MyBase.New()

		MonsterRace = "large spider"
		CR = 2
		Color = ColorList.Purple

		Strength = 15

		' 4d8+4 HD
		Dim intCtr As Integer
		For intCtr = 1 To 4
			HP += D8()
		Next
		HP += 4
		CurrentHP = HP

		AC = 14

		AttackList.Clear()

		Dim bite As MonsterAttackType
		bite.Name = "huge teeth"
		bite.Verb = "bites"
		bite.Bonus = 4
		bite.MinDamage = 4
		bite.MaxDamage = 11
		bite.Poison = True

		AttackList.Add(bite)
	End Sub
End Class
Public Class HugeSpider
	Inherits Spider

	Public Sub New()
		MyBase.New()

		MonsterRace = "huge spider"
		CR = 4
		Color = ColorList.Magenta

		Strength = 19

		' 10d8+10 HD
		Dim intCtr As Integer
		For intCtr = 1 To 10
			HP += D8()
		Next
		HP += 10
		CurrentHP = HP

		AC = 16

		AttackList.Clear()

        Dim bite As MonsterAttackType
        bite.Name = "huge teeth"
		bite.Verb = "bites"
		bite.Bonus = 9
		bite.MinDamage = 8
		bite.MaxDamage = 18
		bite.Poison = True

		AttackList.Add(bite)
	End Sub
End Class

#End Region

