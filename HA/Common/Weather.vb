﻿Namespace Common

    Public Class Weather
        Public Shared VisibilityMod As Int16
        Public Shared MoveMod As Decimal
        Public Shared Temperature As Int16
        Public Shared WindSpeed As Int16
        Public Shared WindDirection As Heading
        Public Shared Sky As CloudState
        Public Shared Rainbow As Boolean

        Private Shared PreviousWeatherType As String
        Public Shared CurrentWeatherType As String
        Private Shared WeatherChange As Boolean

        Private PreciptationAmount As Int16
        Friend Shared RainbowChance As RainbowChanceByPrecipType
        Private WeatherDuration As DateTimeOffset

        'TODO: Weather System
        Friend Shared Function CheckWeather(zone As OverlandTerrainType) As String
            Dim PrecipitationChance As PrecipChanceByMonth
            Dim Message As String = ""
            WindSpeed = 0
            VisibilityMod = 0
            MoveMod = 0

            ' These two methods compute Temp and Precip Chance by Month and Zone
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

            PreviousWeatherType = CurrentWeatherType
            If D100() <= PrecipitationChance Then
                GetPrecipitation(zone)  ' This also computes windspeed, visibility, and Rainbow Chance
                CheckForRainbow()
            Else
                GetSkyAndWind()
            End If

            If WindSpeed > 0 Then WindDirection = D8()
            AdjustmentsForWindSpeed()

            WeatherChange = PreviousWeatherType <> CurrentWeatherType

            ' TODO: need weather status messages
            Return Message
        End Function

        Friend Shared Function WeatherReport() As String
            Dim message As String = ""

            ' TODO: need full weather report, for status page
            Return message
        End Function

        Private Shared Sub GetSkyAndWind()
            Select Case D6()
                Case 1 To 3
                    CurrentWeatherType = CloudState.Clear.ToString
                    WindSpeed += (D10() + D8() - 2)
                Case 4 To 5
                    CurrentWeatherType = CloudState.PartlyCloudy.ToString
                    WindSpeed += D12() + 8
                Case 6
                    CurrentWeatherType = CloudState.Cloudy
                    WindSpeed += D12() + 10
            End Select
        End Sub

        Private Shared Sub GetPrecipitation(zone As OverlandTerrainType)

            Select Case D100()
                Case 1 To 2         ' BLIZZARD, HEAVY / SANDSTORM, HEAVY
                    WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                    VisibilityMod = -3

                    If zone = OverlandTerrainType.Desert Then
                        CurrentWeatherType = "Heavy Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.HeavySandstorm
                    Else
                        CurrentWeatherType = "Heavy Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.HeavyBlizzard

                        If Temperature > 10 Then Temperature = 10
                    End If

                Case 3 To 5         ' BLIZZARD
                    WindSpeed = D8() + D8() + D8() + 36
                    VisibilityMod = -2

                    If zone = OverlandTerrainType.Desert Then
                        CurrentWeatherType = "Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.Sandstorm
                    Else
                        CurrentWeatherType = "Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.Blizzard

                        If Temperature > 20 Then Temperature = D20()
                    End If

                Case 6 To 10        ' SNOWSTORM, HEAVY
                    WindSpeed = D10() + D10() + D10()
                    VisibilityMod = -2
                    CurrentWeatherType = "Heavy Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.HeavySnowstorm

                    If Temperature > 25 Then Temperature = D20() + 5

                Case 11 To 20       ' SNOWSTORM
                    WindSpeed = D6() + D6() + D6() + D6()
                    VisibilityMod = -1
                    CurrentWeatherType = "Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.Snowstorm

                    If Temperature > 35 Then Temperature = D20() + 15

                Case 21 To 25       ' SLEETSTORM
                    WindSpeed = D10() + D10() + D10()
                    VisibilityMod = -1
                    CurrentWeatherType = "Sleetstorm"
                    RainbowChance = RainbowChanceByPrecipType.Sleet

                    If Temperature > 35 Then Temperature = D20() + 15

                Case 26 To 27       ' HAILSTORM
                    'No Effect on Visibility
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D10() + D10() + D10() + D10()
                        CurrentWeatherType = "Hailstorm"
                        RainbowChance = RainbowChanceByPrecipType.Hail

                        If Temperature > 65 Then Temperature = D20() + D20() + D20() + 5
                    End If

                Case 28 To 30       ' FOG, HEAVY
                    VisibilityMod = -3
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D20()
                        CurrentWeatherType = "Heavy Fog"
                        RainbowChance = RainbowChanceByPrecipType.HeavyFog

                        If Temperature < 20 Or Temperature > 60 Then Temperature = 20 + D20() + D20()
                    End If

                Case 31 To 38       ' FOG
                    VisibilityMod = -2
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        WindSpeed = D10()
                        CurrentWeatherType = "Fog"
                        RainbowChance = RainbowChanceByPrecipType.LightFog

                        If Temperature < 30 Or Temperature > 70 Then Temperature = 30 + D20() + D20()
                    End If

                Case 39 To 40       ' MIST
                    WindSpeed = D10()
                    CurrentWeatherType = "Mist"
                    RainbowChance = RainbowChanceByPrecipType.Mist

                    If Temperature < 30 Then Temperature = 30

                Case 41 To 45       ' DRIZZLE
                    WindSpeed = D20()
                    CurrentWeatherType = "Drizzle"
                    RainbowChance = RainbowChanceByPrecipType.Drizzle

                    If Temperature < 25 Then Temperature = 25

                Case 46 To 60       ' RAINSTORM, LIGHT
                    WindSpeed = D20()
                    CurrentWeatherType = "Light Rain"
                    RainbowChance = RainbowChanceByPrecipType.LightRain

                    If Temperature < 25 Then Temperature = 25

                Case 61 To 70       ' RAINSTORM, HEAVY
                    VisibilityMod = -1
                    WindSpeed = D12() + D12() + 10
                    CurrentWeatherType = "Heavy Rain"
                    RainbowChance = RainbowChanceByPrecipType.HeavyRain

                    If Temperature < 25 Then Temperature = 25

                Case 71 To 84       ' THUNDERSTORM
                    VisibilityMod = -1
                    WindSpeed = D10() + D10() + D10() + D10()
                    CurrentWeatherType = "Thunderstorm"
                    RainbowChance = RainbowChanceByPrecipType.ThunderStorm

                    If Temperature < 30 Then Temperature = 30

                Case 85 To 89       ' TROPICAL STORM
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        VisibilityMod = -1
                        WindSpeed = D12() + D12() + 10
                        CurrentWeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain

                        If Temperature < 25 Then Temperature = 25
                    Else
                        VisibilityMod = -2
                        WindSpeed = D12() + D12() + D12() + 30
                        CurrentWeatherType = "Tropical Storm"
                        RainbowChance = RainbowChanceByPrecipType.TropicalStorm

                        If Temperature < 40 Then Temperature = 40
                    End If

                Case 90 To 94       ' MONSOON
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        VisibilityMod = -1
                        WindSpeed = D12() + D12() + 10
                        CurrentWeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain

                        If Temperature < 25 Then Temperature = 25
                    Else
                        VisibilityMod = -3
                        WindSpeed = D10() + D10() + D10() + D10() + D10() + D10()
                        CurrentWeatherType = "Monsoon"
                        RainbowChance = RainbowChanceByPrecipType.Monsoon

                        If Temperature < 55 Then Temperature = 55
                    End If

                Case 95 To 97       ' GALE
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        VisibilityMod = -3
                        WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                        CurrentWeatherType = "Gale"
                        RainbowChance = RainbowChanceByPrecipType.Gale

                        If Temperature < 40 Then Temperature = 40
                    End If

                Case 98 To 100      ' HURRICANE
                    If zone = OverlandTerrainType.Desert Then
                        WindSpeed = D4()
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        VisibilityMod = -3
                        WindSpeed = D10() + D10() + D10() + D10() + D10() + D10() + D10() + 70
                        CurrentWeatherType = "Hurricane"
                        RainbowChance = RainbowChanceByPrecipType.Hurricane

                        If Temperature < 55 Then Temperature = 55
                    End If

            End Select
        End Sub

        Private Shared Sub CheckForRainbow()
            If D100() <= RainbowChance Then Rainbow = True
        End Sub

        Private Shared Sub AdjustmentsForWindSpeed()
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

