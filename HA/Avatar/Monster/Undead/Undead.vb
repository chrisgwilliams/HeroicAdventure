#Region "    -- Undead Subclass and Inherited Subclasses "

' inherits from Monster
<System.Diagnostics.DebuggerStepThrough()> Public MustInherit Class Undead
    Inherits Monster

    ' undead are immune to fear, sleep and charm
    Public Shadows ReadOnly Property Fear() As Integer
        Get
            Fear = 0
        End Get
    End Property
    Public Shadows ReadOnly Property SleepResist() As Integer
        Get
            SleepResist = 100
        End Get
    End Property
    Public Shadows ReadOnly Property CharmResist() As Integer
        Get
            CharmResist = 100
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub
End Class

#End Region

