
Imports HA.Common

Public MustInherit Class NPC
    ' see Hero and Avatar classes for base stats, etc
    Inherits Monster

    Public Property Stranger() As Boolean

    Public Property QuestMessage() As String
    Public Property QuestUnresolved() As String
    Public Property QuestResolved() As String
    Public Property HomeMap() As Town

    Public Sub New()
        MyBase.New()

        Stranger = True
        Attitude = Disposition.Neutral
    End Sub

End Class