Imports System.Collections.Generic

Public Module Util
	Public Function Scramble(phrase As String) As String
		Dim rand As New Random()
		Dim newPhrase As String = ""
		Dim clist As New List(Of String)

		' break phrase into characters and add to list
		For i As Integer = 1 To Len(phrase)
			clist.Add(Mid(phrase.ToLower(), i, 1))
		Next

		' remove from list randomly and add to new string
		Do While clist.Count > 0
			Dim r As Integer = rand.Next(0, clist.Count)
			newPhrase &= clist(r)
			clist.RemoveAt(r)
		Loop
		
		Return newPhrase

	End Function

End Module
