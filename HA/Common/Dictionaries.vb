Imports System.Collections.Generic

Namespace Common

    Module Dictionaries
        Public WeatherPhrases As Dictionary(Of PrecipitationType, String)

        Public Sub InitializeDictionaries()
            WeatherPhrases.Add(PrecipitationType.None, "Sunny")

            WeatherPhrases.Add(PrecipitationType.Snowstorm, "Snow Storm")
            WeatherPhrases.Add(PrecipitationType.HeavySnowstorm, "Heavy Snowstorm")
            WeatherPhrases.Add(PrecipitationType.Blizzard, "Blizzard")
            WeatherPhrases.Add(PrecipitationType.HeavyBlizzard, "Heavy Blizzard")

            WeatherPhrases.Add(PrecipitationType.Drizzle, "Drizzle")
            WeatherPhrases.Add(PrecipitationType.LightRain, "Light Rain")
            WeatherPhrases.Add(PrecipitationType.Sleet, "Sleet")
            WeatherPhrases.Add(PrecipitationType.HeavyRain, "Heavy Rain")
            WeatherPhrases.Add(PrecipitationType.ThunderStorm, "Thunderstorm")
            WeatherPhrases.Add(PrecipitationType.Monsoon, "Monsoon")
            WeatherPhrases.Add(PrecipitationType.TropicalStorm, "Tropical Storm")
            WeatherPhrases.Add(PrecipitationType.Hurricane, "Hurricane")

            WeatherPhrases.Add(PrecipitationType.Gale, "Gale")
            WeatherPhrases.Add(PrecipitationType.Hail, "Hail")

            WeatherPhrases.Add(PrecipitationType.Mist, "Mist")
            WeatherPhrases.Add(PrecipitationType.LightFog, "Light Fog")
            WeatherPhrases.Add(PrecipitationType.HeavyFog, "Heavy Fog")

            WeatherPhrases.Add(PrecipitationType.Sandstorm, "Sandstorm")
            WeatherPhrases.Add(PrecipitationType.HeavySandstorm, "Heavy Sandstorm")
        End Sub

    End Module
End Namespace
