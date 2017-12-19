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
        Friend Shared Function Update() As String
            Dim Message As String = ""

            Return Message
        End Function

    End Class

End Namespace

