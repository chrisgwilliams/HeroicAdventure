Imports System.Collections.Generic
Imports HA.Common
Imports HA.Common.Helper


' inherits from Avatar
<System.Diagnostics.DebuggerStepThrough()> Public Class Hero
	' see Avatar class for base stats (i.e. strength, etc)

	Inherits Avatar

	Private myRace As Race
	Private strClass As String
	Private myName As String
	Private intTotalLevels As Integer
	Private intBaseAttackBonus As Integer
	Private intArmor As Integer
	Private intShield As Integer

	Private intWeapons As Integer
	Private intTurnCount As Integer
	Private intTurnCountAtDungeonLevelChange As Integer
	Private intTotalKills As Integer
	Private loWeaponMastery As List(Of WeaponMastery)
	Private intXP As Integer
	Private strCurrentDungeon As String
	Private intDeepestLevel As Integer
	Private intCurrentLevel As Integer
	Private strKilledBy As String
	Private intSpellResistance As Integer
	Private intFlight As Integer
	Private intEnergy As Integer
	Private intCurrEnergy As Integer
	Private EffStrength As Integer
	Private EffIntelligence As Integer
	Private EffWisdom As Integer
	Private EffDexterity As Integer
	Private EffConstitution As Integer
	Private EffCharisma As Integer
	Private intLuck As Integer
	Private mtCastingType As MagicType

	Private bCannibal As Boolean

	Private htSkills As New Hashtable

	Public Property HeroRace() As Race
		Get
			HeroRace = myRace
		End Get
		Set(ByVal Value As Race)
			myRace = Value
		End Set
	End Property
	Public Property HeroClass() As Integer
		Get
			HeroClass = strClass
		End Get
		Set(ByVal Value As Integer)
			strClass = Value
		End Set
	End Property
	Public Property Name() As String
		Get
			Name = myName
		End Get
		Set(ByVal Value As String)
			myName = Value
		End Set
	End Property
	Public Property TotalLevels() As Integer
		Get
			TotalLevels = intTotalLevels
		End Get
		Set(ByVal Value As Integer)
			intTotalLevels = Value
		End Set
	End Property
	Public Property BaseAtkBonus() As Integer
		Get
			BaseAtkBonus = intBaseAttackBonus
		End Get
		Set(ByVal Value As Integer)
			intBaseAttackBonus = Value
		End Set
	End Property

	Public Property Weapontype() As Integer
		Get
			Weapontype = intWeapons
		End Get
		Set(ByVal Value As Integer)
			intWeapons = Value
		End Set
	End Property
	Public Property ArmorType() As Integer
		Get
			ArmorType = intArmor
		End Get
		Set(ByVal Value As Integer)
			intArmor = Value
		End Set
	End Property
	Public Property ShieldType() As Integer
		Get
			ShieldType = intShield
		End Get
		Set(ByVal Value As Integer)
			intShield = Value
		End Set
	End Property

	Public Property TurnCount() As Integer
		Get
			TurnCount = intTurnCount
		End Get
		Set(ByVal Value As Integer)
			intTurnCount = Value
		End Set
	End Property
	Public Property TurnCountAtDungeonLevelChange() As Integer
		Get
			TurnCountAtDungeonLevelChange = intTurnCountAtDungeonLevelChange
		End Get
		Set(ByVal Value As Integer)
			intTurnCountAtDungeonLevelChange = Value
		End Set
	End Property
	Public Property CurrentLevel() As Integer
		Get
			CurrentLevel = intCurrentLevel
		End Get
		Set(ByVal Value As Integer)
			intCurrentLevel = Value
		End Set
	End Property
	Public Property CurrentDungeon() As String
		Get
			CurrentDungeon = strCurrentDungeon
		End Get
		Set(ByVal Value As String)
			strCurrentDungeon = Value
		End Set
	End Property

	Public Property XP() As Integer
		Get
			XP = intXP
		End Get
		Set(ByVal Value As Integer)
			intXP = Value
		End Set
	End Property

#Region " Headstone Info "

	Public Property DeepestLevel() As Integer
		Get
			DeepestLevel = intDeepestLevel
		End Get
		Set(ByVal Value As Integer)
			intDeepestLevel = Value
		End Set
	End Property
	Public Property KilledBy() As String
		Get
			KilledBy = strKilledBy
		End Get
		Set(ByVal Value As String)
			strKilledBy = Value
		End Set
	End Property
	Public Property TotalKills() As Integer
		Get
			TotalKills = intTotalKills
		End Get
		Set(ByVal Value As Integer)
			intTotalKills = Value
		End Set
	End Property

	Public Property Cannibal As Boolean
		Get
			Cannibal = bCannibal
		End Get
		Set(Value As Boolean)
			bCannibal = Value
		End Set
	End Property

#End Region

	Public Property SpellResist() As Integer
		Get
			SpellResist = intSpellResistance
		End Get
		Set(ByVal Value As Integer)
			intSpellResistance = Value
		End Set
	End Property
	Public Property Flight() As Integer
		Get
			Flight = intFlight
		End Get
		Set(ByVal Value As Integer)
			intFlight = Value
		End Set
	End Property
	Public Property Energy() As Integer
		Get
			Energy = intEnergy
		End Get
		Set(ByVal Value As Integer)
			intEnergy = Value
		End Set
	End Property
	Public Property CurrentEnergy() As Integer
		Get
			CurrentEnergy = intCurrEnergy
		End Get
		Set(ByVal Value As Integer)
			intCurrEnergy = Value
		End Set
	End Property

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
		Get
			EStrength = EffStrength
		End Get
		Set(ByVal Value As Integer)
			EffStrength = Value
		End Set
	End Property
	Public Overrides Property EIntelligence() As Integer
		Get
			EIntelligence = EffIntelligence
		End Get
		Set(ByVal Value As Integer)
			EffIntelligence = Value
		End Set
	End Property
	Public Overrides Property EWisdom() As Integer
		Get
			EWisdom = EffWisdom
		End Get
		Set(ByVal Value As Integer)
			EffWisdom = Value
		End Set
	End Property
	Public Overrides Property EDexterity() As Integer
		Get
			EDexterity = EffDexterity
		End Get
		Set(ByVal Value As Integer)
			EffDexterity = Value
		End Set
	End Property
	Public Overrides Property EConstitution() As Integer
		Get
			EConstitution = EffConstitution
		End Get
		Set(ByVal Value As Integer)
			EffConstitution = Value
		End Set
	End Property
	Public Overrides Property ECharisma() As Integer
		Get
			ECharisma = EffCharisma
		End Get
		Set(ByVal Value As Integer)
			EffCharisma = Value
		End Set
	End Property

#End Region

	Public Property WeaponSkill() As List(Of WeaponMastery)
		Get
			WeaponSkill = loWeaponMastery
		End Get
		Set(value As List(Of WeaponMastery))
			loWeaponMastery = value
		End Set

	End Property

	Public Property Luck() As Integer
		Get
			Luck = intLuck
		End Get
		Set(ByVal value As Integer)
			intLuck = value
		End Set
	End Property

	Public Property Skills() As Hashtable
		Get
			Skills = htSkills
		End Get
		Set(ByVal value As Hashtable)
			htSkills = value
		End Set
	End Property

	Public Property CasterType() As MagicType
		Get
			Return mtCastingType
		End Get
		Set(ByVal value As MagicType)
			mtCastingType = value
		End Set
	End Property

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
	Public Sub DrinkPotion(ByVal potion As PotionType)
		'TODO: rewrite DrinkPotion to use interfaces for items
		'TODO: implement chance of drinking wrong potion when confused

		Select Case potion
			Case PotionType.Healing     ' heals 1d8
				Me.CurrentHP += D8()
				If Me.CurrentHP > Me.HP Then Me.CurrentHP = Me.HP

			Case PotionType.ExtraHealing    ' heals 2d8
				Me.CurrentHP += D8()
				Me.CurrentHP += D8()
				If Me.CurrentHP > Me.HP Then Me.CurrentHP = Me.HP

			Case PotionType.Poison          ' causes 1 to .5HP plus 1d4 on a 1 in 10 for 2d4 turns
				Me.Poisoned = True
				Me.CurrentHP -= RND.Next(1, Me.CurrentHP \ 2)
				If Me.CurrentHP <= 0 Then Me.Dead = True
				Me.PoisonDuration = Me.TurnCount + D4() + D4()

			Case PotionType.Water
				' TODO: do something for hunger/nutrition when drinking water
				' TODO: factor in b/u/c status

			Case PotionType.Invisibility
				Me.Invisible = True
				Me.InvisibilityDuration = Me.TurnCount + (D20() + 15)
		End Select
	End Sub

	Private Sub BaselineSkills()
		htSkills.Add("Search", 0)
		htSkills.Add("Awareness", 0)
		htSkills.Add("First Aid", 0)
		htSkills.Add("Listen", 0)
		htSkills.Add("Hide", 0)

	End Sub

	Public Function CheckStatuses(strMessage As String) As String
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


#Region " -- NPC Class "

' inherits from Hero
Public Class NPC
    ' see Hero and Avatar classes for base stats, etc
    Inherits Hero

    Private bolStranger As Boolean
    Private strGreeting As String

    Public Property Stranger() As Boolean
        Get
            Stranger = bolStranger
        End Get
        Set(ByVal Value As Boolean)
            bolStranger = Value
        End Set
    End Property
    Public Property Greeting() As String
        Get
            Greeting = strGreeting
        End Get
        Set(ByVal Value As String)
            strGreeting = Value
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        Stranger = True
    End Sub

End Class

#End Region



