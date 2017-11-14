Imports HA.Common

Public Class AoIntellect
	Inherits Amulet

	Public Sub New()
		MyBase.New()

		Color = ColorList.Red
		Walkover = "garnet amulet"
		Name = "amulet of intellect"

		StatBonus = 1
		'TODO: add blessed / cursed / uncursed support
	End Sub

	Public Overrides Sub activate(whoIsActivating As Avatar)
        With whoIsActivating
            Select Case ItemState
                Case DivineState.Blessed
                    StatBonus += 1
                Case DivineState.Normal

                Case DivineState.Cursed
                    StatBonus = -1
            End Select
        End With

        whoIsActivating.IntMods += StatBonus
    End Sub

    Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        With whoIsDeactivating
            Select Case ItemState
                Case DivineState.Blessed
                    StatBonus += 1
                Case DivineState.Normal

                Case DivineState.Cursed
                    StatBonus = -1
            End Select
        End With

        whoIsDeactivating.IntMods -= StatBonus
    End Sub
End Class
