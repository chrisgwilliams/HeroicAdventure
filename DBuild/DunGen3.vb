Imports RND = Random.MersenneTwister

Namespace DunGen3

	Public Class Builder

#Region " Class Level Variables "
		Private m_MaxWidth As Integer
		Private m_MaxHeight As Integer
		Private m_MaxDepth As Integer
		Private m_RoomsPerLevel As Integer
		Private m_RoomsPassed As Integer
		Private m_Level(,,) As GridCell
		Private m_RoomCount As Integer
		Private m_qLastRoom As New Queue
		Private rng As New RND

		' setting either stairschance to 100 should guarantee 
		' the appropriate stairs will be generated in the first room
		Private m_StairsUpChance As Integer = 100
		Private m_StairsDnChance As Integer = 15

		Private m_StairsUpRoomNo As Integer
		Private m_StairsDnRoomNo As Integer

		Private m_TotalTrapsThisLevel As Integer = 0
		Private m_TrapChance As Integer = 2
		Private m_SpecialRoomChance As Integer = 5
#End Region

#Region " High Level Routines "

		Public Function BuildDungeon() As Array

			Dim intCtr As Integer, _
			 success As Boolean, _
			 intTries As Integer = 1

			For intCtr = 0 To m_MaxDepth - 1
				success = False
				Do While Not success
					Try
						Debug.WriteLine("BuildDungeon attempt #" & intTries)
						success = BuildLevel(intCtr)
					Catch ex As Exception
						Debug.WriteLine(ex.Message)
						success = False
					End Try

					If Not success Then
						Dim intx As Integer, inty As Integer, intz As Integer
						For intx = 0 To m_MaxWidth
							For inty = 0 To m_MaxHeight
								For intz = 0 To m_MaxDepth - 1
									m_Level(intx, inty, intz).FloorType = SquareType.Rock
								Next
							Next
						Next

						m_RoomCount = 0
						intTries += 1
					End If
				Loop
			Next

			' pass back the dungeon levels
			Return m_Level

		End Function

		Private Function BuildLevel(ByVal Z As Integer) As Boolean
			Debug.WriteLine("Entering BuildLevel routine")

			Dim loops As Integer = 1, _
			 success As Boolean, _
			 qTargetRoom As New Queue

			' try and change it up a bit. instead of a strict number of rooms, 
			' let the max number of rooms fluctuate a little. (+- 1)
			m_RoomsPerLevel = rng.Next(m_RoomsPassed - 1, m_RoomsPassed + 1)

			' added the "Or loops = 21" part so it won't keep trying
			' to put a room where theres no possible space left
			Do Until (m_RoomCount >= m_RoomsPerLevel) Or (loops = 21)

				' once we have at least one room, put the coords into qTargetRoom
				If m_RoomCount > 0 Then
					qTargetRoom = m_qLastRoom.Clone
				End If

				' Build the room
				Try
					Debug.WriteLine("Calling BuildRoom attempt #" & loops)
					success = BuildRoom(Z)
				Catch ex As System.Exception
					Debug.WriteLine("BuildRoom: " & ex.Message)
					Throw New System.Exception("BuildRoom: " & ex.Message)
				End Try

				' dont make a passage until we have at least 2 rooms
				' and the previous attempt to build a room must have been successful
				If m_RoomCount > 1 And success Then
					Try
						Debug.WriteLine("Calling BuildPassage")
						success = BuildPassage(Z, qTargetRoom)
					Catch ex As System.Exception
						Debug.WriteLine("BuildPassage: " & ex.Message)
						Throw New System.Exception("BuildPassage: " & ex.Message)
					End Try
				End If
				loops += 1
			Loop

			If loops >= 21 Then
				Return False
			Else
				' here's the map scrubber. we're passing in the level to be cleaned
				' keep scrubbing until no changes are made.
				Debug.WriteLine("Starting Map Scrubber")
				Dim FixCtr As Integer = 1
				Do Until FixCtr = 0
					FixCtr = CleanupMap(Z)
					If FixCtr > 0 Then
						Debug.WriteLine(FixCtr.ToString & " changes made. Restarting scrubber.")
					Else
						Debug.WriteLine("No more changes.")
					End If
				Loop
				Debug.WriteLine("Finished Map Scrubber")

				' create an arraylist full of door locations (coord)
				Dim Doors() As coord
				Doors = FindDoors(Z)
				Debug.WriteLine(Doors.Length & " doors generated.")

				' take the list and make a few of them into secret doors
				' this was Doors\5 but we were only getting one secretdoor per level on average
				Dim maxSecret As Int16 = Doors.GetLength(0) \ 3
				Dim ActualSecret As Int16 = rng.Next(0, maxSecret)
				Debug.WriteLine(ActualSecret & " doors will be converted to secret.")

				If ActualSecret > 0 Then
					Dim intCtr As Int16
					For intCtr = 1 To ActualSecret
						Dim DoortoSecret As Int16 = rng.Next(0, Doors.GetLength(0) - 1)
						m_Level(Doors(DoortoSecret).x, Doors(DoortoSecret).y, Z).FloorType = SquareType.Secret
						Debug.WriteLine("Door at x:" & Doors(DoortoSecret).x & ", y:" & Doors(DoortoSecret).y & " converted.")
					Next
				End If

				' show room numbers for up/dn stairs
				Debug.WriteLine("Up stairs room #" & m_StairsUpRoomNo)
				Debug.WriteLine("Dn stairs room #" & m_StairsDnRoomNo)

				Debug.WriteLine("Leaving BuildLevel routine")
				Return success
			End If
		End Function

		Private Function BuildRoom(ByVal Z As Integer) As Boolean
			Dim intCtr As Integer, Attempt As Integer, _
			 shape As Integer, size As Integer, _
			 width As Integer, height As Integer, _
			 StartX As Integer, StartY As Integer, _
			 Blip As Boolean, _
			 q As New Queue, qTemp As New Queue, _
			 qTmpCnt As Integer, XY As coord, _
			 DarkRoomChance As Integer = 7, _
			 RoomLit As Boolean, SpecialRoom As Boolean, _
			 StairsPlaced As Boolean, RoomTrapped As Boolean

			Do While Attempt <= 15
				' randomly set the starting point (X,Y) for the room
				StartX = rng.Next(3, (m_MaxWidth * 0.9))
				StartY = rng.Next(4, (m_MaxHeight * 0.8)) 'was 3, but needed extra row for messages
				' randomly set the size & shape of the room
				SetSizeShape(size, shape)
				' pass in the size and shape and get the width and height
				SetWidthHeight(size, shape, width, height)
				' map out the coordinates, WITHOUT WALLS
				EnqueueRoomCells(q, StartX, StartY, width, height, shape, False)
				' check each square of the room against the map for overlaps
				Blip = CheckForOverlap(q, Z)
				' check to see if room is dark or lit
				RoomLit = True
				If rng.Next(1, 100) < DarkRoomChance Then RoomLit = False

				' if theres no blip (overlap), then jump out of the loop
				If Not Blip Then Exit Do
				'
				' if we get this far, the current room overlaps with a room 
				' already placed on the map, so try again with new dimensions
				Attempt += 1
			Loop

			' if you made it out of the loop with a blip, then you must have
			' exceeded all 15 tries, so go ahead and return a FALSE to BuildLevel
			If Blip Then Return False

			' check for chance of special room (5% chance)
			If rng.Next(1, 100) <= m_SpecialRoomChance Then
				SpecialRoom = True
			Else
				SpecialRoom = False
			End If

			' map out the room coordinates, WITH WALLS
			EnqueueRoomCells(q, StartX, StartY, width, height, shape, True)
			qTmpCnt = q.Count

			' at this point, we didn't get a blip (overlap) on the main map
			' and you didn't exceed 10 attempts, so go ahead and dig it out
			For intCtr = 1 To qTmpCnt
				XY = q.Dequeue
				If XY.x < StartX - 1 _
				Or XY.y < StartY - 1 _
				Or XY.x = StartX + width + 1 _
				Or XY.y = StartY + height + 1 _
				Or ((shape = RoomShape.DownRight) _
				 And (XY.x = StartX + width / 2 + 1) _
				 And (XY.y < StartY + height / 2 + 1)) _
				Or ((shape = RoomShape.UpRight) _
				 And (XY.x > StartX + width / 2) _
				 And (XY.y > StartY + height / 2)) _
				Or ((shape = RoomShape.UpLeft) _
				 And (XY.x <= StartX + width / 2) _
				 And (XY.y > StartY + height / 2)) _
				Or m_Level(XY.x - 1, XY.y, Z).FloorType = SquareType.Rock _
				Or m_Level(XY.x, XY.y - 1, Z).FloorType = SquareType.Rock _
				Or m_Level(XY.x - 1, XY.y - 1, Z).FloorType = SquareType.Rock _
				Then
					' dig a wall square
					Digger(XY.x, XY.y, Z, True, 1)
				Else
					' dig a floor square
					Digger(XY.x, XY.y, Z, RoomLit)

					' if this room is special, flag each floor tile as special
					If SpecialRoom Then m_Level(XY.x, XY.y, Z).SpecialRoom = True

					' place up and down stairs, if appropriate
					StairsPlaced = False
					If m_StairsUpChance > 0 Or m_StairsDnChance > 0 Then
						StairsPlaced = CheckForStairs(XY.x, XY.y, Z)
					End If

					' if we didn't place any stairs on this tile, check for chance of traps
					If (Not StairsPlaced) _
					And (rng.Next(1, 100) <= m_TrapChance) _
					And (m_TotalTrapsThisLevel <= m_RoomsPerLevel + rng.Next(-3, 3)) Then
						' stick a trap here
						m_Level(XY.x, XY.y, Z).TrapType = rng.Next(1, 8)
						m_TotalTrapsThisLevel += 1
						RoomTrapped = True
						' we can pull traptype on the other side and handle however we want
					End If
				End If
			Next

			' once a room has been dug out, store the four corners in a queue
			EnqueueLastRoom(StartX, StartY, width, height)

			' if we didn't place stairs, increase the chance for the next room
			If m_StairsUpChance > 0 Then
				m_StairsUpChance += (100 / m_RoomsPerLevel)
			End If
			If m_StairsDnChance > 0 Then
				m_StairsDnChance += (100 / m_RoomsPerLevel)
			End If

			' do some cleanup
			q = Nothing
			qTemp = Nothing

			' output some debug info
			m_RoomCount += 1
			Debug.WriteLine("Room Count: " & m_RoomCount)
			If SpecialRoom Then Debug.WriteLine("Room #" & m_RoomCount & " is special!")
			If RoomTrapped Then Debug.WriteLine("Room #" & m_RoomCount & " is trapped!")
			ShapeSizeDebug(shape, size)

			' and then return TRUE to the BuildLevel routine
			Return True

		End Function

		Private Function BuildPassage(ByVal Z As Integer, _
				 ByRef qTargetRoom As Queue) As Boolean

			' the intent here is to connect the room just made to the
			' room just previously made. if you run into another room
			' or passage along the way, just connect to it and stop.
			' this SHOULD ensure that every room is accessible

			Dim intCtr As Integer, _
			 wall As Integer, wall2 As Integer, TurningPoint As Integer, _
			 startpos As coord, CurrentXY(3) As coord, DestXY(3) As coord, _
			 qTempLastRoom As New Queue, collision As Boolean, RoomLit As Boolean

			qTempLastRoom = m_qLastRoom.Clone

			For intCtr = 0 To 3
				CurrentXY(intCtr) = qTempLastRoom.Dequeue
				DestXY(intCtr) = qTargetRoom.Dequeue
			Next

			' passages are always lit, so set this to true
			RoomLit = True

			' figure out which wall to start from
			' wall 2 is the direction we'll go after a turn
			PickWall(wall, wall2, CurrentXY, DestXY)

			' ok now pick a spot on the wall we just got
			SetStartPos(wall, startpos, CurrentXY, DestXY, Z)

			' now set a turning point for the passageway
			TurningPoint = SetTurningPoint(wall, DestXY)

			' dig out the door first (wall is passed to fix lighting around doors)
			PlaceDoor(startpos.x, startpos.y, Z, RoomLit, wall)

			' then start excavating the passageway
			intCtr = 0
			Do While Not collision
				intCtr += 1
				Select Case wall
					Case 1 ' heading north
						'
						' #...   
						'##...  if a Northbound passage hits the SW
						'#....  corner of a room, make a suitable
						'#.###  entrance instead.
						'#.#
						'#.#
						' 
						If m_Level(startpos.x + 1, startpos.y - intCtr, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x, startpos.y - intCtr, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x - 1, startpos.y - intCtr, Z).FloorType = SquareType.Rock Then
							Digger(startpos.x, (startpos.y - intCtr) + 1, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x, (startpos.y - intCtr), Z, RoomLit, SquareType.Floor)
							Digger(startpos.x - 1, (startpos.y - intCtr), Z, RoomLit, SquareType.Wall)
							Digger(startpos.x - 1, (startpos.y - intCtr) - 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						'
						' ...#   
						' ...##  if a Northbound passage hits the SE
						' ....#  corner of a room, make a suitable
						' ###.#  entrance instead.
						'   #.#
						'   #.#
						' 
						If m_Level(startpos.x - 1, startpos.y - intCtr, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x, startpos.y - intCtr, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x + 1, startpos.y - intCtr, Z).FloorType = SquareType.Rock _
						Then
							Digger(startpos.x, (startpos.y - intCtr) + 1, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x, (startpos.y - intCtr), Z, RoomLit, SquareType.Floor)
							Digger(startpos.x + 1, (startpos.y - intCtr), Z, RoomLit, SquareType.Wall)
							Digger(startpos.x + 1, (startpos.y - intCtr) - 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						' If we hit a floor square (instead of rock or wall), it's time to stop
						If m_Level(startpos.x, startpos.y - intCtr, Z).FloorType = SquareType.Floor Then
							collision = True
							Exit Select
						End If

						' if we get too close (within 4 lines) to the top of the screen, set a new turning point
						If startpos.y - intCtr <= 4 Then
							TurningPoint = startpos.y - intCtr
						End If

						If m_Level(startpos.x, startpos.y - intCtr, Z).FloorType = (SquareType.Wall _
						   Or SquareType.Floor) Then
							wall = 0
							collision = True
						End If

						' if you run into a door while digging out a passage, make sure and keep the door
						If m_Level(startpos.x - 1, startpos.y - intCtr, Z).FloorType = SquareType.Door Then
							Digger(startpos.x - 1, startpos.y - intCtr, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x - 1, startpos.y - intCtr, Z, RoomLit, SquareType.Wall)
						End If
						Digger(startpos.x, startpos.y - intCtr, Z, SquareType.Floor)
						If m_Level(startpos.x + 1, startpos.y - intCtr, Z).FloorType = SquareType.Door Then
							Digger(startpos.x + 1, startpos.y - intCtr, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x + 1, startpos.y - intCtr, Z, RoomLit, SquareType.Wall)
						End If

						' this changes the direction of a corridor once we enter the vertical range 
						' of the destination room.  once the Y of our northbound hallway is = to the 
						' turning point make a turn towards destxy
						If startpos.y - intCtr = TurningPoint Then
							FixCorner(wall, wall2, startpos.x, startpos.y - intCtr, Z)
							wall = wall2
							intCtr = 0
							startpos.y = TurningPoint
							TurningPoint = 0
						End If


					Case 2 ' east wall
						' if an Eastbound passage hits the bottom left corner of a room,
						' make a suitable entrance
						If m_Level(startpos.x + intCtr, startpos.y - 1, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x + intCtr, startpos.y, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x + intCtr, startpos.y + 1, Z).FloorType = SquareType.Rock Then
							Digger((startpos.x + intCtr) - 1, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x + intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x + intCtr, startpos.y + 1, Z, RoomLit, SquareType.Wall)
							Digger(startpos.x + intCtr + 1, startpos.y + 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						' if an Eastbound passage hits the top left corner of a room, 
						' make a suitable entrance... nice and neat.
						If m_Level(startpos.x + intCtr, startpos.y + 1, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x + intCtr, startpos.y, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x + intCtr, startpos.y - 1, Z).FloorType = SquareType.Rock Then
							Digger(startpos.x + intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger((startpos.x + intCtr) - 1, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x + intCtr, startpos.y - 1, Z, RoomLit, SquareType.Wall)
							Digger(startpos.x + intCtr + 1, startpos.y - 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						If m_Level(startpos.x + intCtr, startpos.y, Z).FloorType = SquareType.Floor Then
							collision = True
							Exit Select
						End If

						If startpos.x + intCtr >= m_MaxWidth - 4 Then
							TurningPoint = startpos.x + intCtr
						End If

						If m_Level(startpos.x + intCtr, startpos.y, Z).FloorType = (SquareType.Wall _
						   Or SquareType.Floor) Then
							wall = 0
							collision = True
						End If

						' this block is to prevent passage walls from overwriting a door
						If m_Level(startpos.x + intCtr, startpos.y - 1, Z).FloorType = SquareType.Door Then
							Digger(startpos.x + intCtr, startpos.y - 1, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x + intCtr, startpos.y - 1, Z, RoomLit, SquareType.Wall)
						End If
						Digger(startpos.x + intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
						If m_Level(startpos.x + intCtr, startpos.y + 1, Z).FloorType = SquareType.Door Then
							Digger(startpos.x + intCtr, startpos.y + 1, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x + intCtr, startpos.y + 1, Z, RoomLit, SquareType.Wall)
						End If

						' this changes the direction of a corridor once we enter the vertical range 
						' of the destination room.  once the Y of our northbound hallway is = to the 
						' turning point make a turn towards destxy
						If startpos.x + intCtr = TurningPoint Then
							FixCorner(wall, wall2, startpos.x + intCtr, startpos.y, Z)
							wall = wall2
							intCtr = 0
							startpos.x = TurningPoint
							TurningPoint = 0
						End If


					Case 3 ' south wall
						' if a Southbound passage hits the top left corner of a room,
						' make a suitable entrance
						If m_Level(startpos.x + 1, startpos.y + intCtr, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x, startpos.y + intCtr, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x - 1, startpos.y + intCtr, Z).FloorType = SquareType.Rock Then
							Digger(startpos.x, (startpos.y + intCtr) - 1, Z, SquareType.Floor)
							Digger(startpos.x, (startpos.y + intCtr), Z, SquareType.Floor)
							Digger(startpos.x - 1, (startpos.y + intCtr), Z, SquareType.Wall)
							Digger(startpos.x - 1, (startpos.y + intCtr) + 1, Z, SquareType.Wall)
							collision = True
							Exit Select
						End If

						' if a Southbound passage hits the top right corner of a room,
						' make a suitable entrance
						If m_Level(startpos.x - 1, startpos.y - intCtr, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x, startpos.y - intCtr, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x + 1, startpos.y - intCtr, Z).FloorType = SquareType.Rock _
						Then
							Digger(startpos.x, (startpos.y - intCtr) + 1, Z, RoomLit, SquareType.Floor)
							Digger(startpos.x, (startpos.y - intCtr), Z, RoomLit, SquareType.Floor)
							Digger(startpos.x + 1, (startpos.y - intCtr), Z, RoomLit, SquareType.Wall)
							Digger(startpos.x + 1, (startpos.y - intCtr) - 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						If m_Level(startpos.x, startpos.y + intCtr, Z).FloorType = SquareType.Floor Then
							collision = True
							Exit Select
						End If

						If startpos.y + intCtr >= m_MaxHeight - 4 Then
							TurningPoint = startpos.y + intCtr
						End If

						If m_Level(startpos.x, startpos.y + intCtr, Z).FloorType = (SquareType.Wall _
						 Or SquareType.Floor) Then
							wall = 0
							collision = True
						End If

						' this block is to prevent passage walls from overwriting a door
						If m_Level(startpos.x - 1, startpos.y + intCtr, Z).FloorType = SquareType.Door Then
							Digger(startpos.x - 1, startpos.y + intCtr, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x - 1, startpos.y + intCtr, Z, RoomLit, SquareType.Wall)
						End If
						Digger(startpos.x, startpos.y + intCtr, Z, RoomLit, SquareType.Floor)
						If m_Level(startpos.x + 1, startpos.y + intCtr, Z).FloorType = SquareType.Door Then
							Digger(startpos.x + 1, startpos.y + intCtr, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x + 1, startpos.y + intCtr, Z, RoomLit, SquareType.Wall)
						End If

						' this changes the direction of a corridor once we enter the vertical range 
						' of the destination room.  once the Y of our southbound hallway is = to the 
						' turning point make a turn towards destxy
						If startpos.y + intCtr = TurningPoint Then
							FixCorner(wall, wall2, startpos.x, startpos.y + intCtr, Z)
							wall = wall2
							intCtr = 0
							startpos.y = TurningPoint
							TurningPoint = 0
						End If


					Case 4 ' west wall
						' if an Westbound passage hits the bottom right corner of a room,
						' make a suitable entrance
						If m_Level(startpos.x - intCtr, startpos.y - 1, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x - intCtr, startpos.y, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x - intCtr, startpos.y + 1, Z).FloorType = SquareType.Rock Then
							Digger(startpos.x - intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger((startpos.x - intCtr) + 1, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger((startpos.x - intCtr) - 1, startpos.y + 1, Z, RoomLit, SquareType.Wall)
							Digger(startpos.x - intCtr, startpos.y + 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						' if an Westbound passage hits the top right corner of a room, 
						' make a suitable entrance... nice and neat.
						If m_Level(startpos.x - intCtr, startpos.y + 1, Z).FloorType = SquareType.Floor _
						And m_Level(startpos.x - intCtr, startpos.y, Z).FloorType = SquareType.Wall _
						And m_Level(startpos.x - intCtr, startpos.y - 1, Z).FloorType = SquareType.Rock Then
							Digger(startpos.x - intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger((startpos.x - intCtr) + 1, startpos.y, Z, RoomLit, SquareType.Floor)
							Digger((startpos.x - intCtr) - 1, startpos.y - 1, Z, RoomLit, SquareType.Wall)
							Digger(startpos.x - intCtr, startpos.y - 1, Z, RoomLit, SquareType.Wall)
							collision = True
							Exit Select
						End If

						If m_Level(startpos.x - intCtr, startpos.y, Z).FloorType = SquareType.Floor Then
							collision = True
							Exit Select
						End If

						If startpos.x - intCtr <= 4 Then
							TurningPoint = startpos.x - intCtr
						End If

						If m_Level(startpos.x - intCtr, startpos.y, Z).FloorType = (SquareType.Wall _
						   Or SquareType.Floor) Then
							wall = 0
							collision = True
						End If

						' this block is to prevent passage walls from overwriting a door
						If m_Level(startpos.x - intCtr, startpos.y - 1, Z).FloorType = SquareType.Door Then
							Digger(startpos.x - intCtr, startpos.y - 1, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x - intCtr, startpos.y - 1, Z, RoomLit, SquareType.Wall)
						End If
						Digger(startpos.x - intCtr, startpos.y, Z, RoomLit, SquareType.Floor)
						If m_Level(startpos.x - intCtr, startpos.y + 1, Z).FloorType = SquareType.Door Then
							Digger(startpos.x - intCtr, startpos.y + 1, Z, RoomLit, SquareType.Door)
						Else
							Digger(startpos.x - intCtr, startpos.y + 1, Z, RoomLit, SquareType.Wall)
						End If

						' this changes the direction of a corridor once we enter the vertical range 
						' of the destination room.  once the Y of our northbound hallway is = to the 
						' turning point make a turn towards destxy
						If startpos.x - intCtr = TurningPoint Then
							FixCorner(wall, wall2, startpos.x - intCtr, startpos.y, Z)
							wall = wall2
							intCtr = 0
							startpos.x = TurningPoint
							TurningPoint = 0
						End If

					Case Else
						collision = True
						Exit Select
				End Select
			Loop

			' if we hit a wall, then our passage stops, so we consider this a success
			Return True

		End Function

		Private Function FindDoors(ByVal intZ As Int16) As Array
			Dim intX As Int16, intY As Int16
			Dim alDoors As New ArrayList

			For intX = 0 To m_MaxWidth - 1
				For intY = 0 To m_MaxHeight - 1
					If m_Level(intX, intY, intZ).FloorType = SquareType.Door Then
						Dim door As coord
						door.x = intX
						door.y = intY
						alDoors.Add(door)
					End If
				Next
			Next

			Return alDoors.ToArray(GetType(coord))
		End Function

		Private Function CleanupMap(ByVal intZ As Integer) As Integer
			Dim intX As Integer, intY As Integer, RoomLit As Boolean

			' increment change count with any changes made, 
			' we will rerun cleanup until it returns 0 changes
			Dim changes As Integer = 0

			For intX = 0 To m_MaxWidth - 1
				For intY = 0 To m_MaxHeight - 1

					' set the observed flag to false when generating the level, this will
					' cause the level to appear gradually as the character discovers it.
					m_Level(intX, intY, intZ).Observed = False

					' this determines if a given square is lit or not
					If m_Level(intX, intY, intZ).Illumination > IlluminationStrength.Dark Then RoomLit = True

					Select Case m_Level(intX, intY, intZ).FloorType

						Case SquareType.Wall
							' check for horizontal double walls ( >12 wide) 
							' with no doors nearby and add a large opening
							If intY < m_MaxHeight - 4 And intX < m_MaxHeight - 8 And intX > 8 Then
								If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 2, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 3, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 4, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 5, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 6, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 7, intY, intZ).FloorType <> SquareType.Floor _
								And m_Level(intX - 8, intY, intZ).FloorType <> SquareType.Floor _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 4, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 5, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 6, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 2, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 3, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 4, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 5, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 6, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 7, intY + 1, intZ).FloorType <> SquareType.Floor _
								And m_Level(intX - 8, intY + 1, intZ).FloorType <> SquareType.Floor _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 4, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 5, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 6, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX - 1, intY, intZ, RoomLit, SquareType.Floor)
									Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
									Digger(intX - 1, intY + 1, intZ, RoomLit, SquareType.Floor)
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Floor)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' southbound dead end corridor hits room, wall but no door
							If intY < m_MaxHeight - 3 And intY > 4 And intX < m_MaxWidth - 2 And intX > 0 Then
								If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY - 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 2, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' hallway nests in crook of L shape room... add passage
							If intY < m_MaxHeight - 6 And intY > 2 And intX < m_MaxWidth - 6 And intX > 2 Then
								If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 3, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 3, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY + 3, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY + 3, intZ).FloorType = SquareType.Wall _
								Then
									If m_Level(intX + 1, intY + 4, intZ).FloorType = SquareType.Floor _
									And m_Level(intX + 4, intY + 1, intZ).FloorType = SquareType.Floor Then
										Dim intRandom As Integer
										intRandom = rng.Next(1, 2)
										If intRandom = 1 Then
											Digger(intX + 2, intY + 1, intZ, RoomLit, SquareType.Floor)
											Digger(intX + 3, intY + 1, intZ, RoomLit, SquareType.Floor)

											changes += 1
										Else
											Digger(intX + 1, intY + 2, intZ, RoomLit, SquareType.Floor)
											Digger(intX + 1, intY + 3, intZ, RoomLit, SquareType.Floor)

											changes += 1
										End If
									ElseIf m_Level(intX + 1, intY + 4, intZ).FloorType = SquareType.Floor Then
										Digger(intX + 1, intY + 2, intZ, RoomLit, SquareType.Floor)
										Digger(intX + 1, intY + 3, intZ, RoomLit, SquareType.Floor)

										changes += 1
									ElseIf m_Level(intX + 4, intY + 1, intZ).FloorType = SquareType.Floor Then
										Digger(intX + 2, intY + 1, intZ, RoomLit, SquareType.Floor)
										Digger(intX + 3, intY + 1, intZ, RoomLit, SquareType.Floor)

										changes += 1
									End If
								End If
							End If
							' ----------------------------------------------------------------

							' Convert any NW corner walls to NW corner squaretype
							If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
							And ((m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor) _
							 Or _
							 (m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Rock)) Then
								Digger(intX, intY, intZ, RoomLit, SquareType.NWCorner)
								changes += 1
							End If
							' ----------------------------------------------------------------

							' Convert any NE corner walls to NE corner squaretype
							If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
							And ((m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor) _
							 Or _
							 (m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Rock)) Then
								Digger(intX, intY, intZ, RoomLit, SquareType.NECorner)
								changes += 1
							End If
							' ----------------------------------------------------------------

							' Convert any SE corner walls to SE corner squaretype
							If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
							And ((m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor) _
							 Or _
							 (m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Rock)) Then
								Digger(intX, intY, intZ, RoomLit, SquareType.SECorner)
								changes += 1
							End If
							' ----------------------------------------------------------------

							' Convert any SW corner walls to SW corner squaretype
							If m_Level(intX, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
							And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
							And ((m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor) _
							 Or _
							 (m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Rock)) Then
								Digger(intX, intY, intZ, RoomLit, SquareType.SWCorner)
								changes += 1
							End If
							' ----------------------------------------------------------------

						Case SquareType.Floor
							' check all adjacent floor tiles for gaps in walls. We should never 
							' EVER have a floor tile immediately adjacent to exposed rock (non wall)
							If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX - 1, intY - 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							If m_Level(intX - 1, intY, intZ).FloorType = SquareType.Rock Then
								Digger(intX - 1, intY, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							If m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX - 1, intY + 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If

							If m_Level(intX, intY - 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX, intY - 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							If m_Level(intX, intY + 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX, intY + 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If

							If m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX + 1, intY - 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							If m_Level(intX + 1, intY, intZ).FloorType = SquareType.Rock Then
								Digger(intX + 1, intY, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							If m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Rock Then
								Digger(intX + 1, intY + 1, intZ, RoomLit, SquareType.Wall)
								changes += 1
							End If
							' ----------------------------------------------------------------

							If intY < 17 And intX > 3 And intX < 57 Then
								' dead end corridor, double thick, with room on other side
								If m_Level(intX, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 3, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 2, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Floor)
									Digger(intX + 1, intY, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							If intY < 17 And intX > 3 And intX < 57 Then
								' dead end hall with room on other side of wall, but no door
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall Then
									Digger(intX, intY + 2, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' horiz - double wall between room and hallway fix
							If intY < 15 And intX > 3 And intX < 57 Then
								If m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Rock _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY + 1, intZ).FloorType = SquareType.Rock _
								And m_Level(intX - 1, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 3, intZ).FloorType = SquareType.Floor Then
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Floor)
									Digger(intX, intY + 2, intZ, RoomLit, SquareType.Floor)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' vertical hallway adjacent to room with double wall between them
							If intY > 2 And intY < 19 And intX > 2 And intX < 57 Then
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 3, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX + 1, intY, intZ, RoomLit, SquareType.Floor)
									Digger(intX + 2, intY, intZ, RoomLit, SquareType.Floor)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' horizontal hallway below room with double wall between them
							If intY > 4 And intY < 19 And intX > 2 And intX < 57 Then
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 2, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 3, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX, intY - 1, intZ, RoomLit, SquareType.Floor)
									Digger(intX, intY - 2, intZ, RoomLit, SquareType.Floor)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

							' south vertical hallway beside room with single wall between them
							If intY > 4 And intY < 19 And intX > 2 And intX < 57 Then
								If m_Level(intX - 2, intY - 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 2, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY, intZ).FloorType = SquareType.Floor _
								And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								And m_Level(intX - 2, intY + 1, intZ).FloorType = SquareType.Floor _
								And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX - 1, intY, intZ, RoomLit, SquareType.Floor)

									changes += 1
								End If
							End If
							' ----------------------------------------------------------------

						Case SquareType.Door ' check existing doors for placement problems
							If _
							 (m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
							 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
							 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
							 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor) _
							Or _
							 (m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
							 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
							 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
							 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor) _
							Then
								' door placement is ok
								Exit Select
							Else

								' check for "nowhere door" and replace with wall (nowhere doors 
								' are adjacent to rock squares. i.e. undug squares)
								If _
								 m_Level(intX - 1, intY, intZ).FloorType = SquareType.Rock _
								 Or m_Level(intX + 1, intY, intZ).FloorType = SquareType.Rock _
								 Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.Rock _
								 Or m_Level(intX, intY + 1, intZ).FloorType = SquareType.Rock _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' check for double door, vertical then horizontal
								If (m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Door) _
								Then
									' fix vertical double door
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Wall)

									changes += 1
								End If
								If (m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Door) _
								Then
									' fix horizontal double door
									Digger(intX + 1, intY, intZ, RoomLit, SquareType.Wall)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' northbound door is placed poorly (empty space to left of door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX - 1, intY, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' southbound door is placed poorly (empty space to left of door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX - 1, intY, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' southbound door is placed poorly (empty space next to door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX + 1, intY, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' westbound door is placed poorly (empty space below door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' westbound door is placed poorly (empty space above door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX, intY - 1, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' eastbound door is placed poorly (empty space next to door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' adjoining room door is placed poorly (empty space next to door)
								If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
								 And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
								 And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
								 And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
								Then
									Digger(intX, intY, intZ, RoomLit, SquareType.Wall)
									Digger(intX, intY + 1, intZ, RoomLit, SquareType.Door)

									changes += 1
								End If
								' ----------------------------------------------------------------

								' door at end of eastbound horizontal corridor, opens into wall
								If intX > 1 And intX < 56 And intY > 2 And intY < 19 Then
									If m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX + 1, intY, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' door at end of westbound horizontal corridor, opens into wall
								If intX > 3 And intX < 56 And intY > 2 And intY < 19 Then
									If m_Level(intX - 2, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX - 1, intY, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' door at end of southbound vertical corridor, opens into wall
								If intX > 1 And intX < 56 And intY > 2 And intY < 19 Then
									If m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX, intY + 1, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' door at end of northbound vertical corridor, opens into wall
								If intX > 1 And intX < 56 And intY > 4 And intY < 19 Then
									If m_Level(intX + 1, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY - 2, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX, intY - 1, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' orphan door opposite of wall southbound corridor
								If intX > 1 And intX < 56 And intY > 4 And intY < 19 Then
									If m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY - 2, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX, intY - 1, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' east-west door missing northern anchor wall
								If intX > 3 And intX < 59 And intY > 3 And intY < 19 Then
									If m_Level(intX - 2, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 2, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 2, intY + 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Wall _
									Then
										Digger(intX, intY, intZ, RoomLit, SquareType.Floor)
										Digger(intX + 1, intY, intZ, RoomLit, SquareType.Door)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

								' east-west door missing northern anchor wall #2
								If intX > 3 And intX < 59 And intY > 3 And intY < 19 Then
									If m_Level(intX - 2, intY - 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 2, intY, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 2, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX - 1, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX - 1, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX, intY, intZ).FloorType = SquareType.Door _
									And m_Level(intX, intY + 1, intZ).FloorType = SquareType.Wall _
									And m_Level(intX + 1, intY - 1, intZ).FloorType = SquareType.Floor _
									And m_Level(intX + 1, intY, intZ).FloorType = SquareType.Floor _
									And m_Level(intX + 1, intY + 1, intZ).FloorType = SquareType.Floor _
									Then
										Digger(intX, intY - 1, intZ, RoomLit, SquareType.Wall)

										changes += 1
									End If
								End If
								' ----------------------------------------------------------------

							End If

						Case SquareType.StairsUp
							If m_Level(intX, intY, intZ).FloorType = SquareType.StairsUp _
							 And (m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.Door _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.OpenDoor _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.Secret) _
							 And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Floor _
							Then
								Digger(intX + 1, intY, intZ, RoomLit, SquareType.StairsUp)
								Digger(intX, intY, intZ, RoomLit, SquareType.Floor)

								changes += 1
							End If

							If m_Level(intX, intY, intZ).FloorType = SquareType.StairsUp _
							 And (m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.Door _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.OpenDoor _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.Secret) _
							 And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Floor _
							Then
								Digger(intX, intY + 1, intZ, RoomLit, SquareType.StairsUp)
								Digger(intX, intY, intZ, RoomLit, SquareType.Floor)

								changes += 1
							End If

						Case SquareType.StairsDn
							If m_Level(intX, intY, intZ).FloorType = SquareType.StairsDn _
							 And (m_Level(intX - 1, intY, intZ).FloorType = SquareType.Wall _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.Door _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.OpenDoor _
							  Or m_Level(intX - 1, intY, intZ).FloorType = SquareType.Secret) _
							 And m_Level(intX + 2, intY, intZ).FloorType = SquareType.Floor _
							Then
								Digger(intX + 1, intY, intZ, RoomLit, SquareType.StairsDn)
								Digger(intX, intY, intZ, RoomLit, SquareType.Floor)

								changes += 1
							End If

							If m_Level(intX, intY, intZ).FloorType = SquareType.StairsDn _
							 And (m_Level(intX, intY - 1, intZ).FloorType = SquareType.Wall _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.Door _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.OpenDoor _
							  Or m_Level(intX, intY - 1, intZ).FloorType = SquareType.Secret) _
							 And m_Level(intX, intY + 2, intZ).FloorType = SquareType.Floor _
							Then
								Digger(intX, intY + 1, intZ, RoomLit, SquareType.StairsDn)
								Digger(intX, intY, intZ, RoomLit, SquareType.Floor)

								changes += 1
							End If

					End Select
				Next
			Next

			Return changes
		End Function

#End Region

#Region " Room Related Routines "

		Private Function CheckForStairs(ByVal x As Integer, ByVal y As Integer, ByVal z As Integer) As Boolean
			Dim chance As Integer

			Static InRoomUpChance As Integer = 15
			Static InRoomDnChance As Integer = 15

			If m_StairsUpChance > 0 Then
				chance = rng.Next(1, 100)
				If chance <= m_StairsUpChance Then
					chance = rng.Next(1, 100)
					If chance < InRoomUpChance Then
						Digger(x, y, z, , SquareType.StairsUp)
						Debug.WriteLine("Stairs Up in room #" & m_RoomCount + 1)
						Debug.WriteLine("Stairs Up: x=" & x & " y=" & y)
						m_StairsUpRoomNo = m_RoomCount + 1
						m_StairsUpChance = 0
						Return True
					Else
						InRoomUpChance += 5
					End If
				End If
			End If

			If (m_StairsDnChance > 0) And (m_RoomCount > m_StairsUpRoomNo + 1) And (m_StairsUpRoomNo > 0) And (z < m_MaxDepth) Then
				chance = rng.Next(1, 100)
				If chance <= m_StairsDnChance Then
					chance = rng.Next(1, 100)
					If chance < InRoomDnChance Then
						Digger(x, y, z, , SquareType.StairsDn)
						Debug.WriteLine("Stairs Dn in room #" & m_RoomCount + 1)
						Debug.WriteLine("Stairs Dn: x=" & x & " y=" & y)
						m_StairsDnRoomNo = m_RoomCount + 1
						m_StairsDnChance = 0
						Return True
					Else
						InRoomDnChance += 5
					End If
				End If
			End If

			Return False
		End Function

#End Region

#Region " Passage Related Routines "

		Private Sub PlaceDoor(ByVal x As Integer, _
			   ByVal y As Integer, _
			   ByVal z As Integer, _
			   ByVal roomlit As Boolean, _
			   ByVal wall As Integer)

			' dig out the door first
			Digger(x, y, z, roomlit, SquareType.Door)

			' even if the room is dark, we still need to light up the two wall
			' sections adjoining the door.
			Select Case wall
				Case 1, 3 ' north or south wall
					Digger(x - 1, y, z, roomlit, SquareType.Wall)
					Digger(x + 1, y, z, roomlit, SquareType.Wall)
				Case 2, 4 ' east wall
					Digger(x, y - 1, z, roomlit, SquareType.Wall)
					Digger(x, y + 1, z, roomlit, SquareType.Wall)
			End Select

		End Sub

		Private Sub PickWall(ByRef wall As Integer, _
			  ByRef wall2 As Integer, _
			  ByRef CurrentXY() As coord, _
			  ByRef destXY() As coord)

			Dim xWall As Integer, yWall As Integer, _
			 xWeight As Integer, yWeight As Integer, _
			 Result As Boolean

			' first we handle the X axis...
			If CurrentXY(0).x > destXY(1).x Then
				' destination room to left, no X overlap
				' passage should emerge from left wall
				xWall = 4
				xWeight = 3
			ElseIf CurrentXY(0).x > destXY(0).x And CurrentXY(0).x < destXY(1).x Then
				' destination room to left, partial X overlap
				' passage might emerge from left wall
				xWall = 4
				xWeight = 2
			ElseIf CurrentXY(0).x > destXY(0).x And CurrentXY(1).x < destXY(1).x Then
				' current room exists within X range of destination
				' passage should emerge from top or bottom
				xWall = 0
				xWeight = 0
			ElseIf CurrentXY(0).x < destXY(0).x And CurrentXY(1).x > destXY(1).x Then
				' destination room exists within X range of current room
				' passage should emerge from top or bottom
				xWall = 0
				xWeight = 0
			ElseIf CurrentXY(0).x < destXY(0).x And CurrentXY(1).x > destXY(0).x Then
				' destination room to right, partial X overlap
				' passage might emerge from right wall
				xWall = 2
				xWeight = 2
			ElseIf CurrentXY(1).x < destXY(0).x Then
				' destination room to right, no X overlap
				' passage should emerge from right wall
				xWall = 2
				xWeight = 3
			End If

			' then we handle the Y axis
			If CurrentXY(0).y > destXY(2).y Then
				' destination room above, no Y overlap
				' passage will emerge from top wall
				yWall = 1
				yWeight = 3
			ElseIf CurrentXY(0).y > destXY(0).y And CurrentXY(0).y < destXY(2).y Then
				' destination room above, partial Y overlap
				' passage might emerge from top wall
				yWall = 1
				yWeight = 2
			ElseIf CurrentXY(0).y > destXY(0).y And CurrentXY(2).y < destXY(2).y Then
				' current room exists within Y range of destination
				' passage should emerge from left or right
				yWall = 0
				yWeight = 0
			ElseIf CurrentXY(0).y < destXY(0).y And CurrentXY(2).y > destXY(2).y Then
				' destination room exists within Y range of current room
				' passage should emerge from left or right
				yWall = 0
				yWeight = 0
			ElseIf CurrentXY(0).y < destXY(0).y And CurrentXY(2).y > destXY(0).y Then
				' destination room below, partial Y overlap
				' passage might emerge from bottom wall
				yWall = 3
				yWeight = 2
			ElseIf CurrentXY(2).y < destXY(0).y Then
				' destination room below, no Y overlap
				' passage will emerge from bottom wall
				yWall = 3
				yWeight = 3
			End If

			' compare weighted values and assign a starting wall
			' in case of tie, flip a coin
			If yWeight > xWeight Then
				wall = yWall
				wall2 = xWall
			ElseIf xWeight > yWeight Then
				wall = xWall
				wall2 = yWall
			ElseIf xWeight = yWeight Then
				Result = CoinFlip()
				If Result = True Then
					wall = yWall
					wall2 = xWall
					If wall = 0 Then
						wall = xWall
						wall2 = yWall
					End If
				Else
					wall = xWall
					wall2 = yWall
					If wall = 0 Then
						wall = yWall
						wall2 = xWall
					End If
				End If

				' if after all that, either wall is still 0, then pick a wall at random
				' based on the position of the room on the map, i.e. a room in the top-right 
				' corner should use either the bottom or left walls
				If wall = 0 Then
					' room is in bottom right corner
					If CurrentXY(0).x > m_MaxWidth / 2 And CurrentXY(0).y > m_MaxHeight / 2 Then
						wall = rng.Next(0, 1)
						If wall = 0 Then
							wall = 1
						ElseIf wall = 1 Then
							wall = 4
						End If
						' room is in bottom left corner
					ElseIf CurrentXY(0).x <= m_MaxWidth / 2 And CurrentXY(0).y > m_MaxHeight / 2 Then
						wall = rng.Next(0, 1)
						If wall = 0 Then
							wall = 1
						ElseIf wall = 1 Then
							wall = 2
						End If
						' room is in top left corner
					ElseIf CurrentXY(0).x <= m_MaxWidth / 2 And CurrentXY(0).y <= m_MaxHeight / 2 Then
						wall = rng.Next(0, 1)
						If wall = 0 Then
							wall = 2
						ElseIf wall = 1 Then
							wall = 3
						End If
						' room is in top right corner
					ElseIf CurrentXY(0).x > m_MaxWidth / 2 And CurrentXY(0).y <= m_MaxHeight / 2 Then
						wall = rng.Next(0, 1)
						If wall = 0 Then
							wall = 3
						ElseIf wall = 1 Then
							wall = 4
						End If
					End If
				End If

				If wall2 = 0 Then
					' room is in bottom right corner
					If CurrentXY(0).x > m_MaxWidth / 2 And CurrentXY(0).y > m_MaxHeight / 2 Then
						Do Until (wall2 <> wall) And wall2 > 0
							wall2 = rng.Next(0, 1)
							If wall2 = 0 Then
								wall2 = 1
							ElseIf wall2 = 1 Then
								wall2 = 4
							End If
						Loop

						' room is in bottom left corner
					ElseIf CurrentXY(0).x <= m_MaxWidth / 2 And CurrentXY(0).y > m_MaxHeight / 2 Then
						Do Until (wall2 <> wall) And wall2 > 0
							wall2 = rng.Next(0, 1)
							If wall2 = 0 Then
								wall2 = 1
							ElseIf wall2 = 1 Then
								wall2 = 2
							End If
						Loop

						' room is in top left corner
					ElseIf CurrentXY(0).x <= m_MaxWidth / 2 And CurrentXY(0).y <= m_MaxHeight / 2 Then
						Do Until (wall2 <> wall) And wall2 > 0
							wall2 = rng.Next(0, 1)
							If wall2 = 0 Then
								wall2 = 2
							ElseIf wall2 = 1 Then
								wall2 = 3
							End If
						Loop

						' room is in top right corner
					ElseIf CurrentXY(0).x > m_MaxWidth / 2 And CurrentXY(0).y <= m_MaxHeight / 2 Then
						Do Until (wall2 <> wall) And wall2 > 0
							wall2 = rng.Next(0, 1)
							If wall2 = 0 Then
								wall2 = 3
							ElseIf wall2 = 1 Then
								wall2 = 4
							End If
						Loop
					End If

				End If

				If wall2 = wall Then
					Throw New System.Exception("We have a wall problem. A turn should have been made.")
				End If

			End If

		End Sub

		Private Sub SetStartPos(ByVal wall As Integer, _
			  ByRef startpos As coord, _
			  ByRef CurrentXY() As coord, _
			  ByVal DestXY() As coord, _
			  ByVal Z As Integer)
			Dim DoAgain As Boolean

			DoAgain = True
			Select Case wall
				Case 1 ' north
					Do Until DoAgain = False
						startpos.x = rng.Next(CurrentXY(0).x + 1, CurrentXY(1).x)
						startpos.y = CurrentXY(0).y - 1

						' these two IFs are to fix the problem of building doors on corners
						If m_Level(startpos.x, startpos.y + 1, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x + 1, startpos.y + 1, Z).FloorType = SquareType.Rock Then startpos.x -= rng.Next(1, 2)
						If m_Level(startpos.x, startpos.y + 1, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x - 1, startpos.y + 1, Z).FloorType = SquareType.Rock Then startpos.x += rng.Next(1, 2)

						' this is to fix the problem where startpos was set one cell outside of the room
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Rock Then startpos.y += 1

						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Floor Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Wall Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Door Then DoAgain = False
					Loop

				Case 2 ' east
					Do Until DoAgain = False
						startpos.x = CurrentXY(1).x + 1
						startpos.y = rng.Next(CurrentXY(1).y + 1, CurrentXY(3).y)

						' these two IFs are to fix the problem of building doors on corners
						If m_Level(startpos.x - 1, startpos.y, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x - 1, startpos.y + 1, Z).FloorType = SquareType.Rock Then startpos.y -= rng.Next(1, 2)
						If m_Level(startpos.x - 1, startpos.y, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x - 1, startpos.y - 1, Z).FloorType = SquareType.Rock Then startpos.y += rng.Next(1, 2)

						' this is to fix the problem where startpos was set one cell outside of the room
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Rock Then startpos.x -= 1

						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Floor Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Wall Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Door Then DoAgain = False
					Loop

				Case 3 ' south
					Do Until DoAgain = False
						startpos.x = rng.Next(CurrentXY(2).x + 1, CurrentXY(3).x)
						startpos.y = CurrentXY(2).y + 1

						' these two IFs are to fix the problem of building doors on corners
						If m_Level(startpos.x, startpos.y - 1, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x + 1, startpos.y - 1, Z).FloorType = SquareType.Rock Then startpos.x -= rng.Next(1, 2)
						If m_Level(startpos.x, startpos.y - 1, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x - 1, startpos.y - 1, Z).FloorType = SquareType.Rock Then startpos.x += rng.Next(1, 2)

						' this is to fix the problem where startpos was set one cell outside of the room
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Rock Then startpos.y -= 1

						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Floor Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Wall Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Door Then DoAgain = False
					Loop

				Case 4 ' west
					Do Until DoAgain = False
						startpos.x = CurrentXY(0).x - 1
						startpos.y = rng.Next(CurrentXY(0).y + 1, CurrentXY(2).y)

						' these two IFs are to fix the problem of building doors on corners
						If m_Level(startpos.x + 1, startpos.y, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x + 1, startpos.y + 1, Z).FloorType = SquareType.Rock Then startpos.y -= rng.Next(1, 2)
						If m_Level(startpos.x + 1, startpos.y, Z).FloorType = SquareType.Wall And _
						   m_Level(startpos.x + 1, startpos.y - 1, Z).FloorType = SquareType.Rock Then startpos.y += rng.Next(1, 2)

						' this is to fix the problem where startpos was set one cell outside of the room
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Rock Then startpos.x += 1

						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Floor Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Wall Then DoAgain = False
						If m_Level(startpos.x, startpos.y, Z).FloorType = SquareType.Door Then DoAgain = False
					Loop
			End Select

		End Sub

		Private Function SetTurningPoint(ByVal wall As Integer, _
				 ByVal DestXY() As coord) As Integer
			Select Case wall
				Case 1	' north wall
					SetTurningPoint = rng.Next(DestXY(0).y + 1, DestXY(2).y)
				Case 2	' east wall
					SetTurningPoint = rng.Next(DestXY(0).x + 1, DestXY(1).x)
				Case 3	' south wall
					SetTurningPoint = rng.Next(DestXY(0).y + 1, DestXY(2).y)
				Case 4	' west wall
					SetTurningPoint = rng.Next(DestXY(0).x + 1, DestXY(1).x)
			End Select

		End Function

		Private Sub FixCorner(ByVal wall As Integer, _
			   ByVal wall2 As Integer, _
			   ByVal x As Integer, _
			   ByVal y As Integer, _
			   ByVal Z As Integer)

			' if wall = wall2 then we've already made one turn, so make a
			' new wall choice in case we come back here for a second turn
			If wall = wall2 Then
				If wall = 2 Or wall = 4 Then
					If rng.Next(1, 2) = 1 Then
						wall2 = 1
					Else
						wall2 = 3
					End If
				ElseIf wall = 1 Or wall = 3 Then
					If rng.Next(1, 2) = 1 Then
						wall2 = 2
					Else
						wall2 = 4
					End If
				End If
			End If

			Select Case wall
				Case 1
					If wall2 = 2 Then
						If m_Level(x - 1, y, Z).FloorType <> SquareType.Door Then Digger(x - 1, y, Z, , SquareType.Wall)
						If m_Level(x, y - 1, Z).FloorType <> SquareType.Door Then Digger(x, y - 1, Z, , SquareType.Wall)
						If m_Level(x - 1, y - 1, Z).FloorType <> SquareType.Door Then Digger(x - 1, y - 1, Z, , SquareType.Wall)
					ElseIf wall2 = 4 Then
						If m_Level(x + 1, y, Z).FloorType <> SquareType.Door Then Digger(x + 1, y, Z, , SquareType.Wall)
						If m_Level(x, y - 1, Z).FloorType <> SquareType.Door Then Digger(x, y - 1, Z, , SquareType.Wall)
						If m_Level(x + 1, y - 1, Z).FloorType <> SquareType.Door Then Digger(x + 1, y - 1, Z, , SquareType.Wall)
					End If

				Case 2
					If wall2 = 1 Then
						If m_Level(x + 1, y, Z).FloorType <> SquareType.Door Then Digger(x + 1, y, Z, , SquareType.Wall)
						If m_Level(x + 1, y + 1, Z).FloorType <> SquareType.Door Then Digger(x + 1, y + 1, Z, , SquareType.Wall)
						If m_Level(x, y + 1, Z).FloorType <> SquareType.Door Then Digger(x, y + 1, Z, , SquareType.Wall)
					ElseIf wall2 = 3 Then
						If m_Level(x + 1, y, Z).FloorType <> SquareType.Door Then Digger(x + 1, y, Z, , SquareType.Wall)
						If m_Level(x + 1, y - 1, Z).FloorType <> SquareType.Door Then Digger(x + 1, y - 1, Z, , SquareType.Wall)
						If m_Level(x, y - 1, Z).FloorType <> SquareType.Door Then Digger(x, y - 1, Z, , SquareType.Wall)
					End If

				Case 3
					If wall2 = 2 Then
						If m_Level(x - 1, y, Z).FloorType <> SquareType.Door Then Digger(x - 1, y, Z, , SquareType.Wall)
						If m_Level(x - 1, y + 1, Z).FloorType <> SquareType.Door Then Digger(x - 1, y + 1, Z, , SquareType.Wall)
						If m_Level(x, y + 1, Z).FloorType <> SquareType.Door Then Digger(x, y + 1, Z, , SquareType.Wall)
					ElseIf wall2 = 4 Then
						If m_Level(x + 1, y, Z).FloorType <> SquareType.Door Then Digger(x + 1, y, Z, , SquareType.Wall)
						If m_Level(x + 1, y + 1, Z).FloorType <> SquareType.Door Then Digger(x + 1, y + 1, Z, , SquareType.Wall)
						If m_Level(x, y + 1, Z).FloorType <> SquareType.Door Then Digger(x, y + 1, Z, , SquareType.Wall)
					End If

				Case 4
					If wall2 = 1 Then
						If m_Level(x - 1, y, Z).FloorType <> SquareType.Door Then Digger(x - 1, y, Z, , SquareType.Wall)
						If m_Level(x - 1, y + 1, Z).FloorType <> SquareType.Door Then Digger(x - 1, y + 1, Z, , SquareType.Wall)
						If m_Level(x, y + 1, Z).FloorType <> SquareType.Door Then Digger(x, y + 1, Z, , SquareType.Wall)
					ElseIf wall2 = 3 Then
						If m_Level(x - 1, y, Z).FloorType <> SquareType.Door Then Digger(x - 1, y, Z, , SquareType.Wall)
						If m_Level(x - 1, y - 1, Z).FloorType <> SquareType.Door Then Digger(x - 1, y - 1, Z, , SquareType.Wall)
						If m_Level(x, y - 1, Z).FloorType <> SquareType.Door Then Digger(x, y - 1, Z, , SquareType.Wall)
					End If

			End Select

		End Sub

#End Region

#Region " Miscellaneous Routines "

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Function CheckForOverlap(ByVal q As Queue, _
		  ByVal Z As Integer) As Boolean
			Dim intCtr As Integer, _
			 qTmpCnt As Integer, _
			 qTemp As Queue, _
			 XY As coord, _
			 Blip As Boolean = False

			intCtr = 1
			qTemp = q.Clone
			qTmpCnt = qTemp.Count

			'Do While (Not Blip) And (intCtr <= qTmpCnt)
			Do Until Blip Or (intCtr > qTmpCnt)
				If qTemp.Count > 0 Then
					XY = qTemp.Dequeue
				End If

				If XY.x + 2 >= m_MaxWidth Or XY.y + 2 >= m_MaxHeight Then
					' this room placement would exceed bounds of map
					Blip = True
					Debug.WriteLine("*blip* Bounds Exceeded XY.x = " & XY.x & ", XY.y = " & XY.y)
				Else
					' room does not exceed bounds, so check for overlap
					If m_Level(XY.x, XY.y, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x - 1, XY.y, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x + 1, XY.y, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x, XY.y - 1, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x, XY.y + 1, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x - 1, XY.y + 1, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x - 1, XY.y - 1, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x + 1, XY.y + 1, Z).FloorType <> SquareType.Rock _
					Or m_Level(XY.x + 1, XY.y - 1, Z).FloorType <> SquareType.Rock _
					Then
						Blip = True
						Debug.WriteLine("*blip* Overlap Detected at x:" & XY.x & " y:" & XY.y)
						Exit Do
					End If
					intCtr += 1
				End If
			Loop

			Return Blip
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub EnqueueLastRoom(ByVal StartX As Integer, _
		ByVal StartY As Integer, _
		ByVal width As Integer, _
		ByVal height As Integer)
			Dim XY As coord

			' setup a seperate queue containing the 4 corners of the last room placed
			m_qLastRoom.Clear()

			'top left
			XY.x = StartX - 1
			XY.y = StartY - 1
			m_qLastRoom.Enqueue(XY)
			'top right
			XY.x = StartX + width
			XY.y = StartY - 1
			m_qLastRoom.Enqueue(XY)
			'bottom left
			XY.x = StartX - 1
			XY.y = StartY + height
			m_qLastRoom.Enqueue(XY)
			'bottom right
			XY.x = StartX + width
			XY.y = StartY + height
			m_qLastRoom.Enqueue(XY)
		End Sub

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub EnqueueRoomCells(ByRef q As Queue, _
		 ByVal startX As Integer, ByVal startY As Integer, _
		 ByVal width As Integer, ByVal height As Integer, _
		 ByVal Shape As Integer, _
	  Optional ByVal walls As Boolean = False)

			' build a list (queue) of all cell coordinates within a room
			Dim XY As coord, _
			 xCtr As Integer, yCtr As Integer

			q.Clear()
			If walls Then
				If Shape = RoomShape.Square Or Shape = RoomShape.Rectangle Then
					For xCtr = -2 To width + 1
						For yCtr = -2 To height + 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.Circle Then
					For yCtr = -2 To (height / 2)
						For xCtr = (width / 2) - (yCtr + 1) - 2 To (width / 2) + (yCtr + 1) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For yCtr = (height / 2) To height + 1
						For xCtr = (width / 2) - (height - yCtr) - 2 To (width / 2) + (height - yCtr) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.DownLeft Then
					For xCtr = (width / 2) To (width - 1) + 2
						For yCtr = -2 To (height - 1) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = -2 To ((width / 2) - 1) + 2
						For yCtr = (height / 2) To (height - 1) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.DownRight Then
					For xCtr = -2 To (width / 2) + 1
						For yCtr = -2 To (height - 1) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = (width / 2) - 2 To (width - 1) + 2
						For yCtr = (height / 2) To (height - 1) + 2
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.UpRight Then
					For xCtr = -1 To width + 1
						For yCtr = -1 To (height / 2) + 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = -1 To (width / 2) + 1
						For yCtr = height / 2 To height + 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.UpLeft Then
					For xCtr = -1 To width + 1
						For yCtr = -1 To (height / 2) + 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = width / 2 To width + 1
						For yCtr = height / 2 To height + 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				End If
			Else
				If Shape = RoomShape.Square Or Shape = RoomShape.Rectangle Then
					For xCtr = 0 To width - 1
						For yCtr = 0 To height - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.Circle Then
					For yCtr = 0 To (height / 2) - 1
						For xCtr = (width / 2) - (yCtr + 1) To (width / 2) + (yCtr + 1)
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For yCtr = (height / 2) To height - 1
						For xCtr = (width / 2) - (height - yCtr) To (width / 2) + (height - yCtr)
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.DownLeft Then
					For xCtr = width / 2 To width - 1
						For yCtr = 0 To height - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = 0 To (width / 2) - 1
						For yCtr = height / 2 To height - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.DownRight Then
					For xCtr = 0 To (width / 2) - 1
						For yCtr = 0 To (height - 1)
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = (width / 2) To (width - 1)
						For yCtr = (height / 2) To (height - 1)
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.UpRight Then
					For xCtr = 0 To width - 1
						For yCtr = 0 To (height / 2) - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = 0 To (width / 2) - 1
						For yCtr = height / 2 To height - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				ElseIf Shape = RoomShape.UpLeft Then
					For xCtr = 0 To width - 1
						For yCtr = 0 To (height / 2) - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next
					For xCtr = width / 2 To width - 1
						For yCtr = height / 2 To height - 1
							XY.x = startX + xCtr
							XY.y = startY + yCtr
							q.Enqueue(XY)
						Next
					Next

				End If
			End If

		End Sub

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Function CoinFlip() As Boolean
			Dim CoinToss As Integer

			CoinToss = rng.Next(0, 1)
			If CoinToss = 0 Then Return False
			If CoinToss = 1 Then Return True
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub SetSizeShape(ByRef size As Integer, _
		ByRef shape As Integer)

			Dim intTemp As Integer

			' weighted shape
			' 0 = square     25%   1 -  25
			' 1 = rectangle  25%  26 -  50
			' 2 = up,left    10%  51 -  60
			' 3 = up,right   10%  61 -  70
			' 4 = down,left  10%  71 -  80
			' 5 = down,right 10%  81 -  90
			' 6 = circle     10%  91 - 100
			intTemp = rng.Next(1, 90)
			Select Case intTemp
				Case 1 To 25
					shape = RoomShape.Square
				Case 26 To 50
					shape = RoomShape.Rectangle
				Case 51 To 60
					shape = RoomShape.UpLeft
				Case 61 To 70
					shape = RoomShape.UpRight
				Case 71 To 80
					shape = RoomShape.DownLeft
				Case 81 To 90
					shape = RoomShape.DownRight
				Case 91 To 100
					shape = RoomShape.Circle
			End Select

			' weighted size:
			' 0 = tiny      25%     1 -  25
			' 1 = small     35%    26 -  60
			' 2 = medium    25%    61 -  85
			' 3 = large     10%    86 -  95
			' 4 = huge       5%    96 - 100

			Dim intMax As Integer = 100

			' as we get more rooms, scale down the chances of trying to 
			' generate larger rooms in favor of smaller ones
			If m_RoomCount = 9 Then		' no more Mediums
				intMax -= 25
			ElseIf m_RoomCount = 7 Then	' no more Larges
				intMax -= 10
			ElseIf m_RoomCount = 5 Then	' no more Huges
				intMax -= 5
			End If

			intTemp = rng.Next(1, intMax)
			Select Case intTemp
				Case 1 To 25
					size = RoomSize.Tiny
				Case 26 To 60
					size = RoomSize.Small
				Case 61 To 85
					size = RoomSize.Medium
				Case 86 To 95
					size = RoomSize.Large
				Case 96 To 100
					size = RoomSize.Huge
			End Select

		End Sub

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub SetWidthHeight(ByVal size As Integer, _
		  ByVal shape As Integer, _
		  ByRef width As Integer, _
		  ByRef height As Integer)

			' flip a coin for horizontal / vertical orientation
			Dim result As Boolean
			result = CoinFlip()

			' define the room dimensions
			Select Case shape
				Case RoomShape.Square
					width = size + 2
					height = size + 2
				Case RoomShape.Rectangle
					If result Then	' HEADS
						width = (size + 2) * 2
						height = size + 2
					Else			' TAILS
						width = size + 2
						height = (size + 2) * 2
					End If
				Case RoomShape.DownLeft
					width = (size + 2) * 2
					height = (size + 2) * 2

				Case RoomShape.DownRight
					width = (size + 2) * 2
					height = (size + 2) * 2

				Case RoomShape.UpLeft
					width = (size + 2) * 2
					height = (size + 2) * 2

				Case RoomShape.UpRight
					width = (size + 2) * 2
					height = (size + 2) * 2

				Case RoomShape.Circle
					width = (size + 2) * 2
					height = (size + 2) * 2

			End Select
		End Sub

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub Digger(ByVal X As Integer, _
		ByVal Y As Integer, _
		ByVal Z As Integer, _
	 Optional ByVal RoomLit As Boolean = True, _
	 Optional ByVal Type As Integer = SquareType.Floor)

			' If the square to be dug out is already marked as Floor
			' then exit the routine without redigging the square unless
			' its to put in a door or stairs. (ordinarily we would only be 
			' putting a door on a floor square if we're rearranging stuff 
			' because of the the level scrub)
			If m_Level(X, Y, Z).FloorType = SquareType.Floor _
			   And Type <> SquareType.Door _
			   And Type <> SquareType.StairsDn _
			   And Type <> SquareType.StairsUp _
			Then
				Exit Sub
			End If

			' If the square to be dug out is already marked as stairs
			' then exit the routine without redigging the square unless
			' its to move the stairs one to the right or down
			If (m_Level(X, Y, Z).FloorType = SquareType.StairsDn _
			 Or m_Level(X, Y, Z).FloorType = SquareType.StairsUp) _
			And m_Level(X + 1, Y, Z).FloorType = SquareType.Floor _
			And m_Level(X, Y + 1, Z).FloorType = SquareType.Floor _
			Then
				Exit Sub
			End If

			' Dig out a section of Floor at the coordinates passed
			' Type is optional, defaults to floor, pass if otherwise
			Select Case Type
				Case SquareType.Wall	 ' 1
					m_Level(X, Y, Z).FloorType = SquareType.Wall
				Case SquareType.Floor	 ' 2
					m_Level(X, Y, Z).FloorType = SquareType.Floor
				Case SquareType.Door	 ' 3
					m_Level(X, Y, Z).FloorType = SquareType.Door
				Case SquareType.Secret	 ' 4
					m_Level(X, Y, Z).FloorType = SquareType.Secret
				Case SquareType.OpenDoor ' 5
					m_Level(X, Y, Z).FloorType = SquareType.OpenDoor
				Case SquareType.StairsUp ' 6
					m_Level(X, Y, Z).FloorType = SquareType.StairsUp
				Case SquareType.StairsDn ' 7
					m_Level(X, Y, Z).FloorType = SquareType.StairsDn
				Case SquareType.NWCorner ' 9
					m_Level(X, Y, Z).FloorType = SquareType.NWCorner
				Case SquareType.NECorner ' 10
					m_Level(X, Y, Z).FloorType = SquareType.NECorner
				Case SquareType.SECorner ' 11
					m_Level(X, Y, Z).FloorType = SquareType.SECorner
				Case SquareType.SWCorner ' 12
					m_Level(X, Y, Z).FloorType = SquareType.SWCorner

			End Select

			' most rooms are lit by default, but if they aren't then set the 
			' illumination strength to 0 (dark)
			If RoomLit Then
				m_Level(X, Y, Z).Illumination = IlluminationStrength.Normal
			Else
				m_Level(X, Y, Z).Illumination = IlluminationStrength.Dark
			End If

		End Sub

#End Region

#Region " DungeonBuilder Debug Routines "

		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub ShapeSizeDebug(ByVal Shape As Integer, ByVal Size As Integer)
			Debug.Write("Room Shape: ")
			Select Case Shape
				Case RoomShape.DownLeft
					Debug.WriteLine("DownLeft ")
				Case RoomShape.DownRight
					Debug.WriteLine("DownRight")
				Case RoomShape.Rectangle
					Debug.WriteLine("Rectangle")
				Case RoomShape.Square
					Debug.WriteLine("Square   ")
				Case RoomShape.UpLeft
					Debug.WriteLine("Upleft   ")
				Case RoomShape.UpRight
					Debug.WriteLine("Upright  ")
			End Select

			Debug.Write("Room Size: ")
			Select Case Size
				Case RoomSize.Tiny
					Debug.WriteLine("Tiny    ")
				Case RoomSize.Small
					Debug.WriteLine("Small   ")
				Case RoomSize.Medium
					Debug.WriteLine("Medium  ")
				Case RoomSize.Large
					Debug.WriteLine("Large   ")
				Case RoomSize.Huge
					Debug.WriteLine("Huge    ")
			End Select
		End Sub

#End Region

#Region " DungeonBuilder Properties "
		Public ReadOnly Property MaxWidth() As Integer
			Get
				Return m_MaxWidth
			End Get
		End Property

		Public ReadOnly Property MaxHeight() As Integer
			Get
				Return m_MaxHeight
			End Get
		End Property

		Public ReadOnly Property MaxDepth() As Integer
			Get
				Return m_MaxDepth
			End Get
		End Property
#End Region

#Region " Instantiation Code "

		<System.Diagnostics.DebuggerStepThrough()> _
		Public Sub New(ByVal Width As Integer, _
	   ByVal Height As Integer, _
	   ByVal Depth As Integer, _
	   ByVal Rooms As Integer)

			'Re-seed the RNG
			Randomize()

			' set up the dimensions of the grid when we instantiate the object
			m_MaxWidth = Width
			m_MaxHeight = Height
			m_MaxDepth = Depth
			m_RoomsPassed = Rooms

			' create the array, using the GridCell structure to fill it
			ReDim m_Level(m_MaxWidth, m_MaxHeight, m_MaxDepth)

			' initialize the entire 3D grid as one solid block
			Dim intXCtr As Integer, _
			 intYCtr As Integer, _
			 intZCtr As Integer
			For intZCtr = 0 To m_MaxDepth
				For intYCtr = 1 To m_MaxHeight
					For intXCtr = 0 To m_MaxWidth
						m_Level(intXCtr, intYCtr, intZCtr).FloorType = SquareType.Rock
						m_Level(intXCtr, intYCtr, intZCtr).items = New ArrayList
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

	Public Enum TrapType
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
	End Enum

	Public Enum IlluminationStrength
		Dark = 0
		Torch = 1
		Normal = 2
		Bright = 3
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
		Dim TrapType As Integer
		Dim TrapDiscovered As Boolean
		Dim SpecialRoom As Boolean

	End Structure

#End Region

End Namespace