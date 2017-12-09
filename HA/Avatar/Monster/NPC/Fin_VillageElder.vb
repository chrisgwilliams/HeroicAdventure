Imports HA.Common

Public Class Fin_VillageElder
    Inherits NPC

    Public Sub New()
        MyBase.New()

        MonsterRace = "village elder"
        CR = 10
        Color = ColorList.White
        Character = "@"
        Sight = 5
        Strength = 25
        Intelligence = 25
        Wisdom = 25
        Dexterity = 25
        Constitution = 25
        Charisma = 25
        HP = 200

        AC += 8
        Initiative += 10
        Attacks = 4

        ' attacks
        Dim openhand As MonsterAttackType
        openhand.Name = "open hand"
        openhand.Verb = "strikes"
        openhand.Bonus = 10
        openhand.MinDamage = 20
        openhand.MaxDamage = 40

        Dim closedfist As MonsterAttackType
        closedfist.Name = "closed fist"
        closedfist.Verb = "punches"
        closedfist.Bonus = 8
        closedfist.MinDamage = 15
        closedfist.MaxDamage = 30

        AttackList.Clear()
        AttackList.Add(openhand)
        AttackList.Add(closedfist)
        AttackList.Add(openhand)
        AttackList.Add(closedfist)

        ' research for Fincastle Elder NPC
        HomeMap = Town.Fincastle
        LocX = 64
        LocY = 11
        Chat = "Hello. I wasn't expecting visitors, but you can find adventure northwest of here."

        QuestMessage = ""
        QuestUnresolved = ""
        QuestResolved = ""
    End Sub

End Class
