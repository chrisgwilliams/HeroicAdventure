Imports HA.Common

Public Class TimeKeeper
    Const MINUTE As Int16 = 60
    Const HOUR As Int16 = 3600
    Const DAY As Integer = 86400
    Const WEEK As Integer = 604800
    Const MONTH As Integer = 2592000

    Const INCREMENT As Int16 = 6
    Const DUNGEONTURN As Int16 = INCREMENT * 1
    Const OUTDOORTURN As Int16 = INCREMENT * 100

    Const DAWN As String = "6:25:01 AM"
    Const DAYLIGHT As String = "7:25:01 AM"
    Const DUSK As String = "8:00:00 PM"
    Const NIGHT As String = "8:50:00 PM"

    Const CREATORSDAY_MONTH As Int16 = 1
    Const CREATORSDAY_DAY As Int16 = 6
    Friend Shared CreatorsDay As Boolean = False

    Friend Shared DayNight As DayNightState
    Friend TurnCountAtStateChange As Integer
    Friend Shared GameCreationDate As DateTime  ' Real World
    Friend Shared GameStartDate As DateTime     ' In Game
    Friend Shared GameTime As DateTime

    Friend Shared BirthSign As StarSign

    Friend Shared Sub InitializeTimeKeeper()
        GameCreationDate = Now()

        If GameCreationDate.Month = CREATORSDAY_MONTH _
            AndAlso GameCreationDate.Day = CREATORSDAY_DAY Then
            CreatorsDay = True
        End If

        Dim RandomDay As String = D100(30).ToString
        Dim RandomMonth As String = D12().ToString

        GameStartDate = CDate(RandomDay + "/" + RandomMonth + "/00 00:00:00")
        GameTime = GameStartDate

        BirthSign = RandomMonth
    End Sub

    Friend Shared Function ProcessTime() As String
        Dim Message As String = ""

        If TheHero.Overland Then
            GameTime.AddSeconds(OUTDOORTURN)
        Else
            GameTime.AddSeconds(DUNGEONTURN)
        End If

        'TODO: add talent/feat for knowing sunup/sundown while underground

        If GameTime.TimeOfDay >= TimeSpan.Parse(DAWN) AndAlso GameTime.TimeOfDay < TimeSpan.Parse(DAY) Then
            If DayNight = DayNightState.Night Then
                DayNight = DayNightState.Dawn
                If TheHero.Overland Then
                    Message = "The sun is starting to rise. "
                End If
            End If
        ElseIf GameTime.TimeOfDay >= TimeSpan.Parse(DAY) AndAlso GameTime.TimeOfDay < TimeSpan.Parse(DUSK) Then
            If DayNight = DayNightState.Dawn Then
                DayNight = DayNightState.Day
                If TheHero.Overland Then
                    Message = "The sun has risen. It is day. "
                End If
            End If
        ElseIf GameTime.TimeOfDay >= TimeSpan.Parse(DUSK) AndAlso GameTime.TimeOfDay < TimeSpan.Parse(NIGHT) Then
            If DayNight = DayNightState.Day Then
                DayNight = DayNightState.Dusk
                If TheHero.Overland Then
                    Message = "The sun is starting to set. "
                End If
            End If
        ElseIf GameTime.TimeOfDay >= TimeSpan.Parse(NIGHT) Or GameTime.TimeOfDay < TimeSpan.Parse(DAWN) Then
            If DayNight = DayNightState.Dusk Then
                DayNight = DayNightState.Night
                If TheHero.Overland Then
                    Message = "The sun has set. It is night. "
                End If
            End If
        End If

        Return Message
    End Function

    Friend Shared Function GetGameDateTime() As String

        Return ""
    End Function

    Friend Shared Function GetBirthDate() As String
        Dim BirthDate As String = ""
        Dim suffix As String = ""
        Dim day As Integer = GameStartDate.Day

        If day = 1 Then suffix = "st"
        If day = 2 Then suffix = "nd"
        If day = 3 Then suffix = "rd"
        If day >= 4 Then suffix = "th"
        BirthDate = "You were born on the " + day + suffix + " day of the month of the " + BirthSign.ToString + ". "

        Return BirthDate
    End Function

    'Activity	                                Speed Modifier
    'Slowed (Slow Monster, Or by an opponent)	-50%
    'Overburdened!	                            -40
    'Strained!	                                -20
    'Strained	                                -10
    'Bloated	                                -10
    'Cold blood corruption	                    -10
    'Decay corruption	                        -10
    'Burdened	                                 -5
    'Satiated	                                 -5
    'Talents (Quick, Very Quick, Greased Lightning)	+2, +3, +4 for 9 total
    'Athletics Skill	                   up To +8
    'Raven-born	                                +10
    'Dexterity	                                 +1 for every 2 points above 17

End Class
