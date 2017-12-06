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

        HitDie = 8 'd8
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
