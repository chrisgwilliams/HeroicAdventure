Module AutoWalk

#Region " AutoWalk Subs and Functions "
	Private Function SafeToAutoWalk_Check(Optional ByVal ExtendedRadius As Boolean = False) As Boolean
		' this scans a 5x5 area for monsters* and 3x3 area for items & traps#, 
		' stopping at the first hit regardless of where it is.
		' Affected area:
		' *****
		' *###*
		' *#@#*
		' *###*
		' *****

		If TheHero.InTown Then
			Return ScanTownMap(ExtendedRadius)
		Else
			Return ScanDungeonMap(ExtendedRadius)
		End If

	End Function

	Private Function ScanDungeonMap(Optional ByVal ExtendedRadius As Boolean = False) As Boolean
		' 5/13/12 adding code to stop autowalk when adjacent to items or visible traps.

		If ExtendedRadius Then
			If Level(TheHero.LocX - 2, TheHero.LocY - 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 1, TheHero.LocY - 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX, TheHero.LocY - 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 1, TheHero.LocY - 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 2, TheHero.LocY - 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 2, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 2, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 2, TheHero.LocY, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 2, TheHero.LocY, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 2, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 2, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 2, TheHero.LocY + 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX - 1, TheHero.LocY + 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX, TheHero.LocY + 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 1, TheHero.LocY + 2, TheHero.LocZ).Monster > 0 _
			OrElse Level(TheHero.LocX + 2, TheHero.LocY + 2, TheHero.LocZ).Monster > 0 _
			Then
				' it's NOT safe to autowalk
				Return False
			End If
		End If

		' adjacent monster
		If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).Monster > 0 _
		Then
			Return False
		End If

		'adjacent item
		If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		Then
			Return False
		End If

		' adjacent (visible) trap
		If Level(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse Level(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		Then
			Return False
		End If

		' all tests passed, so its safe to walk.
		Return True

	End Function

	Private Function ScanTownMap(Optional ByVal ExtendedRadius As Boolean = False) As Boolean
		' 5/13/12 adding code to stop autowalk when adjacent to items or visible traps.

		If ExtendedRadius Then
			If m_TownMap(TheHero.LocX - 2, TheHero.LocY - 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY - 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX, TheHero.LocY - 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY - 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 2, TheHero.LocY - 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 2, TheHero.LocY - 1, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 2, TheHero.LocY - 1, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 2, TheHero.LocY, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 2, TheHero.LocY, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 2, TheHero.LocY + 1, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 2, TheHero.LocY + 1, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 2, TheHero.LocY + 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY + 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX, TheHero.LocY + 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY + 2, TheHero.LocZ).NPC > 0 _
			OrElse m_TownMap(TheHero.LocX + 2, TheHero.LocY + 2, TheHero.LocZ).NPC > 0 _
			Then
				' it's NOT safe to autowalk
				Return False
			End If
		End If

		' adjacent villager
		If m_TownMap(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).NPC > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).NPC > 0 _
		Then
			Return False
		End If

		'adjacent item
		If m_TownMap(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).items.Count > 0 _
		Then
			Return False
		End If

		' adjacent (visible) trap
		If m_TownMap(TheHero.LocX - 1, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY - 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX - 1, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		OrElse m_TownMap(TheHero.LocX + 1, TheHero.LocY + 1, TheHero.LocZ).TrapDiscovered = True _
		Then
			Return False
		End If

		' all tests passed, so its safe to walk.
		Return True

	End Function


	Friend Enum CompassDirection As Integer
		NorthWest = 7
		North = 8
		NorthEast = 9
		West = 4
		StandStill = 5
		East = 6
		SouthWest = 1
		South = 2
		SouthEast = 3
	End Enum

	Friend Function EvaluateDirection(ByVal intDirection As CompassDirection) As String
		Dim strMessage As String = ""

		Select Case intDirection
			Case CompassDirection.South
				intDirection = South(intDirection)
			Case CompassDirection.North
				intDirection = North(intDirection)
			Case CompassDirection.West
				intDirection = West(intDirection)
			Case CompassDirection.East
				intDirection = East(intDirection)
			Case CompassDirection.SouthWest
				intDirection = SouthWest(intDirection)
			Case CompassDirection.SouthEast
				intDirection = SouthEast(intDirection)
			Case CompassDirection.NorthWest
				intDirection = NorthWest(intDirection)
			Case CompassDirection.NorthEast
				intDirection = NorthEast(intDirection)
			Case 5
				If SafeToAutoWalk_Check(True) Then
					' no action needed, keep waiting
				Else
					' we're at an intersection or a monster is within 2 squares
					TheHero.AutoWalk = False
					intDirection = 0
				End If

		End Select

		Return intDirection
	End Function

	Private Function South(ByVal intDirection As Integer) As Integer
		With TheHero
			.AutoWalk = True

			' check for something immediately ahead of us
			If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) = 0 Then
				'now check for monsters
				If SafeToAutoWalk_Check() Then
					' now check for other stuff that may interfere:

					'1. did we hit the beginning of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					Then
						.AutoWalk = False
					End If

					'2. did we hit the end of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) > 0 _
					Then
						.AutoWalk = False
					End If

					'3. did we hit the beginning of a hallway, alongside an EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					Then
						.AutoWalk = False
					End If

					'4. did we hit the beginning of a hallway, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					Then
						.AutoWalk = False
					End If

					'5. did we hit an opening in a room, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					Then
						.AutoWalk = False
					End If

					'6. did we hit an opening in a room, alongside the EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					Then
						.AutoWalk = False
					End If
				Else
					.AutoWalk = False
				End If
			Else
				.AutoWalk = False
			End If

			If .AutoWalk = False Then intDirection = 0
		End With

		Return intDirection
	End Function
	Private Function North(ByVal intDirection As Integer) As Integer
		With TheHero
			.AutoWalk = True

			' check for something immediately ahead of us
			If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) = 0 Then
				'now check for monsters
				If SafeToAutoWalk_Check() Then
					' now check for other stuff that may interfere:

					'1. did we hit the beginning of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					Then
						.AutoWalk = False
					End If

					'2. did we hit the end of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) > 0 _
					Then
						.AutoWalk = False
					End If

					'3. did we hit the beginning of a hallway, alongside an EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					Then
						.AutoWalk = False
					End If

					'4. did we hit the beginning of a hallway, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					Then
						.AutoWalk = False
					End If

					'5. did we hit an opening in a room, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					Then
						.AutoWalk = False
					End If

					'6. did we hit an opening in a room, alongside the EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					Then
						.AutoWalk = False
					End If
				Else
					.AutoWalk = False
				End If
			Else
				.AutoWalk = False
			End If

			If .AutoWalk = False Then intDirection = 0
		End With

		Return intDirection
	End Function
	Private Function West(ByVal intDirection As Integer) As Integer
		With TheHero
			.AutoWalk = True

			' check for something immediately ahead of us
			If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 Then
				'now check for monsters
				If SafeToAutoWalk_Check() Then
					' now check for other stuff that may interfere:

					'1. did we hit the beginning of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) = 0 _
					Then
						.AutoWalk = False
					End If

					'2. did we hit the end of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) > 0 _
					Then
						.AutoWalk = False
					End If

					'3. did we hit the beginning of a hallway, alongside a NORTH wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					Then
						.AutoWalk = False
					End If

					'4. did we hit the beginning of a hallway, alongside a SOUTH wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					Then
						.AutoWalk = False
					End If

					'5. did we hit an opening in a room, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					Then
						.AutoWalk = False
					End If

					'6. did we hit an opening in a room, alongside the EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					Then
						.AutoWalk = False
					End If
				Else
					.AutoWalk = False
				End If
			Else
				.AutoWalk = False
			End If

			If .AutoWalk = False Then intDirection = 0
		End With

		Return intDirection

	End Function
	Private Function East(ByVal intDirection As Integer) As Integer
		With TheHero
			.AutoWalk = True

			' check for something immediately ahead of us
			If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 Then
				'now check for monsters
				If SafeToAutoWalk_Check() Then
					' now check for other stuff that may interfere:

					'1. did we hit the beginning of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) = 0 _
					Then
						.AutoWalk = False
					End If

					'2. did we hit the end of a hallway, or an open doorway?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) > 0 _
					Then
						.AutoWalk = False
					End If

					'3. did we hit the beginning of a hallway, alongside a NORTH wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.North) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					Then
						.AutoWalk = False
					End If

					'4. did we hit the beginning of a hallway, alongside a SOUTH wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.South) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					Then
						.AutoWalk = False
					End If

					'5. did we hit an opening in a room, alongside the WEST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) > 0 _
					Then
						.AutoWalk = False
					End If

					'6. did we hit an opening in a room, alongside the EAST wall?
					If CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.West) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthWest) > 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.NorthEast) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.East) = 0 _
					AndAlso CheckForCollision(.LocX, .LocY, .LocZ, CompassDirection.SouthEast) = 0 _
					Then
						.AutoWalk = False
					End If
				Else
					.AutoWalk = False
				End If
			Else
				.AutoWalk = False
			End If

			If .AutoWalk = False Then intDirection = 0
		End With

		Return intDirection

	End Function
	Private Function NorthWest(ByVal intDirection As Integer) As Integer
		If CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection - 3) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection + 1) = 0 _
		AndAlso SafeToAutoWalk_Check() _
		Then
			' no action needed, stay on course
		Else
			' we're at an intersection or a monster is within 2 squares
			TheHero.AutoWalk = False
			intDirection = 0
		End If

		Return intDirection
	End Function
	Private Function NorthEast(ByVal intDirection As Integer) As Integer
		If CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection - 3) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection - 1) = 0 _
		AndAlso SafeToAutoWalk_Check() _
		Then
			' no action needed, stay on course
		Else
			' we're at an intersection or a monster is within 2 squares
			TheHero.AutoWalk = False
			intDirection = 0
		End If

		Return intDirection
	End Function
	Private Function SouthWest(ByVal intDirection As Integer) As Integer
		If CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection + 1) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection + 3) = 0 _
		AndAlso SafeToAutoWalk_Check() _
		Then
			' no action needed, stay on course
		Else
			' we're at an intersection or a monster is within 2 squares
			TheHero.AutoWalk = False
			intDirection = 0
		End If

		Return intDirection
	End Function
	Private Function SouthEast(ByVal intDirection As Integer) As Integer
		If CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection - 1) = 0 _
		AndAlso CheckForCollision(TheHero.LocX, TheHero.LocY, TheHero.LocZ, intDirection + 3) = 0 _
		AndAlso SafeToAutoWalk_Check() _
		Then
			' no action needed, stay on course
		Else
			' we're at an intersection or a monster is within 2 squares
			TheHero.AutoWalk = False
			intDirection = 0
		End If

		Return intDirection
	End Function


#End Region

End Module
