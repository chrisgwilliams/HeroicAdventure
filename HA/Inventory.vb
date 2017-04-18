Imports System.Console
Imports System.Text
Imports System.Text.RegularExpressions

Imports HA.Common

Module Inventory
#Region " Inventory & Backpack Subs and Functions "

	Private Sub EquippedLocationLayout()

		Clear()
		WriteAt(0, 0, "------------------------------- Readied Equipment ------------------------------", ConsoleColor.DarkYellow)

		WriteAt(68, 2, "Weight", ConsoleColor.DarkYellow)

		WriteAt(2, 4, "] Head         |", ConsoleColor.DarkYellow)
		WriteAt(2, 5, "] Neck         |", ConsoleColor.DarkYellow)
		WriteAt(2, 6, "] Cloak        |", ConsoleColor.DarkYellow)
		WriteAt(2, 7, "] Girdle       |", ConsoleColor.DarkYellow)
		WriteAt(2, 8, "] Armor        |", ConsoleColor.DarkYellow)
		WriteAt(2, 9, "] Left Hand    |", ConsoleColor.DarkYellow)
		WriteAt(2, 10, "] Right Hand   |", ConsoleColor.DarkYellow)
		WriteAt(2, 11, "] Left Ring    |", ConsoleColor.DarkYellow)
		WriteAt(2, 12, "] Right Ring   |", ConsoleColor.DarkYellow)
		WriteAt(2, 13, "] Gloves       |", ConsoleColor.DarkYellow)
		WriteAt(2, 14, "] Bracers      |", ConsoleColor.DarkYellow)
		WriteAt(2, 15, "] Boots        |", ConsoleColor.DarkYellow)
		WriteAt(2, 16, "] Missle Weapon|", ConsoleColor.DarkYellow)
		WriteAt(2, 17, "] Missles      |", ConsoleColor.DarkYellow)
		WriteAt(2, 18, "] Tool         |", ConsoleColor.DarkYellow)

		' display the a - o menu options in WHITE
		Dim intCtr As Integer
		For intCtr = 0 To 14
			WriteAt(1, intCtr + 4, Chr(Asc("a") + intCtr))
		Next


		' bottom border and key choices
		WriteAt(0, 21, "--------------------------------------------------------------------------------", ConsoleColor.DarkYellow)
		WriteAt(7, 23, "[a-o] equip / remove item - [v]iew backpack - [z] return to game", ConsoleColor.DarkYellow)

		' overwrite the menu choice letters of the bottom key choices in WHITE
		WriteAt(8, 23, "a")
		WriteAt(10, 23, "o")
		WriteAt(36, 23, "v")
		WriteAt(54, 23, "z")

	End Sub

	Friend Sub InventoryScreen()
		' build the screen first, then populate equipped items as relevant
		EquippedLocationLayout()

		' Each of these lines check to see if something is equipped before displaying the item.
		' WriteItem() only needs the Item and Y index, unlike WriteAt() which accepts a string and x,y coords.
		If Not TheHero.Equipped.Helmet Is Nothing Then WriteItem(TheHero.Equipped.Helmet, 4)
		If Not TheHero.Equipped.Neck Is Nothing Then WriteItem(TheHero.Equipped.Neck, 5)
		If Not TheHero.Equipped.Cloak Is Nothing Then WriteItem(TheHero.Equipped.Cloak, 6)
		If Not TheHero.Equipped.Girdle Is Nothing Then WriteItem(TheHero.Equipped.Girdle, 7)
		If Not TheHero.Equipped.Armor Is Nothing Then WriteItem(TheHero.Equipped.Armor, 8)
		If Not TheHero.Equipped.LeftRing Is Nothing Then WriteItem(TheHero.Equipped.LeftRing, 11)
		If Not TheHero.Equipped.RightRing Is Nothing Then WriteItem(TheHero.Equipped.RightRing, 12)
		If Not TheHero.Equipped.Gloves Is Nothing Then WriteItem(TheHero.Equipped.Gloves, 13)
		If Not TheHero.Equipped.Bracers Is Nothing Then WriteItem(TheHero.Equipped.Bracers, 14)
		If Not TheHero.Equipped.Boots Is Nothing Then WriteItem(TheHero.Equipped.Boots, 15)
		If Not TheHero.Equipped.MissleWeapon Is Nothing Then WriteItem(TheHero.Equipped.MissleWeapon, 16)
		If Not TheHero.Equipped.Tool Is Nothing Then WriteItem(TheHero.Equipped.Tool, 18)


		' Left and Right Hand are a bit more complex and don't rely upon WriteItem() although I 
		' should probably stick this functionality in there just to clean things up.
		If Not TheHero.Equipped.LeftHand Is Nothing Then
			Dim str As String = TheHero.Equipped.LeftHand.name
			Dim Modifier As Integer = Helper.AbilityMod(TheHero.EStrength)

			If TheHero.Equipped.LeftHand.Type = ItemType.Weapon Then
				str += " (" & TheHero.Equipped.LeftHand.damage
				If Modifier > 0 Then
					str += "+" & Modifier
				ElseIf Modifier < 0 Then
					str += Modifier
				End If
				str += ")"
			ElseIf TheHero.Equipped.LeftHand.Type = ItemType.Shield Then
				str += String.Format(" (AC +{0})", TheHero.Equipped.LeftHand.acbonus)
			End If
			WriteAt(19, 9, str)
			WriteAt(70, 9, TheHero.Equipped.LeftHand.weight)

			'ToDo: add code to handle checking for two handed weapons/items and clearing out the other hand.
			' items removed from "other" hand should be returned to inventory.
		End If

		If Not TheHero.Equipped.RightHand Is Nothing Then
			Dim str As String = TheHero.Equipped.RightHand.name
			Dim Modifier As Integer = Helper.AbilityMod(TheHero.EStrength)

			If TheHero.Equipped.RightHand.Type = ItemType.Weapon Then
				str += " (" & TheHero.Equipped.RightHand.damage
				If Modifier > 0 Then
					str += "+" & Modifier
				ElseIf Modifier < 0 Then
					str += CStr(Modifier)
				End If
				str += ")"
			ElseIf TheHero.Equipped.RightHand.Type = ItemType.Shield Then
				str += String.Format(" (AC +{0})", TheHero.Equipped.RightHand.acbonus)
			End If
			WriteAt(19, 10, str)
			WriteAt(70, 10, TheHero.Equipped.RightHand.weight)
		End If


		' Missiles are also complex and don't rely upon WriteItem() although I should probably 
		' stick this functionality in there too, just to clean things up.
		If Not TheHero.Equipped.Missles Is Nothing Then
			WriteAt(19, 17, String.Format("{0} (Qty: {1})", TheHero.Equipped.Missles.name, CInt(TheHero.Equipped.Missles.quantity)))
			WriteAt(70, 17, CType(TheHero.Equipped.Missles.weight, Single) * CInt(TheHero.Equipped.Missles.quantity))
		End If

	End Sub

	Private Sub WriteItem(ByVal item As Object, _
						  ByVal Ypos As Integer)

		'If item.stataffected > 0 Then
		'	WriteAt(19, Ypos, String.Format("{0} ({1} +{2})", item.name, Helper.GetStat(item.stataffected), item.statbonus))
		'Else
		If item.acbonus > 0 Then
			WriteAt(19, Ypos, String.Format("{0} (AC +{1})", item.name, item.acbonus))
		Else
			WriteAt(19, Ypos, item.name)
		End If

		WriteAt(70, Ypos, item.weight)

	End Sub

	Private Function ShowAllItems(ByVal intCtr As Integer, _
								  ByVal itemtype As Integer, _
								  ByRef yPos As Integer, _
								  ByRef ItemArray As ArrayList, _
								  ByVal MaxRows As Integer, _
								  ByRef startletter As String, _
								  ByRef itemcount As Integer, _
								  ByVal category As String, _
						 Optional ByVal action As String = "") As String

		ShowAllItems = ""

		Dim thing As Object, _
			intQty As Integer, strDash As String = "", _
			strSelection As ConsoleKeyInfo, _
			ok As Boolean, i As Integer, _
			intTitlePos As Integer, intTypeCtr As Integer = 0

		itemcount += intCtr

		For i = 1 To category.Length
			strDash = strDash & "-"
		Next

		If intCtr > 0 Then
			' yPos will only equal 5 if it's below an empty category, which 
			' shouldn't be displayed. this was an error that popped up on the
			' second + page(s) of backpack
			If yPos = 5 Then
				WriteAt(0, 2, CLEARSPACE, ConsoleColor.DarkYellow)
				WriteAt(0, 3, CLEARSPACE, ConsoleColor.DarkYellow)
				yPos = 2
			End If

			WriteAt(0, yPos, category, ConsoleColor.DarkYellow)
			intTitlePos = yPos

			yPos += 1
			WriteAt(0, yPos, strDash, ConsoleColor.DarkYellow)

			yPos += 1
			For Each thing In TheHero.Equipped.BackPack
				If thing.type = itemtype Then
					intQty = thing.quantity
					Dim strItem As New StringBuilder

					strItem.Append(String.Format("{0}] {1}", startletter, thing.Name))
					If intQty > 1 Then strItem.Append(String.Format(" (Qty: {0})", thing.quantity))
					strItem.Append("               ")
					WriteAt(1, yPos, strItem.ToString)
					startletter = Chr(Asc(startletter) + 1)

					yPos += 1
					ItemArray.Add(thing)
					intTypeCtr += 1
				End If

				' wrap the list
				If yPos >= MaxRows And action = "" Then
					If intTypeCtr > 0 Then
						' nothing
					Else
						If yPos - intTitlePos = 2 Then
							WriteAt(0, intTitlePos, CLEARSPACE)
							WriteAt(0, intTitlePos + 1, CLEARSPACE)
							itemcount -= 1
						End If
					End If

					WriteAt(0, MaxRows + 2, "Press z to return to main inventory screen.")
					'check for more items after this point

					Dim plus As Boolean
					If itemcount < TheHero.Equipped.BackPack.Count Then
						WriteAt(44, MaxRows + 2, "Press + for more.")
						plus = True
					End If

					ok = False
					Do While Not ok
						strSelection = ReadKey()
						Select Case strSelection.KeyChar.ToString
							Case "z"
								Return 9999
							Case "+"
								If plus Then
									ok = True
									yPos = 2
									For intCtr = yPos To MaxRows
										WriteAt(0, intCtr, CLEARSPACE)
									Next

									WriteAt(0, yPos, category, ConsoleColor.DarkYellow)
									yPos += 1
									WriteAt(0, yPos, strDash, ConsoleColor.DarkYellow)
									yPos += 1
								End If
						End Select
					Loop
				End If
			Next
			yPos += 1

		End If

	End Function

	Private Function CheckBackpack(ByVal item As Integer) As Integer
		' This checks to see if there are ANY of a specific item type in the backpack

		Dim thing As Object, intCtr As Integer = 0

		For Each thing In TheHero.Equipped.BackPack
			If thing.type = item Then
				intCtr += 1
			End If
		Next

		Return intCtr

	End Function

	Friend Function ShowBackpack(ByVal action As String, Optional ByVal intCategory As Integer = 0) As Object
		Dim yPos As Integer = 2, _
			startLetter As String = "a", _
			itemArray As New ArrayList, _
			strSelection As ConsoleKeyInfo, _
			ok As Boolean, _
			intQty As Integer, _
			MaxRows As Integer = 21, _
			itemcount As Integer

		Clear()

		WriteAt(0, 0, "---------------------------------- My Backpack ---------------------------------")

		Select Case intCategory
			Case 0 ' show EVERYTHING in the backpack
				Dim thing As Object

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Helmet), ItemType.Helmet, yPos, itemArray, MaxRows, startLetter, itemcount, "Helmets:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Neck), ItemType.Neck, yPos, itemArray, MaxRows, startLetter, itemcount, "Amulets & Talismans:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Cloak), ItemType.Cloak, yPos, itemArray, MaxRows, startLetter, itemcount, "Cloaks:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Girdle), ItemType.Girdle, yPos, itemArray, MaxRows, startLetter, itemcount, "Girdles:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Armor), ItemType.Armor, yPos, itemArray, MaxRows, startLetter, itemcount, "Armor:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Weapon), ItemType.Weapon, yPos, itemArray, MaxRows, startLetter, itemcount, "Weapons:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Shield), ItemType.Shield, yPos, itemArray, MaxRows, startLetter, itemcount, "Shields:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Gloves), ItemType.Gloves, yPos, itemArray, MaxRows, startLetter, itemcount, "Gloves:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Bracers), ItemType.Bracers, yPos, itemArray, MaxRows, startLetter, itemcount, "Bracers:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Boots), ItemType.Boots, yPos, itemArray, MaxRows, startLetter, itemcount, "Boots:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.MissleWeapon), ItemType.MissleWeapon, yPos, itemArray, MaxRows, startLetter, itemcount, "Missle Weapons:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Missles), ItemType.Missles, yPos, itemArray, MaxRows, startLetter, itemcount, "Missles:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Tool), ItemType.Tool, yPos, itemArray, MaxRows, startLetter, itemcount, "Tools:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Ring), ItemType.Ring, yPos, itemArray, MaxRows, startLetter, itemcount, "Rings:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Potion), ItemType.Potion, yPos, itemArray, MaxRows, startLetter, itemcount, "Potions:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Scroll), ItemType.Scroll, yPos, itemArray, MaxRows, startLetter, itemcount, "Scrolls:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Book), ItemType.Book, yPos, itemArray, MaxRows, startLetter, itemcount, "Books:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Wand), ItemType.Wand, yPos, itemArray, MaxRows, startLetter, itemcount, "Wands:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				ShowBackpack = ShowAllItems(CheckBackpack(ItemType.Gem), ItemType.Gem, yPos, itemArray, MaxRows, startLetter, itemcount, "Gems:", action)
				If ShowBackpack = "9999" Then
					Exit Function
				End If

				Dim gpCount As Integer = 0
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Gold Then
						gpCount += thing.quantity
					End If
				Next

				TheHero.GP = gpCount

				If TheHero.GP > 0 Then
					If yPos = 5 Then yPos = 2
					WriteAt(0, yPos, CLEARSPACE)
					WriteAt(0, yPos + 1, CLEARSPACE)

					WriteAt(0, yPos, "Gold:", ConsoleColor.DarkYellow)
					yPos += 1
					WriteAt(0, yPos, "-----", ConsoleColor.DarkYellow)
					yPos += 1
					WriteAt(1, yPos, TheHero.GP & "GP               ")
					startLetter = Chr(Asc(startLetter) + 1)

					yPos += 1
					itemArray.Add(thing)
				End If

				If ShowBackpack = "9999" Then
					Exit Function
				End If

			Case ItemType.Helmet
				Dim thing As Object
				WriteAt(0, yPos, "Helmets:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Helmet Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(String.Format("{0}] {1}", startLetter, thing.Name))
						If intQty > 1 Then strItem.Append(String.Format(" (Qty: {0})", thing.quantity))
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Neck
				Dim thing As Object
				WriteAt(0, yPos, "Amulets & Talismans:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------------------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Neck Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(String.Format("{0}] {1}", startLetter, thing.Name))
						If intQty > 1 Then strItem.Append(String.Format(" (Qty: {0})", thing.quantity))
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Cloak
				Dim thing As Object
				WriteAt(0, yPos, "Cloaks:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "-------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Cloak Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(String.Format("{0}] {1}", startLetter, thing.Name))
						If intQty > 1 Then strItem.Append(String.Format(" (Qty: {0})", thing.quantity))
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Girdle
				Dim thing As Object
				WriteAt(0, yPos, "Girdles:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Girdle Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(String.Format("{0}] {1}", startLetter, thing.Name))
						If intQty > 1 Then strItem.Append(String.Format(" (Qty: {0})", thing.quantity))
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Armor
				Dim thing As Object
				WriteAt(0, yPos, "Armor:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Armor Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Weapon
				Dim thing As Object
				WriteAt(0, yPos, "Weapons:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Weapon Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next

				yPos += 2
				WriteAt(0, yPos, "Shields:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Shield Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Ring
				Dim thing As Object
				WriteAt(0, yPos, "Rings:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.Type = ItemType.Ring Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Gloves
				Dim thing As Object
				WriteAt(0, yPos, "Gloves:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "-------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Gloves Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Bracers
				Dim thing As Object
				WriteAt(0, yPos, "Bracers:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Bracers Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Boots
				Dim thing As Object
				WriteAt(0, yPos, "Boots:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Boots Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.MissleWeapon
				Dim thing As Object
				WriteAt(0, yPos, "Missle Weapons:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "---------------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.MissleWeapon Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Missles
				action = "throw"
				Dim thing As Object
				WriteAt(0, yPos, "Missles:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Missles Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Tool
				action = "use"
				Dim thing As Object
				WriteAt(0, yPos, "Tools:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Tool _
					Or thing.type = ItemType.Gem _
					Or thing.type = ItemType.Potion Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Potion
				action = "drink"
				Dim thing As Object
				WriteAt(0, yPos, "Potions:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "--------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Potion Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

			Case ItemType.Wand
				action = "zap"
				Dim thing As Object
				WriteAt(0, yPos, "Wands:", ConsoleColor.DarkYellow)
				yPos += 1
				WriteAt(0, yPos, "------", ConsoleColor.DarkYellow)
				yPos += 1

				WriteAt(1, yPos, "No items of this type.")
				For Each thing In TheHero.Equipped.BackPack
					If thing.type = ItemType.Wand Then
						intQty = thing.quantity
						Dim strItem As New StringBuilder

						strItem.Append(startLetter & "] " & thing.Name)
						If intQty > 1 Then strItem.Append(" (Qty: " & thing.quantity & ")")
						strItem.Append("               ")
						WriteAt(1, yPos, strItem.ToString)

						yPos += 1
						startLetter = Chr(Asc(startLetter) + 1)
						itemArray.Add(thing)
					End If
				Next
				yPos += 2

		End Select

		If itemArray.Count > 0 And (intCategory > 0 Or action = "drop") Then
			startLetter = Chr(Asc(startLetter) - 1)
			WriteAt(0, MaxRows + 2, "Please make a selection [a - " & startLetter & "] or z to exit.")
		Else
			If action = "drop" Then
				WriteAt(0, MaxRows + 2, "Press z to cancel drop and return to game.")
			Else
				WriteAt(0, MaxRows + 2, "Press z to return to main inventory screen.")
			End If
			WriteAt(44, MaxRows + 2, "                      ")
		End If

		' Use regular expressions to validate input
		Dim ValidInput As Match

		ok = False
		Do While Not ok
			strSelection = ReadKey()
			ValidInput = Regex.Match(strSelection.KeyChar.ToString, "[a-z]")

			' if they entered an appropriate value, then continue
			If ValidInput.Success Then
				If strSelection.KeyChar.ToString = "z" Then
					ShowBackpack = Nothing
					ok = True
				ElseIf intCategory > 0 Or action = "drop" Then
					Dim intItemIndex As Integer = Asc(strSelection.KeyChar.ToString) - Asc("a")

					If intItemIndex > itemArray.Count - 1 Then
						' user pressed a letter not onscreen, so do nothing
					Else
						ShowBackpack = itemArray(intItemIndex)

						' drink the potion, then remove it from the backpack
						If action = "drink" Then
							Dim strMessage As String

							TheHero.DrinkPotion(ShowBackpack.ptype)

							strMessage = ShowBackpack.message
							ShowBackpack = strMessage
						End If

						' drop the item, then remove it from the backpack
						If action = "drop" Then
							' place the item in the current tile Hero is occupying
							Dim item As ItemBase = itemArray(intItemIndex)

							If Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).itemcount = 0 Then
								Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items = New ArrayList
							End If

							Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).items.Add(item)
							Level(TheHero.LocX, TheHero.LocY, TheHero.LocZ).itemcount += 1
							ShowBackpack = "You drop the " & itemArray(intItemIndex).walkover & "."

							' subtract item weight from backpack on drop
							TheHero.BackpackWeight -= item.Weight
						End If

						' TODO: Should this be inside the drop block?
						If itemArray(intItemIndex).quantity = 1 Then
							TheHero.Equipped.BackPack.Remove(itemArray(intItemIndex))
							itemArray.RemoveAt(intItemIndex)

						ElseIf itemArray(intItemIndex).quantity > 1 Then
							TheHero.Equipped.BackPack.Remove(itemArray(intItemIndex))
							itemArray(intItemIndex).quantity -= 1
							TheHero.Equipped.BackPack.Add(itemArray(intItemIndex))
						End If

						ok = True
					End If
				End If
			End If
		Loop

	End Function

#End Region

End Module
