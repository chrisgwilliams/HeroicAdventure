Namespace Common

    Public Class Weather
        Public Shared VisibilityMod As Int16
        Public Shared MoveMod As Decimal
        Public Shared Temperature As Int16
        Public Shared WindSpeed As Int16
        Public Shared WindDirection As Heading
        Public Shared Sky As CloudState
        Public Shared Rainbow As Boolean
        Public Shared WeatherType As String

        Private PreciptationAmount As Int16
        Friend Shared RainbowChance As RainbowChanceByPrecipType
        Private WeatherDuration As DateTimeOffset

        'TODO: Weather System
        Friend Shared Function CheckWeather(zone As OverlandTerrainType) As String
            Dim PrecipitationChance As PrecipChanceByMonth
            Dim Message As String = ""
            VisibilityMod = 0
            MoveMod = 0

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
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Temperature -= D20()
                        Case DayNightState.Day
                            Temperature += (D20() * 2)
                        Case DayNightState.Dusk
                            Temperature += D10()
                        Case DayNightState.Night
                            Temperature -= D10()
                    End Select

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

            If D100() <= PrecipitationChance Then
                GetWeather(zone)
                AdjustmentsForWindSpeed()
                CheckForRainbow()
            End If

            ' TODO: still need weather status messages
            Return Message
        End Function

        Friend Shared Function WeatherReport() As String
            Dim message As String = ""

            ' TODO: need full weather report, for status page
            Return message
        End Function

        Private Shared Sub GetWeather(zone As OverlandTerrainType)
            'TODO: adjust result by min/max temp and zone

            Select Case D100()
                Case 1 To 2         ' BLIZZARD, HEAVY / SANDSTORM, HEAVY
                    WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                    VisibilityMod = -3

                    If zone = OverlandTerrainType.Desert Then
                        WeatherType = "Heavy Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.HeavySandstorm
                    Else
                        WeatherType = "Heavy Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.HeavyBlizzard
                    End If

                Case 3 To 5         ' BLIZZARD
                    WindSpeed = D8() + D8() + D8() + 36
                    VisibilityMod = -2

                    If zone = OverlandTerrainType.Desert Then
                        WeatherType = "Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.Sandstorm
                    Else
                        WeatherType = "Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.Blizzard
                    End If

                Case 6 To 10        ' SNOWSTORM, HEAVY
                    WindSpeed = D10() + D10() + D10()
                    VisibilityMod = -2
                    WeatherType = "Heavy Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.HeavySnowstorm

                Case 11 To 20       ' SNOWSTORM
                    WindSpeed = D6() + D6() + D6() + D6()
                    VisibilityMod = -1
                    WeatherType = "Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.Snowstorm

                Case 21 To 25       ' SLEETSTORM
                    WindSpeed = D10() + D10() + D10()
                    VisibilityMod = -1
                    WeatherType = "Sleetstorm"
                    RainbowChance = RainbowChanceByPrecipType.Sleet

                Case 26 To 27       ' HAILSTORM
                    'No Effect on Visibility
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D10() + D10() + D10() + D10()
                        WeatherType = "Hailstorm"
                        RainbowChance = RainbowChanceByPrecipType.Hail
                    End If

                Case 28 To 30       ' FOG, HEAVY
                    VisibilityMod = -3
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D20()
                        WeatherType = "Heavy Fog"
                        RainbowChance = RainbowChanceByPrecipType.HeavyFog
                    End If

                Case 31 To 38       ' FOG
                    VisibilityMod = -2
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D10()
                        WeatherType = "Fog"
                        RainbowChance = RainbowChanceByPrecipType.LightFog
                    End If

                Case 39 To 40       ' MIST
                    WindSpeed = D10()
                    WeatherType = "Mist"
                    RainbowChance = RainbowChanceByPrecipType.Mist

                Case 41 To 45       ' DRIZZLE
                    WindSpeed = D20()
                    WeatherType = "Drizzle"
                    RainbowChance = RainbowChanceByPrecipType.Drizzle

                Case 46 To 60       ' RAINSTORM, LIGHT
                    WindSpeed = D20()
                    WeatherType = "Light Rain"
                    RainbowChance = RainbowChanceByPrecipType.LightRain

                Case 61 To 70       ' RAINSTORM, HEAVY
                    VisibilityMod = -1
                    WindSpeed = D12() + D12() + 10
                    WeatherType = "Heavy Rain"
                    RainbowChance = RainbowChanceByPrecipType.HeavyRain

                Case 71 To 84       ' THUNDERSTORM
                    VisibilityMod = -1
                    WindSpeed = D10() + D10() + D10() + D10()
                    WeatherType = "Thunderstorm"
                    RainbowChance = RainbowChanceByPrecipType.ThunderStorm

                Case 85 To 89       ' TROPICAL STORM
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        VisibilityMod = -1
                        WindSpeed = D12() + D12() + 10
                        WeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain
                    Else
                        VisibilityMod = -2
                        WindSpeed = D12() + D12() + D12() + 30
                        WeatherType = "Tropical Storm"
                        RainbowChance = RainbowChanceByPrecipType.TropicalStorm
                    End If

                Case 90 To 94       ' MONSOON
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        VisibilityMod = -1
                        WindSpeed = D12() + D12() + 10
                        WeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain
                    Else
                        VisibilityMod = -3
                        WindSpeed = D10() + D10() + D10() + D10() + D10() + D10()
                        WeatherType = "Monsoon"
                        RainbowChance = RainbowChanceByPrecipType.Monsoon
                    End If

                Case 95 To 97       ' GALE
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        VisibilityMod = -3
                        WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                        WeatherType = "Gale"
                        RainbowChance = RainbowChanceByPrecipType.Gale
                    End If

                Case 98 To 100      ' HURRICANE
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        WeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        VisibilityMod = -3
                        WindSpeed = D10() + D10() + D10() + D10() + D10() + D10() + D10() + 70
                        WeatherType = "Hurricane"
                        RainbowChance = RainbowChanceByPrecipType.Hurricane
                    End If

            End Select

            If WindSpeed > 0 Then WindDirection = D8()
        End Sub

        Friend Shared Sub CheckForRainbow()
            If D100() <= RainbowChance Then Rainbow = True
        End Sub

        Friend Shared Sub AdjustmentsForWindSpeed()
            ' TODO: Some of these windspeed adjustments require modification to main loop and avatar classes

            Select Case WindSpeed
                Case < 30
                    ' normal
                    MoveMod = 1.0
                Case 30 To 44
                    ' no torches
                    ' missiles 1/2 range, -1 TH
                    MoveMod = 1.25

                Case 45 To 59
                    ' no torches
                    ' no fires
                    ' missiles 1/4 range, -3 TH
                    MoveMod = 1.5

                Case 60 To 74
                    ' no torches
                    ' no fires
                    ' small trees uprooted
                    ' no missile fire
                    ' nomagical melee at -1 TH
                    ' AC Dex Bonus cancelled
                    MoveMod = 1.75

                Case > 74
                    ' no torches
                    ' no fires
                    ' small trees uprooted
                    ' building damage
                    ' movement impossible
                    ' chance (adjusted by weight) of knocked prone
                    ' no missile fire
                    ' nomagical melee at -3 TH
                    ' 20% chance of crit fumble (disarmed by wind)
                    ' AC Dex Bonus cancelled
                    MoveMod = 0

            End Select
        End Sub

    End Class

End Namespace

