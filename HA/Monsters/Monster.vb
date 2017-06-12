Imports System.Math
Imports DBuild.DunGen3
Imports HA.Common

<DebuggerStepThrough()> Public MustInherit Class Monster
    Inherits Avatar

    Public AttackList As New ArrayList

    '********************************************************************************************************
    'Cullen's changes for sorting characters by initiative

    'At some point, the initiative calculation should include a random roll fo a d20; that would be added here
    Public Overrides ReadOnly Property TotalInitForRound() As Integer
        Get
			Return Helper.AbilityMod(EDexterity) + Initiative
        End Get
    End Property
    Public Overrides Property ECharisma() As Integer
        Get
            Return Charisma
        End Get
        Set(ByVal value As Integer)
            Charisma = value
        End Set
    End Property
    Public Overrides Property EConstitution() As Integer
        Get
            Return Constitution
        End Get
        Set(ByVal value As Integer)
            Constitution = value
        End Set
    End Property
    Public Overrides Property EDexterity() As Integer
        Get
            Return Dexterity
        End Get
        Set(ByVal value As Integer)
            Dexterity = value
        End Set
    End Property
    Public Overrides Property EIntelligence() As Integer
        Get
            Return Intelligence
        End Get
        Set(ByVal value As Integer)
            Intelligence = value
        End Set
    End Property
    Public Overrides Property EStrength() As Integer
        Get
            Return Strength
        End Get
        Set(ByVal value As Integer)
            Strength = value
        End Set
    End Property
    Public Overrides Property EWisdom() As Integer
        Get
            Return Wisdom
        End Get
        Set(ByVal value As Integer)
            Wisdom = value
        End Set
    End Property
    'end Cullen's changes for sorting characters by initiative
    '********************************************************************************************************

    Public Property CR() As Single
    Public Property MonsterRace() As String
    Public Property MonsterRacePlural() As String
    Public Property Number() As Integer
    Public Property Character() As String
    Public Property DestX() As Integer
    Public Property DestY() As Integer
    Public Property Observed() As Boolean
    Public Property Fear() As Integer
    Public Property Attacks() As Integer
    Public Property Chat() As String

    Public Sub New()
        Observed = False

        ' female = 0, male = 1
        Gender = RND.Next(0, 1)

        ' most monster have hands (to open doors), so set default to TRUE 
        ' and set individual monsters to FALSE as needed
        HasHands = True

        ' this primarily affects combat messages, set individual to false as needed
        HasFeet = True

        ' set generic chat text for critters with nothing to say. 
        ' This can be overridden below of course.
        Chat = "Apparently it has nothing to say."

        Equipped.BackPack = New ArrayList
    End Sub
End Class

Module m_Monster
#Region " Monster Subs and Functions "
Friend Sub SpawnMonsters()
    ' (new creatures should be generated out of LOS every 40 
    ' turns, after at least 100 turns on the current level, 
    ' assuming no more than 60 creatures TOTAL have been 
    ' spawned on this level)
    If TheHero.TurnCount - TheHero.TurnCountAtDungeonLevelChange >= 100 _
    And m_arrMonster.Count <= 60 _
    And TheHero.TurnCount Mod 40 = 0 Then
        PlaceMonsters(TheHero.LocZ, 1)
    End If
End Sub

Private Function MonsterAttack(ByVal intEncounter As Integer, _
                               ByVal intMID As Integer) As String
    Dim AC As Integer, _
    intDamage As Integer, minDam As Integer, maxDam As Integer, _
    intRoll As Integer, _
    intCtr As Integer, _
    AlreadyDead As Boolean

    AC = TheHero.AC
    MonsterAttack = ""

        'TODO: add check for Monster in a dark room, should affect their ability to hit hero
        'TODO: add check for Monster in a dark room, should affect combat status messages, UNLESS hero saw the monster before entering a dark room (maybe?)

        For intCtr = 0 To m_arrMonster(intMID).attacklist.count - 1

        ' start building the combat message...
        MonsterAttack += "The " & m_arrMonster(intMID).monsterrace

        intRoll = D20()
        If (intRoll + m_arrMonster(intMID).attacklist(intCtr).bonus >= AC) Or intRoll = 20 Then
            ' hero was hit, so calculate the damage
            minDam = m_arrMonster(intMID).attacklist(intCtr).MinDamage
            maxDam = m_arrMonster(intMID).attacklist(intCtr).MaxDamage
            intDamage = RND.Next(minDam, maxDam)

        ElseIf intRoll = 1 Then ' critical miss
				intDamage = 0
				Select Case RND.Next(1, 3)
					Case 1
						If m_arrMonster(intMID).hashands Then
							MonsterAttack += " swings"
						Else
							MonsterAttack += " strikes"
						End If
						MonsterAttack += " wildly and misses you horribly. "
					Case 2
						MonsterAttack += " hesitates. "
					Case 3
						If m_arrMonster(intMID).hasfeet Then
							MonsterAttack += " missteps and nearly falls. "
						Else
							MonsterAttack += " seems briefly confused. "
						End If
				End Select

        Else ' regular miss
            intDamage = 0
            MonsterAttack += " lunges forward but misses you. "
        End If

        If intDamage > 0 Then
            ' hero was hit, so subtract the damage from current health
            TheHero.CurrentHP -= intDamage

            'check for poison creatures too
            Dim strPoisonMessage As String = ""
            If m_arrMonster(intMID).attacklist(intCtr).poison Then
                Dim iPD As Int16 = TheHero.PoisonDuration
                PoisonHero()

                strPoisonMessage = " You've been poisoned"
                If iPD > 1 AndAlso TheHero.PoisonDuration > iPD Then
						strPoisonMessage &= " again!"
                Else
						strPoisonMessage &= " !"
                End If
            End If

            ' update the HP counter
            UpdateHPDisplay()

            Select Case intDamage
                Case Is > maxDam * 0.75
                    MonsterAttack += " " & m_arrMonster(intMID).attacklist(intCtr).verb & " you savagely with its " & m_arrMonster(intMID).attacklist(intCtr).name & "! "
                Case Is > maxDam * 0.5
                    Select Case RND.Next(1, 3)
                        Case 1
                            MonsterAttack += " " & m_arrMonster(intMID).attacklist(intCtr).verb & " you with its " & m_arrMonster(intMID).attacklist(intCtr).name & "! "
                        Case 2
                            MonsterAttack += " lands a solid hit with its " & m_arrMonster(intMID).attacklist(intCtr).name & "! "
                        Case 3
                            MonsterAttack += " " & m_arrMonster(intMID).attacklist(intCtr).verb & " you! "
                    End Select
                Case Else
                    Select Case RND.Next(1, 2)
                        Case 1
                            MonsterAttack += " grazes you with its " & m_arrMonster(intMID).attacklist(intCtr).name & "! "
                        Case 2
                            MonsterAttack += " barely hits you with its " & m_arrMonster(intMID).attacklist(intCtr).name & "! "
                    End Select
            End Select

            ' let hero know they've been poisoned when it happens
            If TheHero.Poisoned Then MonsterAttack += strPoisonMessage

        End If
    Next

    ' is the hero dead?
    If TheHero.CurrentHP <= 0 And Not AlreadyDead Then
            MonsterAttack += RandomDeathMessage()
            AlreadyDead = True
            TheHero.KilledBy = m_arrMonster(intMID).monsterrace
    End If

End Function

Friend Function MonsterAction(ByVal strMessage As String) As String
        Dim HeroIsNoticed As Boolean
        Dim intObstacle As Integer
        Dim intEncounter As Integer
        Dim intCtr As Integer

        ' we need to check each monster on this level
        For intCtr = 0 To m_arrMonster.Count - 1
            With m_arrMonster(intCtr)

                ' has the monster been killed?
                If .dead = False Then

                    ' check to see if PC is within range of vision
                    HeroIsNoticed = MonsterLOS(intCtr)

                    ' Trolls regenerate d4 HP every other round
                    ' TODO: we don't differentiate between regular damage and fire/acid/magic damage.
                    If (.monsterrace = "troll" And TheHero.TurnCount Mod 2 = 0) Then
                        If .CurrentHP < .HP Then
                            If Abs(.locx - TheHero.LocX) <= TheHero.Sight _
                        And Abs(.locy - TheHero.LocY) <= TheHero.Sight Then
                                strMessage &= "Some of the troll's wounds close. "
                            End If
                            .currenthp += D4()
                            If .currenthp > .hp Then
                                .currenthp = .hp
                                strMessage &= "The troll is completely healed. "
                            End If
                        End If
                    End If

                    If HeroIsNoticed Then
                        Dim intDirection As Integer

                        intDirection = MonsterFindDirection(intCtr)

                        ' We have a direction, see if anything is in the way
                        intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, intDirection)
                        intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, intDirection)

                        ' nothing is in the way, so go ahead and move
                        If intObstacle = 0 And intEncounter = 0 Then
                            Level(.locX, .locY, TheHero.LocZ).Monster = 0

                            ' if its a square we've seen before, tidy it up, otherwise dont worry about it
                            If Level(.locX, .locY, TheHero.LocZ).Observed = True Then
                                FixFloor(.locX, .locY, TheHero.LocZ)
                            End If

                            If Not (.monsterrace = "zombie") Then
                                .walk(intDirection)
                            ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                .walk(intDirection)
                            Else
                                ' zombies only walk every other turn
                            End If

                            Level(.locX, .locY, TheHero.LocZ).Monster = intCtr + 1
                            RedrawMonsters()
                            HeroLOS()

                        ElseIf intObstacle = SquareType.Wall _
                            Or intObstacle = SquareType.NWCorner _
                            Or intObstacle = SquareType.NECorner _
                            Or intObstacle = SquareType.SECorner _
                            Or intObstacle = SquareType.SWCorner Then
                            Level(.locX, .locY, TheHero.LocZ).Monster = 0

                            ' if its a square we've seen before, tidy it up, otherwise dont worry about it
                            If Level(.locX, .locY, TheHero.LocZ).Observed Then
                                FixFloor(.locX, .locY, TheHero.LocZ)
                            End If

                            ' pick a new direction that follows the passage (or room) wall instead of 
                            ' going directly towards the hero
                            Select Case intDirection
                                Case 1
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 4)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 4)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(4)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(4)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(4)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(4, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 2)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 2)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(2)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(2)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(2)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(2, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 2
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 1)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 1)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(1)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(1)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(1)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(1, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 3)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 3)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(3)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(3)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(3)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(3, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 3
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 2)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 2)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(2)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(2)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(2)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(2, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 6)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 6)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(6)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(6)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(6)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(6, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 4
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 7)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 7)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(7)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(7)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(7)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(7, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 1)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 1)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(1)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(1)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(1)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(1, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 6
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 3)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 3)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(3)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(3)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(3)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(3, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 9)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 9)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(9)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(9)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(9)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(9, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 7
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 8)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 8)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(8)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(8)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(8)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(8, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 4)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 4)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(4)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(4)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(4)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(4, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 8
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 7)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 7)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(7)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(7)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(7)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(7, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 9)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 9)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(9)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(9)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(9)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(9, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                Case 9
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 6)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 6)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(6)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(6)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(6)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(6, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 8)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 8)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        If Not (.monsterrace = "zombie") Then
                                            .walk(8)
                                        ElseIf (.monsterrace = "zombie" And TheHero.TurnCount Mod 2 = 0) Then
                                            .walk(8)
                                        Else
                                            ' zombies only walk every other turn
                                        End If
                                        'm_arrMonster(intCtr).walk(8)
                                        Exit Select
                                    ElseIf intObstacle = SquareType.Door Then
                                        If .hashands And .intelligence > 0 Then
                                            strMessage += MonsterOpenDoor(8, intCtr)
                                        ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                            ' probably animal (dog, rat, lizard, etc)
                                            strMessage += "You hear a scratching noise. "
                                        Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                            ' these things don't make any noise
                                        End If
                                    End If
                            End Select

                            Level(.locX, .locY, TheHero.LocZ).Monster = intCtr + 1
                            RedrawMonsters()

                        ElseIf intObstacle = SquareType.Door Then
                            If .hashands And .intelligence > 0 Then
                                ' if the monster has hands AND a brain, open the door
                                strMessage += MonsterOpenDoor(intDirection, intCtr)
                            ElseIf .intelligence > 0 And LCase(.ToString).Contains("spider") = False Then
                                ' probably animal (dog, rat, lizard, etc)
                                strMessage += "You hear a scratching noise. "
                            Else ' must be non-sentient (undead, ooze, jelly, etc), or its a spider!
                                ' these things don't make any noise
                            End If

                        ElseIf intEncounter = 9999 Then
                            ' don't let critters attack the round our hero appears on a new level
                            If TheHero.TurnCountAtDungeonLevelChange < TheHero.TurnCount Then
                                Debug.WriteLine(.monsterrace & " is attacking.")

                                ' start building the combat messages
                                strMessage += MonsterAttack(intEncounter, intCtr)

                                ' is the hero dead?
                                If TheHero.CurrentHP <= 0 Then
                                    ' set the hero status to dead
                                    TheHero.Dead = True
                                    ' debug messages
                                    Debug.WriteLine("Hero has been killed.")
                                    Exit For
                                End If
                            End If

                        ElseIf intEncounter > 0 Then
                            Debug.WriteLine("Monster in the way, sidestepping")

                            Level(.locX, .locY, TheHero.LocZ).Monster = 0

                            ' if its a square we've seen before, tidy it up, otherwise dont worry about it
                            If Level(.locX, .locY, TheHero.LocZ).Observed = True Then
                                FixFloor(.locX, .locY, TheHero.LocZ)
                            End If

                            ' pick a new direction that goes around the monster in the way
                            Select Case intDirection
                                Case 1
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 4)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 4)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(4)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 2)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 2)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(2)
                                        Exit Select
                                    End If

                                Case 2
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 1)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 1)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(1)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 3)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 3)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(3)
                                        Exit Select
                                    End If

                                Case 3
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 2)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 2)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(2)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 6)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 6)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(6)
                                        Exit Select
                                    End If

                                Case 4
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 7)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 7)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(7)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 1)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 1)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(1)
                                        Exit Select
                                    End If

                                Case 6
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 3)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 3)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(3)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 9)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 9)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(9)
                                        Exit Select
                                    End If

                                Case 7
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 8)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 8)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(8)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 4)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 4)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(4)
                                        Exit Select
                                    End If

                                Case 8
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 7)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 7)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(7)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 9)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 9)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(9)
                                        Exit Select
                                    End If

                                Case 9
                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 6)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 6)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(6)
                                        Exit Select
                                    End If

                                    intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, 8)
                                    intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, 8)
                                    If intObstacle = 0 And intEncounter = 0 Then
                                        .walk(8)
                                        Exit Select
                                    End If
                            End Select

                            Level(.locX, .locY, TheHero.LocZ).Monster = intCtr + 1
                            RedrawMonsters()

                        End If

                        'TODO: have monster pick up item on floor if any

                        ' attack pc if in range

                    Else
                        Dim intDirection As Integer
                        intDirection = RND.Next(1, 9)

                        ' We have a direction, see if anything is in the way
                        intObstacle = CheckForCollision(.locX, .locY, TheHero.LocZ, intDirection)
                        intEncounter = CheckForMonster(.locX, .locY, TheHero.LocZ, intDirection)

                        If intObstacle = 0 And intEncounter = 0 Then
                            ' move randomly
                            Level(.locX, .locY, TheHero.LocZ).Monster = 0
                            If Level(.locX, .locY, TheHero.LocZ).Observed = True Then
                                FixFloor(.locX, .locY, TheHero.LocZ)
                            End If

                            ' move the monster 1 square
                            .walk(intDirection)

                            Level(.locX, .locY, TheHero.LocZ).Monster = intCtr + 1
                            RedrawMonsters()
                        End If

                    End If
                End If
            End With
        Next
        MonsterAction = strMessage

End Function

Private Function MonsterLOS(ByVal intMID As Integer) As Boolean
    ' intMID is the index of the monster array
    ' scan the area around monster to see if PC is within visual range
    ' if PC is spotted, set destination coordinates to PC location
    Dim intX As Integer, intY As Integer, intZ As Integer, _
        intSight As Integer, _
        intDarkVision As Integer, _
        intXCtr As Integer, _
        intYCtr As Integer

    intX = m_arrMonster(intMID).LocX
    intY = m_arrMonster(intMID).LocY
    intZ = m_arrMonster(intMID).LocZ
    intSight = m_arrMonster(intMID).sight
    intDarkVision = m_arrMonster(intMID).darkvision

    For intXCtr = intX - intSight To intX + intSight
        For intYCtr = intY - intSight To intY + intSight
            If intXCtr = TheHero.LocX And intYCtr = TheHero.LocY Then
                m_arrMonster(intMID).DestX = TheHero.LocX
                m_arrMonster(intMID).DestY = TheHero.LocY
                Return True
            End If
        Next
    Next

End Function

Private Function MonsterFindDirection(ByVal intMID As Integer) As Integer
    ' if player is SW of monster
    If m_arrMonster(intMID).destX < m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY > m_arrMonster(intMID).LocY Then Return 1

    ' if player is S of monster
    If m_arrMonster(intMID).destX = m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY > m_arrMonster(intMID).LocY Then Return 2

    ' if player is SE of monster
    If m_arrMonster(intMID).destX > m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY > m_arrMonster(intMID).LocY Then Return 3

    ' if player is W of monster
    If m_arrMonster(intMID).destX < m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY = m_arrMonster(intMID).LocY Then Return 4

    ' if player is E of monster
    If m_arrMonster(intMID).destX > m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY = m_arrMonster(intMID).LocY Then Return 6

    ' if player is NW of monster
    If m_arrMonster(intMID).destX < m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY < m_arrMonster(intMID).LocY Then Return 7

    ' if player is N of monster
    If m_arrMonster(intMID).destX = m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY < m_arrMonster(intMID).LocY Then Return 8

    ' if player is NE of monster
    If m_arrMonster(intMID).destX > m_arrMonster(intMID).LocX _
    And m_arrMonster(intMID).destY < m_arrMonster(intMID).LocY Then Return 9
End Function

Private Function MonsterOpenDoor(ByVal intDoorDirection As Integer, _
                                 ByVal intMID As Integer) As String

    Dim intX As Integer, intY As Integer, intZ As Integer
    intX = m_arrMonster(intMID).locx
    intY = m_arrMonster(intMID).locy
    intZ = m_arrMonster(intMID).locz

    Dim doorLoc As coord, dieroll As Int16, DoorDc As Byte

    ' take the direction and convert to coordinates
    Select Case intDoorDirection
        Case 1
            doorLoc.x = intX - 1
            doorLoc.y = intY + 1
        Case 2
            doorLoc.x = intX
            doorLoc.y = intY + 1
        Case 3
            doorLoc.x = intX + 1
            doorLoc.y = intY + 1
        Case 4
            doorLoc.x = intX - 1
            doorLoc.y = intY
        Case 6
            doorLoc.x = intX + 1
            doorLoc.y = intY
        Case 7
            doorLoc.x = intX - 1
            doorLoc.y = intY - 1
        Case 8
            doorLoc.x = intX
            doorLoc.y = intY - 1
        Case 9
            doorLoc.x = intX + 1
            doorLoc.y = intY - 1
    End Select

    'is the door stuck?
		dieroll = D20() + Helper.AbilityMod(m_arrMonster(intMID).strength)
    DoorDc = 9 + D10()
    If dieroll < DoorDc Then
        MonsterOpenDoor = "You hear a grunting noise. "
    Else
        ' open the door
        Level(doorLoc.x, doorLoc.y, intZ).FloorType = SquareType.OpenDoor

        If Abs(doorLoc.x - TheHero.LocX) > TheHero.Sight _
        Or Abs(doorLoc.y - TheHero.LocY) > TheHero.Sight Then
            ' door opened outside of LOS
            MonsterOpenDoor = "You hear a door open somewhere in the dungeon. "
        Else
            WriteAt(doorLoc.x, doorLoc.y, "/", ConsoleColor.DarkYellow)
            HeroLOS()
            MonsterOpenDoor = "The door opens. "
        End If
    End If
End Function

Friend Function BuildMonsterStatusMsg(ByVal activemonster As Monster) As String
    Dim monStatus As String = ""

    ' examine the activemonster and get applicable status
    monStatus = IIf(activemonster.Confused, "confused ", "")
    monStatus &= IIf(activemonster.Poisoned, IIf(monStatus.Length > 0, ", poisoned ", "poisoned "), "")
    monStatus &= IIf(activemonster.Sleeping, IIf(monStatus.Length > 0, ", sleeping ", "sleeping "), "")

    ' Add any status, such as: confused, sleeping, etc
    Dim MonsterMsg As String = monStatus & activemonster.MonsterRace & "."

    ' Generate the article (A, An)
    Dim monArticle As String = GetArticle(MonsterMsg, False)

    ' Stick the article in front of our message
    MonsterMsg = monArticle & MonsterMsg

    Return MonsterMsg
End Function
#End Region

End Module