Namespace Common

    Public Class Weather
        Public SightMod As Int16
        Public Temperature As Int16
        Public WindSpeed As Int16
        Public Sky As CloudState

        Private PreciptationAmount As Int16
        Private PrecipitationChance As PrecipChanceByMonth
        Private RainbowChance As RainbowChanceByPrecipType

        Private WeatherDuration As DateTimeOffset

        'TODO: Weather System
        Friend Shared Function CheckWeather(zone As OverlandTerrainType) As String
            Dim Message As String = ""

            Select Case D100()
                Case 1 To 2         ' BLIZZARD, HEAVY
                Case 3 To 5         ' BLIZZARD
                Case 6 To 10        ' SNOWSTORM, HEAVY
                Case 11 To 20       ' SNOWSTORM, LIGHT
                Case 21 To 25       ' SLEETSTORM
                Case 26 To 27       ' HAILSTORM
                Case 28 To 30       ' FOG, HEAVY
                Case 31 To 38       ' FOG, LIGHT
                Case 39 To 40       ' MIST
                Case 41 To 45       ' DRIZZLE
                Case 46 To 60       ' RAINSTORM, LIGHT
                Case 61 To 70       ' RAINSTORM, HEAVY
                Case 71 To 84       ' THUNDERSTORM
                Case 85 To 89       ' TROPICAL STORM
                Case 90 To 94       ' MONSOON
                Case 95 To 97       ' GALE
                Case 98 To 100      ' HURRICANE
            End Select

            Return Message
        End Function

        Friend Shared Function LongString() As String
            Dim message As String = ""

            Return message
        End Function


    End Class

End Namespace

