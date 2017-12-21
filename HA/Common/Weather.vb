Namespace Common

    Public Class Weather
        Public Shared SightMod As Int16
        Public Shared Temperature As Int16
        Public Shared WindSpeed As Int16
        Public Shared Sky As CloudState
        Public Shared Rainbow As Boolean

        Private PreciptationAmount As Int16
        Private RainbowChance As RainbowChanceByPrecipType
        Private WeatherDuration As DateTimeOffset

        'TODO: Weather System
        Friend Shared Function CheckWeather(zone As OverlandTerrainType) As String
            Dim PrecipitationChance As PrecipChanceByMonth
            Dim Message As String = ""

            Select Case Month(TimeKeeper.GameTime)
                Case GameMonth.Raven
                    PrecipitationChance = PrecipChanceByMonth.Raven
                    Temperature = BaseTempByMonth.Raven
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= D20()
                        Case DayNightState.Day
                            Temperature += D10()
                        Case DayNightState.Dusk
                            Temperature += 0
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

                Case GameMonth.Book
                    PrecipitationChance = PrecipChanceByMonth.Book
                    Temperature = BaseTempByMonth.Book
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Temperature += (D6() + 4)
                        Case DayNightState.Dusk
                            Temperature += D6()
                        Case DayNightState.Night
                            Temperature -= D6()
                    End Select

                Case GameMonth.Wand
                    PrecipitationChance = PrecipChanceByMonth.Wand
                    Temperature = BaseTempByMonth.Wand
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Temperature += (D8() + 4)
                        Case DayNightState.Dusk
                            Temperature += D8()
                        Case DayNightState.Night
                            Temperature -= D8()
                    End Select

                Case GameMonth.Unicorn
                    PrecipitationChance = PrecipChanceByMonth.Unicorn
                    Temperature = BaseTempByMonth.Unicorn
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D8() + 4)
                        Case DayNightState.Day
                            Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Temperature += D8()
                        Case DayNightState.Night
                            Temperature -= D8()
                    End Select

                Case GameMonth.Salamander
                    PrecipitationChance = PrecipChanceByMonth.Salamander
                    Temperature = BaseTempByMonth.Salamander
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D10() + 6)
                        Case DayNightState.Day
                            Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Temperature += D10()
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

                Case GameMonth.Dragon
                    PrecipitationChance = PrecipChanceByMonth.Dragon
                    Temperature = BaseTempByMonth.Dragon
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Temperature += (D8() + 8)
                        Case DayNightState.Dusk
                            Temperature += D8()
                        Case DayNightState.Night
                            Temperature -= D6()
                    End Select

                Case GameMonth.Sword
                    PrecipitationChance = PrecipChanceByMonth.Sword
                    Temperature = BaseTempByMonth.Sword
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Temperature += (D6() + 4)
                        Case DayNightState.Dusk
                            Temperature += D6()
                        Case DayNightState.Night
                            Temperature -= D6()
                    End Select

                Case GameMonth.Falcon
                    PrecipitationChance = PrecipChanceByMonth.Falcon
                    Temperature = BaseTempByMonth.Falcon
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Temperature += (D4() + 6)
                        Case DayNightState.Dusk
                            Temperature += D4()
                        Case DayNightState.Night
                            Temperature -= D6()
                    End Select

                Case GameMonth.Cup
                    PrecipitationChance = PrecipChanceByMonth.Cup
                    Temperature = BaseTempByMonth.Cup
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D8() + 6)
                        Case DayNightState.Day
                            Temperature += (D8() + 6)
                        Case DayNightState.Dusk
                            Temperature += D8()
                        Case DayNightState.Night
                            Temperature -= D8()
                    End Select

                Case GameMonth.Candle
                    PrecipitationChance = PrecipChanceByMonth.Candle
                    Temperature = BaseTempByMonth.Candle
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D10() + 5)
                        Case DayNightState.Day
                            Temperature += (D10() + 5)
                        Case DayNightState.Dusk
                            Temperature += D10()
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

                Case GameMonth.Wolf
                    PrecipitationChance = PrecipChanceByMonth.Wolf
                    Temperature = BaseTempByMonth.Wolf
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Temperature += D10()
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

                Case GameMonth.Tree
                    PrecipitationChance = PrecipChanceByMonth.Tree
                    Temperature = BaseTempByMonth.Tree
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= D20()
                        Case DayNightState.Day
                            Temperature += (D8() + 5)
                        Case DayNightState.Dusk
                            Temperature += D8()
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

            End Select

            Select Case zone
                Case OverlandTerrainType.Desert
                    PrecipitationChance -= 30
                Case OverlandTerrainType.Forest
                    PrecipitationChance += 5
                Case OverlandTerrainType.Hills
                    PrecipitationChance += 0
                Case OverlandTerrainType.Mountain
                    Temperature -= 15
                Case OverlandTerrainType.Plains
                    PrecipitationChance += 0
                Case OverlandTerrainType.Road
                    PrecipitationChance += 0
                Case OverlandTerrainType.Special
                    PrecipitationChance += 0
                Case OverlandTerrainType.Town
                    PrecipitationChance += 0
                Case OverlandTerrainType.Volcano
                    Temperature += 40
                Case OverlandTerrainType.Water
                    Temperature -= 10
            End Select

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

