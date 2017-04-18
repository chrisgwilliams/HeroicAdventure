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

