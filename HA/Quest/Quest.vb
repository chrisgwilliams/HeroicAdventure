Imports HA.Common
Imports System.Collections.Generic

Public Class Quest

    Public Name As String
    Public Status As QuestStatus

    Public Source As NPC
    Public Destination As NPC

    ' Quest Types
    Public MeetingQuest As Boolean
    Public KillQuest As Boolean
    Public FedExQuest As Boolean

    ' Kill Quest
    Public KillType As Monster
    Public KillGoal As Int16
    Public KillCount As Int16

    ' FedEx Quest
    Public PackageList As List(Of ItemBase)

    ' Rewards
    Public XPReward As Int16
    Public GPReward As Int16
    Public ItemReward As ItemBase

    ' Conversation
    Public IntroMsg() As String
    Public UnresolvedMsg() As String
    Public ResolvedMsg() As String

    Public Sub New()
        PackageList = New List(Of ItemBase)


    End Sub

End Class
