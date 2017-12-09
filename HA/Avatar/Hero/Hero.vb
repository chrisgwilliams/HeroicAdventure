Imports System.Collections.Generic
Imports HA.Common
Imports HA.Common.Helper

Public Class Hero
    ' see Avatar class for base stats (i.e. strength, etc)
    Inherits Avatar

    Public Property HeroRace() As Race
    Public Property HeroClass() As PCClass
    Public Property Name() As String
    Public Property TotalLevels() As Integer
    Public Property BaseAtkBonus() As Integer

    Public Property Weapontype() As Integer
    Public Property ArmorType() As Integer
    Public Property ShieldType() As Integer

    Public Property TurnCount() As Integer
    Public Property TurnCountAtDungeonLevelChange() As Integer
    Public Property CurrentLevel() As Integer
    Public Property CurrentDungeon() As String
    Public Property XP() As Integer

#Region " Headstone Info "

    Public Property DeepestLevel() As Integer
    Public Property KilledBy() As String
    Public Property TotalKills() As Integer

#End Region

    Public Property StarSign() As StarSign
    Public Property SpellResist() As Integer
    Public Property Flight() As Integer
    Public Property Energy() As Integer
    Public Property CurrentEnergy() As Integer

    '********************************************************************************************************
    'Cullen's changes for sorting characters by initiative
    'at some point, the initiative for the round should probably also include a random roll; that addition
    'would go here
    Public Overrides ReadOnly Property TotalInitForRound() As Integer
        Get
            Return AbilityMod(EDexterity) + Initiative + D10()
        End Get
    End Property

#Region " Effective Stats "

    Public Overrides Property EStrength() As Integer
    Public Overrides Property EIntelligence() As Integer
    Public Overrides Property EWisdom() As Integer
    Public Overrides Property EDexterity() As Integer
    Public Overrides Property EConstitution() As Integer
    Public Overrides Property ECharisma() As Integer

#End Region

    Public Property WeaponSkill() As List(Of WeaponMastery)
    Public Property Luck() As Integer
    Public Property Skills() As New Hashtable
    Public Property CasterType() As MagicType


    Public Sub New()
        TurnCount = 0
        Sight = 3
        TotalLevels = 1
        CurrentLevel = 0
        DeepestLevel = 0
        GP = 0

        ' setup the initial values for skills everyone has
        BaselineSkills()

        Equipped.BackPack = New ArrayList
        EStrength = Strength
        EIntelligence = Intelligence
        EWisdom = Wisdom
        EDexterity = Dexterity
        EConstitution = Constitution
        ECharisma = Charisma
    End Sub

    Public Sub CastSpell()
        'TODO: Magic System

        If Me.HeroClass = PCClass.Druid Or Me.HeroClass = PCClass.Priest Then
            ' Divine Spellcasters

        ElseIf Me.HeroClass = PCClass.Sorceror Or Me.HeroClass = PCClass.Wizard Then
            ' Arcane Spellcasters

        Else
            ' Non-Spellcasters

        End If
    End Sub
    Public Sub UseWand()

    End Sub
    '    Public Sub DrinkPotion(ByVal potion As PotionType)
    '        Select Case potion
    '            Case PotionType.Healing     ' heals 1d8
    '                Me.CurrentHP += D8()
    '                If Me.CurrentHP > Me.HP Then Me.CurrentHP = Me.HP

    '            Case PotionType.ExtraHealing    ' heals 2d8
    '                Me.CurrentHP += D8()
    '                Me.CurrentHP += D8()
    '                If Me.CurrentHP > Me.HP Then Me.CurrentHP = Me.HP

    '            Case PotionType.Poison          ' causes 1 to .5HP plus 1d4 on a 1 in 10 for 2d4 turns
    '                Me.Poisoned = True
    '                Me.CurrentHP -= RND.Next(1, Me.CurrentHP \ 2)
    '                If Me.CurrentHP <= 0 Then Me.Dead = True
    '                Me.PoisonDuration = Me.TurnCount + D4() + D4()

    '			Case PotionType.Invisibility
    '                    Me.Invisible = True
    '                    Me.InvisibilityDuration = Me.TurnCount + (D20() + 15)
    '        End Select
    '    End Sub

    Private Sub BaselineSkills()
        Skills.Add("Search", 0)
        Skills.Add("Awareness", 0)
        Skills.Add("First Aid", 0)
        Skills.Add("Listen", 0)
        Skills.Add("Hide", 0)

    End Sub

    Public Function CheckStatuses(strMessage As String) As String
        ' if hero is Sick, decrement duration and check for damage
        strMessage = CheckForSickness(strMessage)

        ' if hero is poisoned, decrement duration and check for damage
        strMessage = CheckForPoison(strMessage)

        ' if hero is confused, decrement duration
        strMessage = CheckForConfusion(strMessage)

        ' if hero is sleeping, decrement duration
        strMessage = CheckForSleeping(strMessage)

        ' if hero is invisible, decrement duration
        strMessage = CheckForInvisible(strMessage)

        Return strMessage
    End Function
End Class
