Namespace Common

    Public Class TimeKeeper
        Const MINUTE As Int16 = 60
        Const HOUR As Int16 = 3600
        Const DAY As Integer = 86400
        Const WEEK As Integer = 604800
        Const MONTH As Integer = 2592000

        Const INCREMENT As Int16 = 6
        Const DUNGEONTURN As Int16 = INCREMENT * 1
        Const OUTDOORTURN As Int16 = INCREMENT * 100

        Const DAWN As String = "06:25:01"
        Const DAYLIGHT As String = "07:25:01"
        Const DUSK As String = "20:00:00"
        Const NIGHT As String = "20:50:00"

        Const CREATORSDAY_MONTH As Int16 = 1
        Const CREATORSDAY_DAY As Int16 = 6
        Friend Shared CreatorsDay As Boolean = False

        Friend Shared DayNight As DayNightState
        Friend TurnCountAtStateChange As Integer
        Friend Shared GameCreationDate As DateTime  ' Real World, to check for Creators Day
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

        Friend Shared Function Update(Optional Action As ActionType = ActionType.None) As String
            Dim Message As String = ""

            If TheHero.Overland Then
                If TheHero.TerrainZoom Then
                    GameTime = GameTime.AddSeconds(DUNGEONTURN)
                Else
                    GameTime = GameTime.AddSeconds(OUTDOORTURN)
                End If
            Else
                GameTime = GameTime.AddSeconds(DUNGEONTURN)
            End If

            'TODO: create talent/feat for knowing sunup/sundown while underground
            'TODO: Call Weather.update as part of ProcessTime
            If GameTime.TimeOfDay >= TimeSpan.Parse(DAWN) AndAlso GameTime.TimeOfDay < TimeSpan.Parse(DAYLIGHT) Then
                If DayNight = DayNightState.Night Then
                    DayNight = DayNightState.Dawn
                    If TheHero.Overland Then
                        Message = "The sun is starting to rise. "
                    End If
                End If
            ElseIf GameTime.TimeOfDay >= TimeSpan.Parse(DAYLIGHT) AndAlso GameTime.TimeOfDay < TimeSpan.Parse(DUSK) Then
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

        Friend Shared Function GetGameElapsedTimeMessage(Alive As Boolean) As String
            Dim ts As TimeSpan = GameTime - GameStartDate
            Dim preamble As String
            Dim postamble As String

            Dim Mon As Integer = ts.Days \ 30
            Dim Day As Integer = If(Mon > 0, ts.Days Mod 30, ts.Days)
            Dim Hrs As Integer = ts.Hours
            Dim Min As Integer = ts.Minutes
            Dim Sec As Integer = ts.Seconds

            Dim MonMsg As String = If(Mon > 0, Mon.ToString + If(Mon = 1, " month, ", " months, "), "")
            Dim DayMsg As String = If(Day > 0, Day.ToString + If(Day = 1, " day, ", " days, "), "")
            Dim HrsMsg As String = If(Hrs > 0, Hrs.ToString + If(Hrs = 1, " hour, ", " hours, "), "")
            Dim MinMsg As String = If(Min > 0, Min.ToString + If(Min = 1, " minute, ", " minutes, "), "")
            Dim SecMsg As String = If(Sec > 0, Sec.ToString + If(Sec = 1, " second", " seconds"), "")

            If Alive Then
                preamble = "You have been adventuring for "
                postamble = " so far. "
            Else
                preamble = "survived for "
            End If

            Dim message As String = preamble + MonMsg + DayMsg + HrsMsg + MinMsg + SecMsg
            If Left(message, message.Length - 1) = "," Then message = Left(message, message.Length - 2)

            Return message

        End Function

        Friend Shared Function GetBirthDateMessage() As String
            Dim suffix As String

            Dim day As Integer = GameStartDate.Day
            If day = 1 Then suffix = "st"
            If day = 2 Then suffix = "nd"
            If day = 3 Then suffix = "rd"
            If day >= 4 Then suffix = "th"

            Return "You were born on the " + day + suffix + " day of the month of the " + BirthSign.ToString + ". "
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

End Namespace