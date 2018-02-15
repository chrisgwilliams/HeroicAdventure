Imports System.Collections.Generic

Namespace Common

    Public Class Weather
        Public Shared CurrentWeatherType As String

        Private Shared PreviousWeatherType As String
        Private Shared WeatherChange As Boolean
        Private Shared RainbowChance As RainbowChanceByPrecipType

        Private WeatherDuration As DateTimeOffset

        'TODO: Weather System
        Friend Shared Function CheckWeather(zone As OverlandTerrainType) As String
            Dim PrecipitationChance As PrecipChanceByMonth

            'ToDo: storms should persist for multiple turns. 
            '      check for new weather once the storm has ended, rather than every round.
            '      this comes into play when in tactical mode (overland), because tactical 
            '      turns use smaller time increments than overland map movement.
            ResetWeatherEffectsForNewTurn()

            ' These two blocks compute Temp and Precip Chance by Month and Zone
            Select Case Month(TimeKeeper.GameTime)
                Case GameMonth.Raven
                    PrecipitationChance = PrecipChanceByMonth.Raven
                    Effects.Temperature = BaseTempByMonth.Raven
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= D20()
                        Case DayNightState.Day
                            Effects.Temperature += D10()
                        Case DayNightState.Dusk
                            Effects.Temperature += 0
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

                Case GameMonth.Book
                    PrecipitationChance = PrecipChanceByMonth.Book
                    Effects.Temperature = BaseTempByMonth.Book
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Effects.Temperature += (D6() + 4)
                        Case DayNightState.Dusk
                            Effects.Temperature += D6()
                        Case DayNightState.Night
                            Effects.Temperature -= D6()
                    End Select

                Case GameMonth.Wand
                    PrecipitationChance = PrecipChanceByMonth.Wand
                    Effects.Temperature = BaseTempByMonth.Wand
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Effects.Temperature += (D8() + 4)
                        Case DayNightState.Dusk
                            Effects.Temperature += D8()
                        Case DayNightState.Night
                            Effects.Temperature -= D8()
                    End Select

                Case GameMonth.Unicorn
                    PrecipitationChance = PrecipChanceByMonth.Unicorn
                    Effects.Temperature = BaseTempByMonth.Unicorn
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D8() + 4)
                        Case DayNightState.Day
                            Effects.Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Effects.Temperature += D8()
                        Case DayNightState.Night
                            Effects.Temperature -= D8()
                    End Select

                Case GameMonth.Salamander
                    PrecipitationChance = PrecipChanceByMonth.Salamander
                    Effects.Temperature = BaseTempByMonth.Salamander
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D10() + 6)
                        Case DayNightState.Day
                            Effects.Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Effects.Temperature += D10()
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

                Case GameMonth.Dragon
                    PrecipitationChance = PrecipChanceByMonth.Dragon
                    Effects.Temperature = BaseTempByMonth.Dragon
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Effects.Temperature += (D8() + 8)
                        Case DayNightState.Dusk
                            Effects.Temperature += D8()
                        Case DayNightState.Night
                            Effects.Temperature -= D6()
                    End Select

                Case GameMonth.Sword
                    PrecipitationChance = PrecipChanceByMonth.Sword
                    Effects.Temperature = BaseTempByMonth.Sword
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Effects.Temperature += (D6() + 4)
                        Case DayNightState.Dusk
                            Effects.Temperature += D6()
                        Case DayNightState.Night
                            Effects.Temperature -= D6()
                    End Select

                Case GameMonth.Falcon
                    PrecipitationChance = PrecipChanceByMonth.Falcon
                    Effects.Temperature = BaseTempByMonth.Falcon
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D6() + 6)
                        Case DayNightState.Day
                            Effects.Temperature += (D4() + 6)
                        Case DayNightState.Dusk
                            Effects.Temperature += D4()
                        Case DayNightState.Night
                            Effects.Temperature -= D6()
                    End Select

                Case GameMonth.Cup
                    PrecipitationChance = PrecipChanceByMonth.Cup
                    Effects.Temperature = BaseTempByMonth.Cup
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D8() + 6)
                        Case DayNightState.Day
                            Effects.Temperature += (D8() + 6)
                        Case DayNightState.Dusk
                            Effects.Temperature += D8()
                        Case DayNightState.Night
                            Effects.Temperature -= D8()
                    End Select

                Case GameMonth.Candle
                    PrecipitationChance = PrecipChanceByMonth.Candle
                    Effects.Temperature = BaseTempByMonth.Candle
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D10() + 5)
                        Case DayNightState.Day
                            Effects.Temperature += (D10() + 5)
                        Case DayNightState.Dusk
                            Effects.Temperature += D10()
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

                Case GameMonth.Wolf
                    PrecipitationChance = PrecipChanceByMonth.Wolf
                    Effects.Temperature = BaseTempByMonth.Wolf
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= (D10() + 4)
                        Case DayNightState.Day
                            Effects.Temperature += (D10() + 6)
                        Case DayNightState.Dusk
                            Effects.Temperature += D10()
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

                Case GameMonth.Tree
                    PrecipitationChance = PrecipChanceByMonth.Tree
                    Effects.Temperature = BaseTempByMonth.Tree
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= D20()
                        Case DayNightState.Day
                            Effects.Temperature += (D8() + 5)
                        Case DayNightState.Dusk
                            Effects.Temperature += D8()
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

            End Select
            Select Case zone
                Case OverlandTerrainType.Desert
                    PrecipitationChance -= 30
                    Select Case TimeKeeper.DayNight
                        Case DayNightState.Dawn
                            Effects.Temperature -= D20()
                        Case DayNightState.Day
                            Effects.Temperature += (D20() * 2)
                        Case DayNightState.Dusk
                            Effects.Temperature += D10()
                        Case DayNightState.Night
                            Effects.Temperature -= D10()
                    End Select

                Case OverlandTerrainType.Forest
                    PrecipitationChance += 5
                Case OverlandTerrainType.Hills
                    PrecipitationChance += 0
                Case OverlandTerrainType.Mountain
                    Effects.Temperature -= 15
                Case OverlandTerrainType.Plains
                    PrecipitationChance += 0
                Case OverlandTerrainType.Road
                    PrecipitationChance += 0
                Case OverlandTerrainType.Special
                    PrecipitationChance += 0
                Case OverlandTerrainType.Town
                    PrecipitationChance += 0
                Case OverlandTerrainType.Volcano
                    Effects.Temperature += 40
                Case OverlandTerrainType.Water
                    Effects.Temperature -= 10
            End Select

            PreviousWeatherType = CurrentWeatherType
            If D100() <= PrecipitationChance Then
                GetPrecipitation(zone)  ' This also computes windspeed, visibility, and Rainbow Chance
                CheckForRainbow()
            Else
                GetSkyAndWind()
            End If

            If Effects.WindSpeed > 0 Then Effects.WindDirection = D8()

            AdjustmentsForWindSpeed()

            WeatherChange = PreviousWeatherType <> CurrentWeatherType

            ' TODO: need weather status messages
            Return WeatherReport(True)
        End Function

        Public Shared Function WeatherReport(Optional Shortened As Boolean = False) As String
            Dim message As String = ""

            If Shortened Then
                message = CurrentWeatherType
            Else
                'ToDo: Determine format for more verbose weather message (status page)

            End If

            Return message
        End Function

        Private Shared Sub GetSkyAndWind()
            Select Case D6()
                Case 1 To 3
                    Effects.Sky = CloudState.Clear.ToString
                    Effects.WindSpeed += (D10() + D8() - 2)
                Case 4 To 5
                    Effects.Sky = CloudState.PartlyCloudy.ToString
                    Effects.WindSpeed += D12() + 8
                Case 6
                    Effects.Sky = CloudState.Cloudy
                    Effects.WindSpeed += D12() + 10
            End Select

            CurrentWeatherType = Effects.Sky
        End Sub

        Private Shared Sub GetPrecipitation(zone As OverlandTerrainType)
            ' TODO: Calculate Precipitation Amount

            Select Case D100()
                Case 1 To 2         ' BLIZZARD, HEAVY / SANDSTORM, HEAVY
                    Effects.WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                    Effects.VisibilityMod = -3

                    If zone = OverlandTerrainType.Desert Then
                        CurrentWeatherType = "Heavy Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.HeavySandstorm
                        Effects.Sky = CloudState.Clear
                    Else
                        CurrentWeatherType = "Heavy Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.HeavyBlizzard
                        Effects.Sky = CloudState.Cloudy

                        If Effects.Temperature > 10 Then Effects.Temperature = 10
                    End If

                Case 3 To 5         ' BLIZZARD
                    Effects.WindSpeed = D8() + D8() + D8() + 36
                    Effects.VisibilityMod = -2

                    If zone = OverlandTerrainType.Desert Then
                        CurrentWeatherType = "Sandstorm"
                        RainbowChance = RainbowChanceByPrecipType.Sandstorm
                        Effects.Sky = CloudState.Clear
                    Else
                        CurrentWeatherType = "Blizzard"
                        RainbowChance = RainbowChanceByPrecipType.Blizzard
                        Effects.Sky = CloudState.Cloudy

                        If Effects.Temperature > 20 Then Effects.Temperature = D20()
                    End If

                Case 6 To 10        ' SNOWSTORM, HEAVY
                    Effects.WindSpeed = D10() + D10() + D10()
                    Effects.VisibilityMod = -2
                    Effects.Sky = CloudState.Cloudy

                    CurrentWeatherType = "Heavy Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.HeavySnowstorm

                    If Effects.Temperature > 25 Then Effects.Temperature = D20() + 5

                Case 11 To 20       ' SNOWSTORM
                    Effects.WindSpeed = D6() + D6() + D6() + D6()
                    Effects.VisibilityMod = -1
                    Effects.Sky = CloudState.Cloudy

                    CurrentWeatherType = "Snowstorm"
                    RainbowChance = RainbowChanceByPrecipType.Snowstorm

                    If Effects.Temperature > 35 Then Effects.Temperature = D20() + 15

                Case 21 To 25       ' SLEETSTORM
                    Effects.WindSpeed = D10() + D10() + D10()
                    Effects.VisibilityMod = -1
                    Effects.Sky = CloudState.Cloudy

                    CurrentWeatherType = "Sleetstorm"
                    RainbowChance = RainbowChanceByPrecipType.Sleet

                    If Effects.Temperature > 35 Then Effects.Temperature = D20() + 15

                Case 26 To 27       ' HAILSTORM
                    'No Effect on Visibility
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        Effects.WindSpeed = D10() + D10() + D10() + D10()
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Hailstorm"
                        RainbowChance = RainbowChanceByPrecipType.Hail

                        If Effects.Temperature > 65 Then Effects.Temperature = D20() + D20() + D20() + 5
                    End If

                Case 28 To 30       ' FOG, HEAVY
                    Effects.VisibilityMod = -3
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        Effects.WindSpeed = D20()
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Heavy Fog"
                        RainbowChance = RainbowChanceByPrecipType.HeavyFog

                        If Effects.Temperature < 20 Or Effects.Temperature > 60 Then Effects.Temperature = 20 + D20() + D20()
                    End If

                Case 31 To 38       ' FOG
                    Effects.VisibilityMod = -2
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        Effects.WindSpeed = D10()
                        CurrentWeatherType = "Fog"
                        RainbowChance = RainbowChanceByPrecipType.LightFog

                        If Effects.Temperature < 30 Or Effects.Temperature > 70 Then Effects.Temperature = 30 + D20() + D20()
                    End If

                Case 39 To 40       ' MIST
                    Effects.WindSpeed = D10()
                    Effects.Sky = CloudState.Cloudy
                    CurrentWeatherType = "Mist"
                    RainbowChance = RainbowChanceByPrecipType.Mist

                    If Effects.Temperature < 30 Then Effects.Temperature = 30

                Case 41 To 45       ' DRIZZLE
                    Effects.WindSpeed = D20()
                    Effects.Sky = CloudState.Cloudy
                    CurrentWeatherType = "Drizzle"
                    RainbowChance = RainbowChanceByPrecipType.Drizzle

                    If Effects.Temperature < 25 Then Effects.Temperature = 25

                Case 46 To 60       ' RAINSTORM, LIGHT
                    Effects.WindSpeed = D20()
                    Effects.Sky = CloudState.Cloudy
                    CurrentWeatherType = "Light Rain"
                    RainbowChance = RainbowChanceByPrecipType.LightRain

                    If Effects.Temperature < 25 Then Effects.Temperature = 25

                Case 61 To 70       ' RAINSTORM, HEAVY
                    Effects.VisibilityMod = -1
                    Effects.WindSpeed = D12() + D12() + 10
                    Effects.Sky = CloudState.Cloudy
                    CurrentWeatherType = "Heavy Rain"
                    RainbowChance = RainbowChanceByPrecipType.HeavyRain

                    If Effects.Temperature < 25 Then Effects.Temperature = 25

                Case 71 To 84       ' THUNDERSTORM
                    Effects.VisibilityMod = -1
                    Effects.WindSpeed = D10() + D10() + D10() + D10()
                    Effects.Sky = CloudState.Cloudy
                    CurrentWeatherType = "Thunderstorm"
                    RainbowChance = RainbowChanceByPrecipType.ThunderStorm

                    If Effects.Temperature < 30 Then Effects.Temperature = 30

                Case 85 To 89       ' TROPICAL STORM
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        Effects.VisibilityMod = -1
                        Effects.WindSpeed = D12() + D12() + 10
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain

                        If Effects.Temperature < 25 Then Effects.Temperature = 25
                    Else
                        Effects.VisibilityMod = -2
                        Effects.WindSpeed = D12() + D12() + D12() + 30
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Tropical Storm"
                        RainbowChance = RainbowChanceByPrecipType.TropicalStorm

                        If Effects.Temperature < 40 Then Effects.Temperature = 40
                    End If

                Case 90 To 94       ' MONSOON
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    ElseIf zone = OverlandTerrainType.Plains Then
                        ' Convert to Heavy Rainstorm
                        Effects.VisibilityMod = -1
                        Effects.WindSpeed = D12() + D12() + 10
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Heavy Rain"
                        RainbowChance = RainbowChanceByPrecipType.HeavyRain

                        If Effects.Temperature < 25 Then Effects.Temperature = 25
                    Else
                        Effects.VisibilityMod = -3
                        Effects.WindSpeed = D10() + D10() + D10() + D10() + D10() + D10()
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Monsoon"
                        RainbowChance = RainbowChanceByPrecipType.Monsoon

                        If Effects.Temperature < 55 Then Effects.Temperature = 55
                    End If

                Case 95 To 97       ' GALE
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Clear
                        CurrentWeatherType = "Sunny"
                        RainbowChance = 0
                    Else
                        Effects.VisibilityMod = -3
                        Effects.WindSpeed = D8() + D8() + D8() + D8() + D8() + D8() + 40
                        Effects.Sky = CloudState.PartlyCloudy
                        CurrentWeatherType = "Gale"
                        RainbowChance = RainbowChanceByPrecipType.Gale

                        If Effects.Temperature < 40 Then Effects.Temperature = 40
                    End If

                Case 98 To 100      ' HURRICANE
                    If zone = OverlandTerrainType.Desert Then
                        Effects.WindSpeed = D4()
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Cloudy"
                        RainbowChance = 0
                    Else
                        Effects.VisibilityMod = -3
                        Effects.WindSpeed = D10() + D10() + D10() + D10() + D10() + D10() + D10() + 70
                        Effects.Sky = CloudState.Cloudy
                        CurrentWeatherType = "Hurricane"
                        RainbowChance = RainbowChanceByPrecipType.Hurricane

                        If Effects.Temperature < 55 Then Effects.Temperature = 55
                    End If

            End Select
        End Sub

        Private Shared Sub CheckForRainbow()
            If D100() <= RainbowChance Then Effects.Rainbow = True
        End Sub

        Private Shared Sub AdjustmentsForWindSpeed()
            ' TODO: Some of these windspeed adjustments require modification to main loop and avatar classes

            Select Case Effects.WindSpeed
                Case < 30
                    ' normal
                    Effects.MoveMod = 1.0
                Case 30 To 44
                    ' no torches
                    ' missiles 1/2 range, -1 TH
                    Effects.MoveMod = 1.25

                Case 45 To 59
                    ' no torches
                    ' no fires
                    ' missiles 1/4 range, -3 TH
                    Effects.MoveMod = 1.5

                Case 60 To 74
                    ' no torches
                    ' no fires
                    ' small trees uprooted
                    ' no missile fire
                    ' nomagical melee at -1 TH
                    ' AC Dex Bonus cancelled
                    Effects.MoveMod = 1.75

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
                    Effects.MoveMod = 0

            End Select
        End Sub

        Private Shared Sub ResetWeatherEffectsForNewTurn()
            Effects.WindSpeed = 0
            Effects.VisibilityMod = 0
            Effects.MoveMod = 0
            Effects.PreciptationAmount = 0
            Effects.Rainbow = False
            Effects.Temperature = 0
            Effects.WindSpeed = 0
            Effects.WindDirection = 0
        End Sub

        Public Structure Effects
            Public Shared Sky As CloudState
            Public Shared MoveMod As Int16
            Public Shared PreciptationAmount As Int16
            Public Shared Rainbow As Boolean
            Public Shared Temperature As Int16
            Public Shared VisibilityMod As Int16
            Public Shared WindDirection As Heading
            Public Shared WindSpeed As Int16
        End Structure




    End Class

End Namespace

