Imports System.Collections.Generic
Imports RND = Random.MersenneTwister
Imports CC = Microsoft.VisualBasic.ControlChars
Imports System.Collections.ObjectModel


Namespace DunGen4
	Public Class Builder

#Region " Class Level Variables "
		Private MaxWidth As Integer
		Private MaxHeight As Integer
		Private MaxDepth As Integer
		Private RoomsPerLevel As Integer
		Private RoomsPassed As Integer
		Private Level(,,) As GridCell
		Private RoomCount As Integer
		Private qLastRoom As New Queue
		Private rng As New RND

		' setting either stairschance to 100 should guarantee 
		' the appropriate stairs will be generated in the first room
		Private StairsUpChance As Integer = 100
		Private StairsDnChance As Integer = 15

		Private StairsUpRoomNo As Integer
		Private StairsDnRoomNo As Integer

		Private TotalTrapsThisLevel As Integer = 0
		Private TrapChance As Integer = 2
		Private SpecialRoomChance As Integer = 5

#End Region

#Region " High Level Routines "

		Public Function BuildDungeon() As GridCell(,,)

			Dim ctr As Integer
			Dim tries As Integer = 1
			Dim success As Boolean

			For ctr = 0 To MaxDepth - 1
				success = False
				Do While Not success
					Try
						Debug.WriteLine("BuildDungeon attempt #" & tries)
						success = BuildLevel(ctr)
					Catch ex As Exception
						Debug.WriteLine(ex.Message)
						success = False
					End Try

					If Not success Then
						ClearLevel()
						tries += 1
					End If
				Loop
			Next

			Return Level
		End Function

		Private Sub ClearLevel()
			Dim intx As Integer, inty As Integer, intz As Integer
			For intx = 0 To MaxWidth
				For inty = 0 To MaxHeight
					For intz = 0 To MaxDepth - 1
						Level(intx, inty, intz).FloorType = SquareType.Rock
					Next
				Next
			Next

			RoomCount = 0

		End Sub

		Private Function BuildLevel(ByVal Z As Integer) As Boolean
			Debug.WriteLine("Entering BuildLevel routine")

			Dim loops As Integer = 1, _
			 success As Boolean, _
			 qTargetRoom As New Queue

			' try and change it up a bit. instead of a strict number of rooms, 
			' let the max number of rooms fluctuate a little. (+- 1)
			RoomsPerLevel = rng.Next(RoomsPassed - 1, RoomsPassed + 1)


			Return True
		End Function

		Private Function BuildRoom(ByVal Z As Integer) As Boolean


			Return True
		End Function





#End Region


#Region " Instantiation Code "

		Public Sub New(ByVal Width As Integer, ByVal Height As Integer, ByVal Depth As Integer, ByVal Rooms As Integer)

			'Re-seed the RNG
			Randomize()

			' set up the dimensions of the grid when we instantiate the object
			MaxWidth = Width
			MaxHeight = Height
			MaxDepth = Depth
			RoomsPassed = Rooms

			' create the array, using the GridCell structure to fill it
			ReDim Level(MaxWidth, MaxHeight, MaxDepth)

			' initialize the entire 3D grid as one solid block
			Dim intXCtr As Integer, _
			 intYCtr As Integer, _
			 intZCtr As Integer
			For intZCtr = 0 To MaxDepth
				For intYCtr = 1 To MaxHeight
					For intXCtr = 0 To MaxWidth
						With Level(intXCtr, intYCtr, intZCtr)
							.FloorType = SquareType.Rock
							.items = New ArrayList()
						End With
					Next
				Next
			Next

		End Sub

#End Region


	End Class

#Region " DungeonBuilder Structures and Enums "

	Public Enum Direction
		Up = 1
		Down = 2
	End Enum

	Public Enum Heading
		North = 1
		NorthEast = 2	'8 1 2
		East = 3		'7 x 3
		SouthEast = 4	'6 5 4
		South = 5
		SouthWest = 6
		West = 7
		NorthWest = 8
	End Enum

	Public Enum RoomSize
		Tiny = 0
		Small = 1
		Medium = 2
		Large = 3
		Huge = 4
	End Enum

	Public Enum RoomShape
		Square = 0
		Rectangle = 1
		UpLeft = 2
		UpRight = 3
		DownLeft = 4
		DownRight = 5
		Circle = 6 ' not implemented yet
	End Enum

	Public Enum TrapTypes
		None = 0
		sleep = 1
		poison = 2
		explosion = 3
		pit = 4
		snake = 5
		rock = 6
		confusion = 7
		teleport = 8
	End Enum

	Public Enum SpecialTypes
		None = 0
	End Enum
	
	Public Enum Status
		Normal = 0
		Blessed = 1
		Cursed = 2
	End Enum

	Public Enum SquareType
		Rock = 0		'
		Wall = 1		'#
		Floor = 2		'.
		Door = 3		'+
		Secret = 4		'#
		OpenDoor = 5	'/
		StairsUp = 6	'<
		StairsDn = 7	'>
		Trap = 8		'^
		NWCorner = 9	'#
		NECorner = 10	'#
		SECorner = 11	'#
		SWCorner = 12	'#

		' ADDED in v4
		Altar = 13		'_
		Pool = 14		'0
		Water = 15		'= BLUE
		Ice = 16		'= WHITE
	End Enum

	Public Enum IlluminationStrength
		Dark = 0
		Torch = 1
		Normal = 2
		Bright = 3
		Daylight = 4
		Cloudy = 5
		Gloomy = 6
		Magical = 7
		Bioluminescent = 8
	End Enum

	Public Structure coord
		Dim x As Integer
		Dim y As Integer
	End Structure

	Public Structure GridCell
		Dim location As coord
		' location.x = horizontal coordinate (starting at 0)
		' location.y = vertical coordinate (starting at 0)
		Dim FloorType As SquareType
		Dim Observed As Boolean
		Dim Illumination As Integer
		Dim Monster As Integer
		Dim itemcount As Integer
		Dim items As ArrayList
		Dim TrapType As TrapTypes
		Dim TrapDiscovered As Boolean
		Dim SpecialRoom As Boolean
		Dim SpecialType As SpecialTypes

	End Structure

#End Region

End Namespace