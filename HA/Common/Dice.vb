Imports System.Collections.Generic

Namespace Common

    Module Dice

#Region " Dice Routines "

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D100(Optional cap As Integer = 100) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D20(Optional cap As Integer = 20) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D12(Optional cap As Integer = 12) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D10(Optional cap As Integer = 10) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D8(Optional cap As Integer = 8) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D6(Optional cap As Integer = 6) As Integer
            Return RND.Next(1, cap)
        End Function

        <System.Diagnostics.DebuggerStepThrough()>
        Friend Function D4(Optional cap As Integer = 4) As Integer
            Return RND.Next(1, cap)
        End Function

#End Region

        Friend Function GetRandomListOfUniqueInts(NumberOfInts As Int16) As Short()
            Dim intList As HashSet(Of Int16) = New HashSet(Of Int16)
            Dim ctr As Int16

            Do Until ctr = NumberOfInts
                If intList.Add(RND.Next(1, NumberOfInts)) = True Then
                    ctr += 1
                End If
            Loop

            Dim intArray(NumberOfInts - 1) As Short
            intList.CopyTo(intArray)
            Return intArray

        End Function

    End Module
End Namespace
