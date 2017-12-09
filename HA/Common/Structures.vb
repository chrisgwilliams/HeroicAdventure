Namespace Common

    Public Structure BodyLocations
        Dim Helmet As Helmet
        Dim Neck As Amulet
        Dim Cloak As Cloak
        Dim Girdle As Girdle
        Dim Armor As Armor
        Dim LeftHand As Object
        Dim RightHand As Object
        Dim LeftRing As Ring
        Dim RightRing As Ring
        Dim Gloves As Gloves
        Dim Bracers As Bracers
        Dim Boots As Boots

        Dim MissleWeapon As Object
        Dim Missles As Object

        Dim Tool As Tool
        Dim BackPack As ArrayList
    End Structure

    Public Structure MonsterAttackType
        Dim Name As String
        Dim Verb As String
        Dim Bonus As Integer
        Dim MinDamage As Integer
        Dim MaxDamage As Integer
        Dim Poison As Boolean
    End Structure

    Public Structure WeaponMastery
        Public WeaponType As WeaponType
        Public Crits As Integer
        Public Hits As Integer
        Public Swings As Integer
        Public Fumbles As Integer
    End Structure

    Public Structure Award
        Public Name As String
        Public Points As Integer
        Public Achieved As Boolean
    End Structure

    Public Structure Quest
        Public Name As String
        Public Questgiver As NPC
        Public Status As QuestStatus
        Public ReportTo As NPC  ' Used for MeetingQuest, KillQuest, and FedExQuest

        ' Quest Types
        Public MeetingQuest As Boolean
        Public KillQuest As Boolean
        Public FedExQuest As Boolean

        ' Kill Quest
        Public KillType As Monster
        Public KillGoal As Int16
        Public KillCount As Int16

        ' FedEx Quest
        Public PackageList As ArrayList

        ' Rewards
        Public XPReward As Int16
        Public GPReward As Int16
        Public ItemReward As ItemBase

        ' Conversation
        Public IntroMsg() As String
        Public UnresolvedMsg() As String
        Public ResolvedMsg() As String
    End Structure
End Namespace