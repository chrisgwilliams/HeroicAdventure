Imports HA.Common

Public Class TimeKeeper

    Const COMBATROUND As Int16 = 1
    Const OUTDOORTURN As Int16 = 10

    Const DAWN As DateTime = #6:25:01 AM#
    Const DAY As DateTime = #7:25:01 AM#
    Const DUSK As DateTime = #8:00:00 PM#
    Const NIGHT As DateTime = #8:50:00 PM#

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
    'Bloody sweat corruption	                 +5
    'Talents (Quick, Very Quick, Greased Lightning)	+2, +3, +4 for 9 total
    'Athletics Skill	                   up To +8
    'Raven-born	                                +10
    'Dexterity	                                 +1 for every 2 points above 17
    'Very light corruption	                    +20
    'Wish (temporary only)	                   +100-200



    Friend Shared Sub InitializeTimeKeeper()

    End Sub



End Class
