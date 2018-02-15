Namespace Common
    Public Class Helper

#Region " Helper Functions "

        Public Shared Function GetBase(ByVal Skill As String) As Int16
            Select Case Skill

                Case "Appraise" : Return AbilityMod(TheHero.EIntelligence)
                Case "Awareness" : Return AbilityMod(TheHero.EWisdom)
                Case "Backstab" : Return AbilityMod(TheHero.EDexterity)
                Case "Climbing" : Return AbilityMod(TheHero.EStrength)
                Case "Concentration" : Return AbilityMod(TheHero.EIntelligence)
                Case "Disable Trap" : Return AbilityMod(TheHero.EDexterity)
                Case "Find Weakness" : Return AbilityMod(TheHero.EIntelligence)
                Case "First Aid" : Return AbilityMod(TheHero.EWisdom)
                Case "Healing" : Return AbilityMod(TheHero.EConstitution)
                Case "Hide" : Return AbilityMod(TheHero.EDexterity)
                Case "Listen" : Return AbilityMod(TheHero.EWisdom)
                Case "Literacy" : Return AbilityMod(TheHero.EIntelligence)
                Case "Mining" : Return AbilityMod(TheHero.EWisdom)
                Case "Pick Locks" : Return AbilityMod(TheHero.EDexterity)
                Case "Pick Pockets" : Return AbilityMod(TheHero.EDexterity)
                Case "Search" : Return AbilityMod(TheHero.EIntelligence)
                Case "Spellcraft" : Return AbilityMod(TheHero.EIntelligence)
                Case "Survival" : Return AbilityMod(TheHero.EConstitution)
                Case "Swimming" : Return AbilityMod(TheHero.EStrength)

            End Select
        End Function

        <DebuggerStepThrough()> Public Shared Function GetStat(ByVal s As PCStats) As String
            Return s.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetRace(ByVal r As Race) As String
            If r = 0 Then Return "" Else Return r.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetClass(ByVal c As PCClass) As String
            If c = 0 Then Return "" Else Return c.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetWeapon(ByVal w As WeaponType) As String
            Return w.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetArmor(ByVal a As ArmorType) As String
            Return a.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetGender(ByVal g As Avatar.Sex) As String
            If g = 0 Then Return "" Else Return g.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetPronoun(ByVal g As Avatar.Sex) As String
            Select Case g
                Case Avatar.Sex.female
                    Return "she"
                Case Avatar.Sex.male
                    Return "he"
                Case Else
                    Return "it"
            End Select
        End Function
        <DebuggerStepThrough()> Public Shared Function GetColor(ByVal c As ColorList) As String
            Return c.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function GetItem(ByVal i As ItemType) As String
            Return i.ToString
        End Function
        <DebuggerStepThrough()> Public Shared Function AbilityMod(ByVal stat As Integer) As Integer
            Return Math.Floor(stat / 2) - 5
        End Function
        <DebuggerStepThrough()> Public Shared Function GetTrap(ByVal t As TrapType) As String
            Return t.ToString
        End Function

#End Region

    End Class
End Namespace