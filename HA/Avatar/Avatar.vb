Imports HA.Common
Imports HA.Common.Helper

' Avatar is the base class for all PCs, NPCs and Monsters
Public MustInherit Class Avatar
    Friend Equipped As BodyLocations

#Region " Physical and Mental Attributes "
    Public MustOverride Property EStrength() As Integer
    Public MustOverride Property EIntelligence() As Integer
    Public MustOverride Property EWisdom() As Integer
    Public MustOverride Property EDexterity() As Integer
    Public MustOverride Property EConstitution() As Integer
    Public MustOverride Property ECharisma() As Integer

    Public Property Strength() As Int16
    Public Property Intelligence() As Int16
    Public Property Wisdom() As Int16
    Public Property Dexterity() As Int16
    Public Property Constitution() As Int16
    Public Property Charisma() As Int16
#End Region

#Region " Stat Mods "

    Public Property StrMods() As Integer
    Public Property IntMods() As Integer
    Public Property WisMods() As Integer
    Public Property DexMods() As Integer
    Public Property ConMods() As Integer
    Public Property ChaMods() As Integer

#End Region

#Region " Effective Stat Modifiers "

    Public ReadOnly Property StrMod() As Integer
        Get
            Return AbilityMod(EStrength)
        End Get
    End Property
    Public ReadOnly Property IntMod() As Integer
        Get
            Return AbilityMod(EIntelligence)
        End Get
    End Property
    Public ReadOnly Property WisMod() As Integer
        Get
            Return AbilityMod(EWisdom)
        End Get
    End Property
    Public ReadOnly Property ConMod() As Integer
        Get
            Return AbilityMod(EConstitution)
        End Get
    End Property
    Public ReadOnly Property DexMod() As Integer
        Get
            Return AbilityMod(EDexterity)
        End Get
    End Property
    Public ReadOnly Property ChaMod() As Integer
        Get
            Return AbilityMod(ECharisma)
        End Get
    End Property

#End Region

#Region " Status Flags "

    Public Property Cannibal() As Boolean

    Public Property Charmed As Boolean
    Public Property CharmedDuration() As Int16

    Public Property Confused() As Boolean
    Public Property ConfusionDuration() As Int16

    Public Property Invisible() As Boolean
    Public Property InvisibilityDuration() As Int16

    Public Property Paralyzed() As Boolean
    Public Property ParalysisDuration() As Int16

    Public Property Poisoned() As Boolean
    Public Property PoisonDuration() As Int16

    Public Property Sick() As Boolean
    Public Property SickDuration() As Int16

    Public Property Sleeping() As Boolean
    Public Property SleepDuration() As Int16

#End Region

#Region " Resistances "

    Public Property CharmResist() As Int16
    Public Property SleepResist() As Int16
    Public Property MagicResist() As Int16
    Public Property PoisonResist() As Int16
    Public Property ColdResist() As Int16
    Public Property SickResist() As Int16

#End Region

    Public Property Gender() As Sex

    Public Property Nourishment As Integer
    Public Property HungerLevel As HungerState

    Public Property Piety As Integer
    Public Property PietyLevel As PietyState
    Public Property Prayers As Int16

    Public Property HasHands() As Boolean
    Public Property HasFeet() As Boolean
    Public Property Dead() As Boolean
    Public Property NaturalArmor() As Int16
    Public Property Sight() As Int16
    Public Property DarkVision() As Int16

    Public Property EquippedWeight As Integer
    Public Property BackpackWeight As Integer
    Public Property GP() As Integer

    Public Property HitDieType() As Int16
    Public Property HP() As Integer
    Public Property CurrentHP() As Integer
    Public Property Regeneration As Double

    Public Property AC() As Int16
    Public Property MiscACMod() As Int16
    Public Property Initiative() As Int16

    Public MustOverride ReadOnly Property TotalInitForRound() As Integer

    ' meta data
    Public Property AutoWalk() As Boolean
    Public Property Icon() As String
    Public Property Color() As ConsoleColor
    Public Property LocX() As Int16
    Public Property LocY() As Int16
    Public Property LocZ() As Int16
    Public Property OverX() As Int16
    Public Property OverY() As Int16
    Public Property Overland() As Boolean
    Public Property InTown() As Boolean
    Public Property TerrainZoom() As Boolean
    Public Property Town() As Town

#Region " Methods "

    Public Sub Walk(ByVal direction As Integer)
        If TheHero.InTown Then
            Select Case direction
                Case 1
                    Me.LocX -= 1
                    Me.LocY += 1
                Case 2
                    Me.LocY += 1
                Case 3
                    Me.LocX += 1
                    Me.LocY += 1
                Case 4
                    Me.LocX -= 1
                Case 5

                Case 6
                    Me.LocX += 1
                Case 7
                    Me.LocX -= 1
                    Me.LocY -= 1
                Case 8
                    Me.LocY -= 1
                Case 9
                    Me.LocX += 1
                    Me.LocY -= 1
            End Select
        ElseIf TheHero.TerrainZoom Then
            Select Case direction
                Case 1
                    Me.LocX -= 1
                    Me.LocY += 1
                Case 2
                    Me.LocY += 1
                Case 3
                    Me.LocX += 1
                    Me.LocY += 1
                Case 4
                    Me.LocX -= 1
                Case 5

                Case 6
                    Me.LocX += 1
                Case 7
                    Me.LocX -= 1
                    Me.LocY -= 1
                Case 8
                    Me.LocY -= 1
                Case 9
                    Me.LocX += 1
                    Me.LocY -= 1
            End Select
        ElseIf TheHero.Overland Then
            Select Case direction
                Case 1
                    Me.OverX -= 1
                    Me.OverY += 1
                Case 2
                    Me.OverY += 1
                Case 3
                    Me.OverX += 1
                    Me.OverY += 1
                Case 4
                    Me.OverX -= 1
                Case 5

                Case 6
                    Me.OverX += 1
                Case 7
                    Me.OverX -= 1
                    Me.OverY -= 1
                Case 8
                    Me.OverY -= 1
                Case 9
                    Me.OverX += 1
                    Me.OverY -= 1
            End Select
        Else
            Select Case direction
                Case 1
                    Me.LocX -= 1
                    Me.LocY += 1
                Case 2
                    Me.LocY += 1
                Case 3
                    Me.LocX += 1
                    Me.LocY += 1
                Case 4
                    Me.LocX -= 1
                Case 5

                Case 6
                    Me.LocX += 1
                Case 7
                    Me.LocX -= 1
                    Me.LocY -= 1
                Case 8
                    Me.LocY -= 1
                Case 9
                    Me.LocX += 1
                    Me.LocY -= 1
            End Select
        End If
    End Sub
    Public Sub ResistPoison()

    End Sub

#End Region

#Region "Enumerations"

    Public Enum Sex
        female = 1
        male = 2
    End Enum

    Public Enum PietyState
        Hated = -3
        Unfavored = -2
        Displeased = -1
        Normal = 0
        Pleased = 1
        Favored = 2
        Loved = 3
    End Enum

    Public Enum HungerState
        Starving
        Hungry
        Normal
        Satiated
        Bloated
    End Enum

#End Region

End Class


