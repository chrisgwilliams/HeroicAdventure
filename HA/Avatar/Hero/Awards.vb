Imports System.Collections.Generic
Imports System.Linq
Imports HA.Common

Public Class Awards

	Private AwardList As New List(Of Award)

	Public Sub QualifyAward(hero As Hero)
		If hero.TotalKills = 1 Then CreateAward("First Blood!", 5)
		If hero.TotalKills = 100 Then CreateAward("Angry Warrior", 10)
		If hero.TotalKills = 1000 Then CreateAward("Mass Murderer", 20)
		If hero.Cannibal Then CreateAward("C.H.U.D.", 5)
		If hero.TurnCount = 1000 Then CreateAward("Survivor", 10)

		If hero.GP >= 10000 Then CreateAward("Tycoon", 10)
		If hero.GP >= 50000 Then CreateAward("Scrooge", 10)

		' TODO: Future Awards / Achievements
		' Cannibal = eats corpse from same race
		' Garbage Disposal = eats 20 different corpse species
		' Packrat = xx number of unique items in inventory
		' Hoarder = xx number of same item


	End Sub

	Public Function LookUpAward(name As String) As Boolean

		Return AwardList.Any(Function(awardItem) awardItem.Name = name)

	End Function

	Public Sub CreateAward(name As String, points As Integer)
		Dim myAward As Award
		myAward.Name = name
		myAward.Points = points

		If Not LookUpAward(name) Then
			AwardList.Add(myAward)
		End If
	End Sub

End Class
