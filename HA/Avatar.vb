Imports HA.Common

#Region " Avatar Base Class "

' Avatar is the base class for all PCs, NPCs and Monsters

<System.Diagnostics.DebuggerStepThrough()> Public MustInherit Class Avatar
	Private strName As String
	Private intNaturalArmor As Integer
	Private intSight As Integer
	Private intDarkVision As Integer
	Private intHitDie As Integer
	Private intHitPoints As Integer
	Private intCurrentHP As Integer
	Private intAC As Integer
	Private intMiscACMod As Integer
	Private intInitiative As Integer
	Private intAvatarColor As Integer
	Private intAvatarIcon As String
	Private intX As Integer
	Private intY As Integer
	Private intZ As Integer
	Private intOverX As Integer
	Private intOverY As Integer
	Private intCharmResist As Integer
	Private intGender As Integer
	Private bolDead As Boolean
	Private bolHasHands As Boolean
	Private bolHasFeet As Boolean
	Private bolIsInvisible As Boolean
	Private intInvisibilityDuration As Int16
	Private bolPoisoned As Boolean
	Private intPoisonDuration As Int16
	Private intPoisonResist As Int16
	Private intColdResist As Int16
	Private bolConfused As Boolean
	Private intConfusionDuration As Int16
	Private intSleepResist As Int16
	Private intMagicResist As Int16
	Private bolSleeping As Boolean
	Private intSleepDuration As Int16
	Private bolAutoWalk As Boolean
	Private intGP As Int16
	Private bolOverland As Boolean
	Private bolInTown As Boolean
	Private bolTerrainZoom As Boolean
	Private intTown As Town = -1
	Private intEquippedWeight As Integer
	Private intBackpackWeight As Integer
	Friend Equipped As Structures.BodyLocations

#Region " Physical and Mental Attributes "
	Public MustOverride Property EStrength() As Integer
	Public MustOverride Property EIntelligence() As Integer
	Public MustOverride Property EWisdom() As Integer
	Public MustOverride Property EDexterity() As Integer
	Public MustOverride Property EConstitution() As Integer
	Public MustOverride Property ECharisma() As Integer

	Private intStr As Integer
	Private intInt As Integer
	Private intWis As Integer
	Private intDex As Integer
	Private intCon As Integer
	Private intCha As Integer

	Public Property Strength() As Int16
		Get
			Strength = intStr
		End Get
		Set(ByVal Value As Int16)
			intStr = Value
		End Set
	End Property
	Public Property Intelligence() As Int16
		Get
			Intelligence = intInt
		End Get
		Set(ByVal Value As Int16)
			intInt = Value
		End Set
	End Property
	Public Property Wisdom() As Int16
		Get
			Wisdom = intWis
		End Get
		Set(ByVal Value As Int16)
			intWis = Value
		End Set
	End Property
	Public Property Dexterity() As Int16
		Get
			Dexterity = intDex
		End Get
		Set(ByVal Value As Int16)
			intDex = Value
		End Set
	End Property
	Public Property Constitution() As Int16
		Get
			Constitution = intCon
		End Get
		Set(ByVal Value As Int16)
			intCon = Value
		End Set
	End Property
	Public Property Charisma() As Int16
		Get
			Charisma = intCha
		End Get
		Set(ByVal Value As Int16)
			intCha = Value
		End Set
	End Property
#End Region

#Region " Stat Mods "

	Private intStrMods As Integer
	Private intIntMods As Integer
	Private intWisMods As Integer
	Private intDexMods As Integer
	Private intConMods As Integer
	Private intChaMods As Integer

	Public Property StrMods() As Integer
		Get
			StrMods = intStrMods
		End Get
		Set(ByVal Value As Integer)
			intStrMods = Value
		End Set
	End Property
	Public Property IntMods() As Integer
		Get
			IntMods = intIntMods
		End Get
		Set(ByVal Value As Integer)
			intIntMods = Value
		End Set
	End Property
	Public Property WisMods() As Integer
		Get
			WisMods = intWisMods
		End Get
		Set(ByVal Value As Integer)
			intWisMods = Value
		End Set
	End Property
	Public Property DexMods() As Integer
		Get
			DexMods = intDexMods
		End Get
		Set(ByVal Value As Integer)
			intDexMods = Value
		End Set
	End Property
	Public Property ConMods() As Integer
		Get
			ConMods = intConMods
		End Get
		Set(ByVal Value As Integer)
			intConMods = Value
		End Set
	End Property
	Public Property ChaMods() As Integer
		Get
			ChaMods = intChaMods
		End Get
		Set(ByVal Value As Integer)
			intChaMods = Value
		End Set
	End Property
#End Region


	Public Property Gender() As Int16
		Get
			Gender = intGender
		End Get
		Set(ByVal Value As Int16)
			intGender = Value
		End Set
	End Property
	Public Property HasHands() As Boolean
		Get
			HasHands = bolHasHands
		End Get
		Set(ByVal Value As Boolean)
			bolHasHands = Value
		End Set
	End Property
	Public Property HasFeet() As Boolean
		Get
			HasFeet = bolHasFeet
		End Get
		Set(ByVal value As Boolean)
			bolHasFeet = value
		End Set
	End Property
	Public Property Dead() As Boolean
		Get
			Dead = bolDead
		End Get
		Set(ByVal Value As Boolean)
			bolDead = Value
		End Set
	End Property
	Public Property NaturalArmor() As Int16
		Get
			NaturalArmor = intNaturalArmor
		End Get
		Set(ByVal Value As Int16)
			intNaturalArmor = Value
		End Set
	End Property
	Public Property Sight() As Int16
		Get
			Sight = intSight
		End Get
		Set(ByVal Value As Int16)
			intSight = Value
		End Set
	End Property
	Public Property DarkVision() As Int16
		Get
			DarkVision = intDarkVision
		End Get
		Set(ByVal Value As Int16)
			intDarkVision = Value
		End Set
	End Property
	Public Property CharmResist() As Int16
		Get
			CharmResist = intCharmResist
		End Get
		Set(ByVal Value As Int16)
			intCharmResist = Value
		End Set
	End Property
	Public Property SleepResist() As Int16
		Get
			SleepResist = intSleepResist
		End Get
		Set(ByVal Value As Int16)
			intSleepResist = Value
		End Set
	End Property
	Public Property MagicResist() As Int16
		Get
			MagicResist = intMagicResist
		End Get
		Set(value As Int16)
			intMagicResist = value
		End Set
	End Property
	Public Property Poisoned() As Boolean
		Get
			Poisoned = bolPoisoned
		End Get
		Set(ByVal Value As Boolean)
			bolPoisoned = Value
		End Set
	End Property
	Public Property PoisonDuration() As Int16
		Get
			PoisonDuration = intPoisonDuration
		End Get
		Set(ByVal Value As Int16)
			intPoisonDuration = Value
		End Set
	End Property
	Public Property PoisonResist() As Int16
		Get
			PoisonResist = intPoisonResist
		End Get
		Set(ByVal value As Int16)
			intPoisonResist = value
		End Set
	End Property
	Public Property ColdResist() As Int16
		Get
			ColdResist = intColdResist
		End Get
		Set(ByVal value As Int16)
			intColdResist = value
		End Set
	End Property

	Public Property Confused() As Boolean
		Get
			Confused = bolConfused
		End Get
		Set(ByVal value As Boolean)
			bolConfused = value
		End Set
	End Property
	Public Property ConfusionDuration() As Int16
		Get
			ConfusionDuration = intConfusionDuration
		End Get
		Set(ByVal value As Int16)
			intConfusionDuration = value
		End Set
	End Property
	Public Property AutoWalk() As Boolean
		Get
			AutoWalk = bolAutoWalk
		End Get
		Set(ByVal value As Boolean)
			bolAutoWalk = value
		End Set
	End Property
	Public Property Sleeping() As Boolean
		Get
			Sleeping = bolSleeping
		End Get
		Set(ByVal value As Boolean)
			bolSleeping = value
		End Set
	End Property
	Public Property SleepDuration() As Int16
		Get
			SleepDuration = intSleepDuration
		End Get
		Set(ByVal value As Int16)
			intSleepDuration = value
		End Set
	End Property
	Public Property Invisible() As Boolean
		Get
			Return bolIsInvisible
		End Get
		Set(ByVal value As Boolean)
			bolIsInvisible = value
		End Set
	End Property
	Public Property InvisibilityDuration() As Int16
		Get
			InvisibilityDuration = intInvisibilityDuration
		End Get
		Set(ByVal Value As Int16)
			intInvisibilityDuration = Value
		End Set
	End Property
	Public Property EquippedWeight As Integer
		Get
			EquippedWeight = intEquippedWeight
		End Get
		Set(value As Integer)
			intEquippedWeight = value
		End Set
	End Property
	Public Property BackpackWeight As Integer
		Get
			BackpackWeight = intBackpackWeight
		End Get
		Set(value As Integer)
			intBackpackWeight = value
		End Set
	End Property


	Public Property GP() As Integer
		Get
			GP = intGP
		End Get
		Set(ByVal Value As Integer)
			intGP = Value
		End Set
	End Property

	' this is NOT the number of hitdice, but the sides of the die
	Public Property HitDie() As Integer
		Get
			HitDie = intHitDie
		End Get
		Set(ByVal Value As Integer)
			intHitDie = Value
		End Set
	End Property
	Public Property HP() As Integer
		Get
			HP = intHitPoints
		End Get
		Set(ByVal Value As Integer)
			intHitPoints = Value
		End Set
	End Property
	Public Property CurrentHP() As Integer
		Get
			CurrentHP = intCurrentHP
		End Get
		Set(ByVal Value As Integer)
			intCurrentHP = Value
		End Set
	End Property

	Public Property AC() As Integer
		Get
			AC = intAC
		End Get
		Set(ByVal Value As Integer)
			intAC = Value
		End Set
	End Property
	Public Property MiscACMod() As Integer
		Get
			MiscACMod = intMiscACMod
		End Get
		Set(ByVal Value As Integer)
			intMiscACMod = Value
		End Set
	End Property
	Public Property Initiative() As Integer
		Get
			Initiative = intInitiative
		End Get
		Set(ByVal Value As Integer)
			intInitiative = Value
		End Set
	End Property

	Public MustOverride ReadOnly Property TotalInitForRound() As Integer

	' meta data
	Public Property Icon() As String
		Get
			Icon = intAvatarIcon
		End Get
		Set(ByVal Value As String)
			intAvatarIcon = Value
		End Set
	End Property
	Public Property Color() As Integer
		Get
			Color = intAvatarColor
		End Get
		Set(ByVal Value As Integer)
			intAvatarColor = Value
		End Set
	End Property
	Public Property LocX() As Integer
		Get
			LocX = intX
		End Get
		Set(ByVal Value As Integer)
			intX = Value
		End Set
	End Property
	Public Property LocY() As Integer
		Get
			LocY = intY
		End Get
		Set(ByVal Value As Integer)
			intY = Value
		End Set
	End Property
	Public Property LocZ() As Integer
		Get
			LocZ = intZ
		End Get
		Set(ByVal Value As Integer)
			intZ = Value
		End Set
	End Property
	Public Property OverX() As Int16
		Get
			OverX = intOverX
		End Get
		Set(ByVal value As Int16)
			intOverX = value
		End Set
	End Property
	Public Property OverY() As Int16
		Get
			OverY = intOverY
		End Get
		Set(ByVal value As Int16)
			intOverY = value
		End Set
	End Property
	Public Property Overland() As Boolean
		Get
			Overland = bolOverland
		End Get
		Set(ByVal value As Boolean)
			bolOverland = value
		End Set
	End Property
	Public Property InTown() As Boolean
		Get
			InTown = bolInTown
		End Get
		Set(ByVal value As Boolean)
			bolInTown = value
		End Set
	End Property
	Public Property TerrainZoom() As Boolean
		Get
			TerrainZoom = bolTerrainZoom
		End Get
		Set(ByVal value As Boolean)
			bolTerrainZoom = value
		End Set
	End Property
	Public Property Town() As Town
		Get
			Town = intTown
		End Get
		Set(ByVal value As Town)
			intTown = value
		End Set
	End Property

	' methods
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
End Class

#End Region

