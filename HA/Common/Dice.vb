Namespace Common
	Module Dice

#Region " Dice Routines "

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D100(Optional cap As Integer = 100) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D20(Optional cap As Integer = 20) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D12(Optional cap As Integer = 12) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D10(Optional cap As Integer = 10) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D8(Optional cap As Integer = 8) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D6(Optional cap As Integer = 6) As Integer
			Return RND.Next(1, cap)
		End Function

		<System.Diagnostics.DebuggerStepThrough()> _
		Friend Function D4(Optional cap As Integer = 4) As Integer
			Return RND.Next(1, cap)
		End Function

#End Region

	End Module
End Namespace
