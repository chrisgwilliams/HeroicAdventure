Imports HA.Common

' Avatar is the base class for all PCs, NPCs and Monsters
<DebuggerStepThrough()> Public MustInherit Class Avatar
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

    Private intStrMods As Integer
	Private intIntMods As Integer
	Private intWisMods As Integer
	Private intDexMods As Integer
	Private intConMods As Integer
	Private intChaMods As Integer

	Public Property StrMods() As Integer
    Public Property IntMods() As Integer
    Public Property WisMods() As Integer
    Public Property DexMods() As Integer
    Public Property ConMods() As Integer
    Public Property ChaMods() As Integer

#End Region

    Public Property Gender() As Gender
    Public Property HasHands() As Boolean
    Public Property HasFeet() As Boolean
    Public Property Dead() As Boolean
    Public Property NaturalArmor() As Int16
    Public Property Sight() As Int16
    Public Property DarkVision() As Int16
    Public Property CharmResist() As Int16
    Public Property SleepResist() As Int16
    Public Property MagicResist() As Int16
    Public Property Poisoned() As Boolean
    Public Property PoisonDuration() As Int16
    Public Property PoisonResist() As Int16
    Public Property ColdResist() As Int16
    Public Property Confused() As Boolean
    Public Property ConfusionDuration() As Int16
    Public Property AutoWalk() As Boolean
    Public Property Sleeping() As Boolean
    Public Property SleepDuration() As Int16
    Public Property Invisible() As Boolean
    Public Property InvisibilityDuration() As Int16
    Public Property EquippedWeight As Integer
    Public Property BackpackWeight As Integer

    Public Property GP() As Integer

    ' this is NOT the number of hitdice, but the sides of the die
    Public Property HitDie() As Integer
    Public Property HP() As Integer
    Public Property CurrentHP() As Integer

    Public Property AC() As Integer
    Public Property MiscACMod() As Integer
    Public Property Initiative() As Integer

    Public MustOverride ReadOnly Property TotalInitForRound() As Integer

	' meta data
	Public Property Icon() As String
    Public Property Color() As ConsoleColor
    Public Property LocX() As Integer
    Public Property LocY() As Integer
    Public Property LocZ() As Integer
    Public Property OverX() As Int16
    Public Property OverY() As Int16
    Public Property Overland() As Boolean
    Public Property InTown() As Boolean
    Public Property TerrainZoom() As Boolean
    Public Property Town() As Town

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


