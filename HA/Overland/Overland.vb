Imports HA.Common
Imports RND = DBuild.MersenneTwister
Imports System.Console
Imports HA.Common.Helper

Module Overland
#Region " Module Level Variables "
	Friend OverlandMap(73, 18) As OverlandCell

#End Region

#Region " Initialization Routines "
	Public Sub InitializeOverland()
		Dim intCtr As Int16

		' bottom row (17) of map
		For intCtr = 0 To 71
			OverlandMap(intCtr, 17).TerrainType = TerrainType.Impassable
		Next

		' next row (16) of map
		OverlandMap(0, 16).TerrainType = TerrainType.Impassable

		For intCtr = 1 To 4
			OverlandMap(intCtr, 16).TerrainType = TerrainType.Road
		Next
		For intCtr = 5 To 70
			OverlandMap(intCtr, 16).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(71, 16).TerrainType = TerrainType.Impassable

		' next row (15) of map
		OverlandMap(0, 15).TerrainType = TerrainType.Impassable
		OverlandMap(1, 15).TerrainType = TerrainType.Impassable
		OverlandMap(2, 15).TerrainType = TerrainType.Mountain
		OverlandMap(3, 15).TerrainType = TerrainType.Mountain
		OverlandMap(4, 15).TerrainType = TerrainType.Road
		For intCtr = 5 To 9
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 10 To 22
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Forest
		Next
		For intCtr = 23 To 39
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 40 To 49
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Forest
		Next
		For intCtr = 50 To 57
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Plains
		Next
		OverlandMap(58, 15).TerrainType = TerrainType.Mountain
		OverlandMap(59, 15).TerrainType = TerrainType.Mountain
		OverlandMap(60, 15).TerrainType = TerrainType.Plains
		OverlandMap(61, 15).TerrainType = TerrainType.Plains
		OverlandMap(62, 15).TerrainType = TerrainType.Hills
		For intCtr = 63 To 69
			OverlandMap(intCtr, 15).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(70, 15).TerrainType = TerrainType.Impassable
		OverlandMap(71, 15).TerrainType = TerrainType.Impassable

		' next row (14) of map
		OverlandMap(0, 14).TerrainType = TerrainType.Void
		OverlandMap(1, 14).TerrainType = TerrainType.Impassable
		OverlandMap(2, 14).TerrainType = TerrainType.Mountain
		OverlandMap(3, 14).TerrainType = TerrainType.Mountain
		For intCtr = 4 To 7
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Road
		Next
		OverlandMap(8, 14).TerrainType = TerrainType.Town
		For intCtr = 9 To 25
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Forest
		Next
		For intCtr = 26 To 28
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(29, 14).TerrainType = TerrainType.Special
		For intCtr = 30 To 33
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 34 To 45
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Forest
		Next
		For intCtr = 46 To 53
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Plains
		Next
		OverlandMap(54, 14).TerrainType = TerrainType.Mountain
		OverlandMap(55, 14).TerrainType = TerrainType.Mountain
		For intCtr = 56 To 61
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Plains
		Next

		' stonegate
		OverlandMap(58, 14).TerrainType = TerrainType.Town
		OverlandMap(59, 14).TerrainType = TerrainType.Mountain

		For intCtr = 62 To 65
			OverlandMap(intCtr, 14).TerrainType = TerrainType.Hills
		Next
		OverlandMap(66, 14).TerrainType = TerrainType.Mountain
		OverlandMap(67, 14).TerrainType = TerrainType.Mountain
		OverlandMap(68, 14).TerrainType = TerrainType.Mountain
		OverlandMap(69, 14).TerrainType = TerrainType.Impassable
		OverlandMap(70, 14).TerrainType = TerrainType.Impassable

		' next row (13) of map
		OverlandMap(0, 13).TerrainType = TerrainType.Void
		OverlandMap(1, 13).TerrainType = TerrainType.Impassable
		OverlandMap(2, 13).TerrainType = TerrainType.Impassable
		For intCtr = 3 To 6
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 7 To 14
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Forest
		Next
		For intCtr = 15 To 24
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Plains
		Next
		For intCtr = 25 To 28
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 29 To 46
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Forest
		Next
		For intCtr = 47 To 54
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Plains
		Next
		For intCtr = 55 To 59
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(60, 13).TerrainType = TerrainType.Plains
		OverlandMap(61, 13).TerrainType = TerrainType.Plains
		OverlandMap(62, 13).TerrainType = TerrainType.Plains
		For intCtr = 63 To 66
			OverlandMap(intCtr, 13).TerrainType = TerrainType.Hills
		Next
		OverlandMap(67, 13).TerrainType = TerrainType.Mountain
		OverlandMap(68, 13).TerrainType = TerrainType.Mountain
		OverlandMap(69, 13).TerrainType = TerrainType.Impassable
		OverlandMap(70, 13).TerrainType = TerrainType.Impassable

		' next row (12) of map
		OverlandMap(0, 12).TerrainType = TerrainType.Void
		OverlandMap(1, 12).TerrainType = TerrainType.Void
		OverlandMap(2, 12).TerrainType = TerrainType.Impassable
		OverlandMap(3, 12).TerrainType = TerrainType.Impassable
		OverlandMap(4, 12).TerrainType = TerrainType.Mountain
		OverlandMap(5, 12).TerrainType = TerrainType.Mountain
		OverlandMap(6, 12).TerrainType = TerrainType.Mountain
		For intCtr = 7 To 13
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Forest
		Next
		For intCtr = 14 To 25
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Plains
		Next
		For intCtr = 26 To 29
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 30 To 46
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Forest
		Next
		OverlandMap(47, 12).TerrainType = TerrainType.Hills
		OverlandMap(48, 12).TerrainType = TerrainType.Hills
		OverlandMap(49, 12).TerrainType = TerrainType.Plains
		OverlandMap(50, 12).TerrainType = TerrainType.Plains
		For intCtr = 51 To 58
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 59 To 63
			OverlandMap(intCtr, 12).TerrainType = TerrainType.Plains
		Next
		OverlandMap(64, 12).TerrainType = TerrainType.Hills
		OverlandMap(65, 12).TerrainType = TerrainType.Hills
		OverlandMap(66, 12).TerrainType = TerrainType.Hills
		OverlandMap(67, 12).TerrainType = TerrainType.Hills
		OverlandMap(68, 12).TerrainType = TerrainType.Mountain
		OverlandMap(69, 12).TerrainType = TerrainType.Mountain
		OverlandMap(70, 12).TerrainType = TerrainType.Impassable
		OverlandMap(71, 12).TerrainType = TerrainType.Impassable

		' next row (11) of map
		OverlandMap(0, 11).TerrainType = TerrainType.Void
		OverlandMap(1, 11).TerrainType = TerrainType.Impassable
		OverlandMap(2, 11).TerrainType = TerrainType.Impassable
		OverlandMap(3, 11).TerrainType = TerrainType.Mountain
		OverlandMap(4, 11).TerrainType = TerrainType.Mountain
		OverlandMap(5, 11).TerrainType = TerrainType.Mountain
		OverlandMap(6, 11).TerrainType = TerrainType.Mountain
		For intCtr = 7 To 11
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Hills
		Next
		For intCtr = 12 To 20
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Plains
		Next
		OverlandMap(21, 11).TerrainType = TerrainType.Special
		OverlandMap(22, 11).TerrainType = TerrainType.Mountain
		For intCtr = 23 To 26
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Plains
		Next
		OverlandMap(27, 11).TerrainType = TerrainType.Mountain
		OverlandMap(28, 11).TerrainType = TerrainType.Mountain
		For intCtr = 29 To 44
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Forest
		Next
		For intCtr = 45 To 48
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Hills
		Next
		For intCtr = 49 To 55
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 56 To 62
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Plains
		Next
		For intCtr = 63 To 66
			OverlandMap(intCtr, 11).TerrainType = TerrainType.Forest
		Next
		OverlandMap(67, 11).TerrainType = TerrainType.Hills
		OverlandMap(68, 11).TerrainType = TerrainType.Hills
		OverlandMap(69, 11).TerrainType = TerrainType.Hills
		OverlandMap(70, 11).TerrainType = TerrainType.Mountain
		OverlandMap(71, 11).TerrainType = TerrainType.Impassable
		OverlandMap(72, 11).TerrainType = TerrainType.Impassable

		' next row (10) of map
		OverlandMap(0, 10).TerrainType = TerrainType.Void
		OverlandMap(1, 10).TerrainType = TerrainType.Impassable
		OverlandMap(2, 10).TerrainType = TerrainType.Mountain
		OverlandMap(3, 10).TerrainType = TerrainType.Hills
		OverlandMap(4, 10).TerrainType = TerrainType.Hills
		OverlandMap(5, 10).TerrainType = TerrainType.Special
		OverlandMap(6, 10).TerrainType = TerrainType.Mountain
		OverlandMap(7, 10).TerrainType = TerrainType.Mountain
		OverlandMap(8, 10).TerrainType = TerrainType.Mountain
		For intCtr = 9 To 12
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Hills
		Next
		For intCtr = 13 To 19
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Plains
		Next
		For intCtr = 20 To 25
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 26 To 29
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Plains
		Next
		For intCtr = 30 To 43
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Forest
		Next
		OverlandMap(44, 10).TerrainType = TerrainType.Hills
		OverlandMap(45, 10).TerrainType = TerrainType.Hills
		OverlandMap(46, 10).TerrainType = TerrainType.Hills
		OverlandMap(47, 10).TerrainType = TerrainType.Mountain
		OverlandMap(48, 10).TerrainType = TerrainType.Mountain
		OverlandMap(49, 10).TerrainType = TerrainType.Special
		OverlandMap(50, 10).TerrainType = TerrainType.Mountain
		OverlandMap(51, 10).TerrainType = TerrainType.Mountain
		OverlandMap(52, 10).TerrainType = TerrainType.Water
		OverlandMap(53, 10).TerrainType = TerrainType.Water
		OverlandMap(54, 10).TerrainType = TerrainType.Water
		For intCtr = 55 To 60
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Plains
		Next
		For intCtr = 61 To 67
			OverlandMap(intCtr, 10).TerrainType = TerrainType.Forest
		Next
		OverlandMap(68, 10).TerrainType = TerrainType.Hills
		OverlandMap(69, 10).TerrainType = TerrainType.Hills
		OverlandMap(70, 10).TerrainType = TerrainType.Mountain
		OverlandMap(71, 10).TerrainType = TerrainType.Mountain
		OverlandMap(72, 10).TerrainType = TerrainType.Impassable

		' next row (9) of map
		OverlandMap(0, 9).TerrainType = TerrainType.Impassable
		OverlandMap(1, 9).TerrainType = TerrainType.Impassable
		OverlandMap(2, 9).TerrainType = TerrainType.Mountain
		OverlandMap(3, 9).TerrainType = TerrainType.Hills
		For intCtr = 4 To 8
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 9 To 17
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Hills
		Next
		For intCtr = 18 To 21
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Plains
		Next
		OverlandMap(22, 9).TerrainType = TerrainType.Mountain
		OverlandMap(23, 9).TerrainType = TerrainType.Mountain
		For intCtr = 24 To 31
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Plains
		Next
		For intCtr = 32 To 43
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Forest
		Next
		OverlandMap(44, 9).TerrainType = TerrainType.Hills
		OverlandMap(45, 9).TerrainType = TerrainType.Hills
		OverlandMap(46, 9).TerrainType = TerrainType.Hills
		For intCtr = 47 To 50
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 51 To 55
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Water
		Next
		For intCtr = 56 To 60
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Plains
		Next
		For intCtr = 61 To 67
			OverlandMap(intCtr, 9).TerrainType = TerrainType.Forest
		Next
		OverlandMap(68, 9).TerrainType = TerrainType.Hills
		OverlandMap(69, 9).TerrainType = TerrainType.Hills
		OverlandMap(70, 9).TerrainType = TerrainType.Hills
		OverlandMap(71, 9).TerrainType = TerrainType.Mountain
		OverlandMap(72, 9).TerrainType = TerrainType.Impassable

		' next row (8) of map
		OverlandMap(0, 8).TerrainType = TerrainType.Impassable
		OverlandMap(1, 8).TerrainType = TerrainType.Mountain
		OverlandMap(2, 8).TerrainType = TerrainType.Mountain
		OverlandMap(3, 8).TerrainType = TerrainType.Hills
		OverlandMap(4, 8).TerrainType = TerrainType.Hills
		OverlandMap(5, 8).TerrainType = TerrainType.Mountain
		OverlandMap(6, 8).TerrainType = TerrainType.Mountain
		For intCtr = 7 To 11
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Hills
		Next
		For intCtr = 12 To 17
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(18, 8).TerrainType = TerrainType.Hills
		OverlandMap(19, 8).TerrainType = TerrainType.Hills
		OverlandMap(20, 8).TerrainType = TerrainType.Plains
		OverlandMap(21, 8).TerrainType = TerrainType.Plains
		OverlandMap(22, 8).TerrainType = TerrainType.Plains
		For intCtr = 23 To 28
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 29 To 33
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Plains
		Next
		For intCtr = 34 To 37
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Forest
		Next
		OverlandMap(38, 8).TerrainType = TerrainType.Plains
		OverlandMap(39, 8).TerrainType = TerrainType.Plains
		OverlandMap(40, 8).TerrainType = TerrainType.Plains
		OverlandMap(41, 8).TerrainType = TerrainType.Special
		For intCtr = 42 To 45
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Forest
		Next
		OverlandMap(46, 8).TerrainType = TerrainType.Hills
		OverlandMap(47, 8).TerrainType = TerrainType.Hills
		OverlandMap(48, 8).TerrainType = TerrainType.Hills
		OverlandMap(49, 8).TerrainType = TerrainType.Mountain
		OverlandMap(50, 8).TerrainType = TerrainType.Mountain
		OverlandMap(51, 8).TerrainType = TerrainType.Mountain
		OverlandMap(52, 8).TerrainType = TerrainType.Water
		OverlandMap(53, 8).TerrainType = TerrainType.Water
		OverlandMap(54, 8).TerrainType = TerrainType.Water
		OverlandMap(55, 8).TerrainType = TerrainType.Plains
		OverlandMap(56, 8).TerrainType = TerrainType.Plains
		OverlandMap(57, 8).TerrainType = TerrainType.Plains
		For intCtr = 58 To 68
			OverlandMap(intCtr, 8).TerrainType = TerrainType.Forest
		Next
		OverlandMap(69, 8).TerrainType = TerrainType.Hills
		OverlandMap(70, 8).TerrainType = TerrainType.Mountain
		OverlandMap(71, 8).TerrainType = TerrainType.Mountain
		OverlandMap(72, 8).TerrainType = TerrainType.Impassable

		' next row (7) of map
		OverlandMap(0, 7).TerrainType = TerrainType.Impassable
		OverlandMap(1, 7).TerrainType = TerrainType.Mountain
		For intCtr = 2 To 7
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Hills
		Next
		For intCtr = 8 To 13
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(14, 7).TerrainType = TerrainType.Water
		OverlandMap(15, 7).TerrainType = TerrainType.Water
		OverlandMap(16, 7).TerrainType = TerrainType.Water
		OverlandMap(17, 7).TerrainType = TerrainType.Mountain
		OverlandMap(18, 7).TerrainType = TerrainType.Mountain
		OverlandMap(19, 7).TerrainType = TerrainType.Hills
		For intCtr = 20 To 23
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Plains
		Next
		For intCtr = 24 To 29
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 30 To 44
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Plains
		Next
		OverlandMap(45, 7).TerrainType = TerrainType.Hills
		OverlandMap(46, 7).TerrainType = TerrainType.Hills
		OverlandMap(47, 7).TerrainType = TerrainType.Hills
		For intCtr = 48 To 51
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 52 To 57
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Plains
		Next
		For intCtr = 58 To 68
			OverlandMap(intCtr, 7).TerrainType = TerrainType.Forest
		Next
		OverlandMap(69, 7).TerrainType = TerrainType.Hills
		OverlandMap(70, 7).TerrainType = TerrainType.Hills
		OverlandMap(71, 7).TerrainType = TerrainType.Mountain
		OverlandMap(72, 7).TerrainType = TerrainType.Impassable

		' next row (6) of map
		OverlandMap(0, 6).TerrainType = TerrainType.Impassable
		OverlandMap(1, 6).TerrainType = TerrainType.Mountain
		OverlandMap(2, 6).TerrainType = TerrainType.Mountain
		OverlandMap(3, 6).TerrainType = TerrainType.Plains
		OverlandMap(4, 6).TerrainType = TerrainType.Plains
		OverlandMap(5, 6).TerrainType = TerrainType.Plains
		OverlandMap(6, 6).TerrainType = TerrainType.Hills
		OverlandMap(7, 6).TerrainType = TerrainType.Hills
		OverlandMap(8, 6).TerrainType = TerrainType.Hills
		OverlandMap(9, 6).TerrainType = TerrainType.Mountain
		OverlandMap(10, 6).TerrainType = TerrainType.Mountain
		For intCtr = 11 To 15
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Water
		Next
		OverlandMap(16, 6).TerrainType = TerrainType.Mountain
		OverlandMap(17, 6).TerrainType = TerrainType.Mountain
		For intCtr = 18 To 24
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Hills
		Next
		OverlandMap(25, 6).TerrainType = TerrainType.Town
		OverlandMap(26, 6).TerrainType = TerrainType.Water
		OverlandMap(27, 6).TerrainType = TerrainType.Water
		OverlandMap(28, 6).TerrainType = TerrainType.Mountain
		OverlandMap(29, 6).TerrainType = TerrainType.Mountain
		OverlandMap(30, 6).TerrainType = TerrainType.Mountain
		For intCtr = 31 To 40
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Plains
		Next
		For intCtr = 41 To 48
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Forest
		Next
		OverlandMap(49, 6).TerrainType = TerrainType.Town
		OverlandMap(50, 6).TerrainType = TerrainType.Mountain
		OverlandMap(51, 6).TerrainType = TerrainType.Mountain
		OverlandMap(52, 6).TerrainType = TerrainType.Mountain
		For intCtr = 53 To 59
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Plains
		Next
		For intCtr = 60 To 69
			OverlandMap(intCtr, 6).TerrainType = TerrainType.Forest
		Next
		OverlandMap(70, 6).TerrainType = TerrainType.Mountain
		OverlandMap(71, 6).TerrainType = TerrainType.Mountain
		OverlandMap(72, 6).TerrainType = TerrainType.Impassable

		' next row (5) of map
		OverlandMap(0, 5).TerrainType = TerrainType.Impassable
		OverlandMap(1, 5).TerrainType = TerrainType.Impassable
		For intCtr = 2 To 6
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Plains
		Next
		OverlandMap(7, 5).TerrainType = TerrainType.Hills
		OverlandMap(8, 5).TerrainType = TerrainType.Hills
		OverlandMap(9, 5).TerrainType = TerrainType.Mountain
		OverlandMap(10, 5).TerrainType = TerrainType.Mountain
		OverlandMap(11, 5).TerrainType = TerrainType.Special
		OverlandMap(12, 5).TerrainType = TerrainType.Mountain
		For intCtr = 13 To 26
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Water
		Next
		For intCtr = 27 To 37
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 38 To 46
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Forest
		Next
		For intCtr = 47 To 50
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Plains
		Next
		OverlandMap(51, 5).TerrainType = TerrainType.Mountain
		For intCtr = 52 To 60
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Plains
		Next
		For intCtr = 61 To 68
			OverlandMap(intCtr, 5).TerrainType = TerrainType.Forest
		Next
		OverlandMap(69, 5).TerrainType = TerrainType.Mountain
		OverlandMap(70, 5).TerrainType = TerrainType.Impassable
		OverlandMap(71, 5).TerrainType = TerrainType.Impassable

		' next row (4) of map
		OverlandMap(0, 4).TerrainType = TerrainType.Impassable
		OverlandMap(1, 4).TerrainType = TerrainType.Mountain
		OverlandMap(2, 4).TerrainType = TerrainType.Mountain
		OverlandMap(3, 4).TerrainType = TerrainType.Mountain
		For intCtr = 4 To 8
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Plains
		Next
		OverlandMap(9, 4).TerrainType = TerrainType.Hills
		OverlandMap(10, 4).TerrainType = TerrainType.Hills
		OverlandMap(11, 4).TerrainType = TerrainType.Hills
		For intCtr = 12 To 18
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(19, 4).TerrainType = TerrainType.Water
		OverlandMap(20, 4).TerrainType = TerrainType.Water
		OverlandMap(21, 4).TerrainType = TerrainType.Water
		For intCtr = 22 To 28
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 29 To 49
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Forest
		Next
		OverlandMap(50, 4).TerrainType = TerrainType.Mountain
		OverlandMap(51, 4).TerrainType = TerrainType.Mountain
		For intCtr = 52 To 54
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Water
		Next
		For intCtr = 55 To 61
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Plains
		Next
		For intCtr = 62 To 68
			OverlandMap(intCtr, 4).TerrainType = TerrainType.Forest
		Next
		OverlandMap(69, 4).TerrainType = TerrainType.Mountain
		OverlandMap(70, 4).TerrainType = TerrainType.Mountain
		OverlandMap(71, 4).TerrainType = TerrainType.Impassable

		' next row (3) of map
		OverlandMap(0, 3).TerrainType = TerrainType.Impassable
		OverlandMap(1, 3).TerrainType = TerrainType.Impassable
		OverlandMap(2, 3).TerrainType = TerrainType.Impassable
		OverlandMap(3, 3).TerrainType = TerrainType.Mountain
		For intCtr = 4 To 9
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Forest
		Next
		OverlandMap(10, 3).TerrainType = TerrainType.Plains
		For intCtr = 11 To 16
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Hills
		Next
		OverlandMap(17, 3).TerrainType = TerrainType.Town
		OverlandMap(18, 3).TerrainType = TerrainType.Mountain
		OverlandMap(19, 3).TerrainType = TerrainType.Mountain
		For intCtr = 20 To 23
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Water
		Next
		For intCtr = 24 To 37
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Forest
		Next
		For intCtr = 38 To 44
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Hills
		Next
		For intCtr = 45 To 48
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Forest
		Next
		OverlandMap(49, 3).TerrainType = TerrainType.Mountain
		OverlandMap(50, 3).TerrainType = TerrainType.Mountain
		For intCtr = 51 To 56
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Water
		Next
		For intCtr = 57 To 62
			OverlandMap(intCtr, 3).TerrainType = TerrainType.Plains
		Next
		OverlandMap(63, 3).TerrainType = TerrainType.Forest
		OverlandMap(64, 3).TerrainType = TerrainType.Mountain
		OverlandMap(65, 3).TerrainType = TerrainType.Volcano
		OverlandMap(66, 3).TerrainType = TerrainType.Mountain
		OverlandMap(67, 3).TerrainType = TerrainType.Forest
		OverlandMap(68, 3).TerrainType = TerrainType.Forest
		OverlandMap(69, 3).TerrainType = TerrainType.Forest
		OverlandMap(70, 3).TerrainType = TerrainType.Mountain
		OverlandMap(71, 3).TerrainType = TerrainType.Impassable

		' next row (2) of map
		OverlandMap(0, 2).TerrainType = TerrainType.Void
		OverlandMap(1, 2).TerrainType = TerrainType.Void
		For intCtr = 2 To 5
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(6, 2).TerrainType = TerrainType.Forest
		OverlandMap(7, 2).TerrainType = TerrainType.Forest
		OverlandMap(8, 2).TerrainType = TerrainType.Forest
		For intCtr = 9 To 12
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Plains
		Next
		OverlandMap(13, 2).TerrainType = TerrainType.Hills
		OverlandMap(14, 2).TerrainType = TerrainType.Hills
		OverlandMap(15, 2).TerrainType = TerrainType.Hills
		For intCtr = 16 To 19
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 20 To 24
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Water
		Next
		OverlandMap(25, 2).TerrainType = TerrainType.Mountain
		OverlandMap(26, 2).TerrainType = TerrainType.Mountain
		For intCtr = 27 To 33
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Forest
		Next
		For intCtr = 34 To 45
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Plains
		Next
		For intCtr = 46 To 49
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 50 To 56
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Water
		Next
		For intCtr = 57 To 62
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(63, 2).TerrainType = TerrainType.Impassable
		OverlandMap(64, 2).TerrainType = TerrainType.Impassable
		For intCtr = 65 To 70
			OverlandMap(intCtr, 2).TerrainType = TerrainType.Mountain
		Next
		OverlandMap(71, 2).TerrainType = TerrainType.Impassable

		' next row (1) of map
		For intCtr = 0 To 4
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Void
		Next
		For intCtr = 5 To 11
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Impassable
		Next
		For intCtr = 12 To 17
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 18 To 21
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Impassable
		Next
		OverlandMap(22, 1).TerrainType = TerrainType.Water
		OverlandMap(23, 1).TerrainType = TerrainType.Water
		For intCtr = 24 To 50
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 51 To 55
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Water
		Next
		For intCtr = 56 To 59
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Mountain
		Next
		For intCtr = 60 To 71
			OverlandMap(intCtr, 1).TerrainType = TerrainType.Impassable
		Next

		' last row (0) of map
		For intCtr = 0 To 72
			OverlandMap(intCtr, 0).TerrainType = TerrainType.Void
		Next
		OverlandMap(7, 0).TerrainType = TerrainType.Impassable
		OverlandMap(8, 0).TerrainType = TerrainType.Impassable
		OverlandMap(9, 0).TerrainType = TerrainType.Impassable
		For intCtr = 11 To 18
			OverlandMap(intCtr, 0).TerrainType = TerrainType.Impassable
		Next
		For intCtr = 21 To 60
			OverlandMap(intCtr, 0).TerrainType = TerrainType.Impassable
		Next

		' Assign Movement Costs
		Dim intX As Integer, intY As Integer
		For intX = 0 To 72
			For intY = 0 To 17
				Select Case OverlandMap(intX, intY).TerrainType
					Case TerrainType.Forest
						OverlandMap(intX, intY).MovementCost = MovementCost.Forest
					Case TerrainType.Hills
						OverlandMap(intX, intY).MovementCost = MovementCost.Hills
					Case TerrainType.Mountain
						OverlandMap(intX, intY).MovementCost = MovementCost.Mountain
					Case TerrainType.Plains
						OverlandMap(intX, intY).MovementCost = MovementCost.Plains
					Case TerrainType.Road
						OverlandMap(intX, intY).MovementCost = MovementCost.Road
					Case TerrainType.Special
						OverlandMap(intX, intY).MovementCost = MovementCost.Special
					Case TerrainType.Town
						OverlandMap(intX, intY).MovementCost = MovementCost.Town
					Case TerrainType.Volcano
						OverlandMap(intX, intY).MovementCost = MovementCost.Volcano
					Case TerrainType.Water
						OverlandMap(intX, intY).MovementCost = MovementCost.Water

					Case TerrainType.Impassable, TerrainType.Void
						OverlandMap(intX, intY).MovementCost = 0

				End Select
			Next
		Next
	End Sub
#End Region

#Region " Overland Structures and Enums "

	Public Enum Direction
		Up = 1
		Down = 2
	End Enum

	Public Enum Heading
		North = 1
		NorthEast = 2	'8 1 2
		East = 3		'7 x 3
		SouthEast = 4	'6 5 4
		South = 5
		SouthWest = 6
		West = 7
		NorthWest = 8
	End Enum

	Public Enum TerrainType
		Void = 0			'    Unreachable Void
		Impassable = 1		' ^  Unclimbable Mountain
		Mountain = 2		' ^  Mountain
		Volcano = 3			' ^  Volcano
		Hills = 4			' ~  Hills
		Plains = 5			' "  Plains
		Road = 6			' .  Road
		Forest = 7			' &  Forest
		Special = 8			' *  Dungeon/Temple/etc
		Town = 9			' o  Town
		Water = 10			' =  Water
	End Enum

	Public Enum MovementCost
		Forest = 12
		Hills = 10
		Mountain = 20
		Plains = 7
		Road = 5
		Special = 4
		Town = 6
		Volcano = 18
		Water = 15
	End Enum

	Public Structure OverlandCell
		Dim TerrainType As Integer
		Dim Observed As Boolean
		Dim MovementCost As Integer
	End Structure

	Public Enum OverlandEncounters
		Skeletons = 1
		Necromancer = 2
		Orcs = 3
		Bandits = 4
		Wolves = 5
		Trolls = 6
	End Enum

#End Region

#Region " Helper Functions "

	Friend Function GetTerrain(ByVal intTerrain As Int16) As String
		Dim t As TerrainType = intTerrain
		Return t.ToString
	End Function

	Friend Sub ReDrawOverlandMap()
		Dim intCtrX As Integer, _
			intCtrY As Integer

		Clear()
		CursorVisible = False

		' go through the array of the overland map
		For intCtrY = 0 To 17
			For intCtrX = 0 To 72
				With OverlandMap(intCtrX, intCtrY)
					Select Case .TerrainType
						Case TerrainType.Void
							' do nothing, black/black is the default

						Case TerrainType.Forest
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "&", ConsoleColor.DarkGreen)
							End If

						Case TerrainType.Hills
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "~", ConsoleColor.DarkYellow)
							End If

						Case TerrainType.Impassable
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.Gray)
							End If

						Case TerrainType.Mountain
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.DarkGray)
							End If

						Case TerrainType.Plains	' chr(34) = "
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, Chr(34), ConsoleColor.Green)
							End If

						Case TerrainType.Road
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, ".", ConsoleColor.DarkYellow)
							End If

						Case TerrainType.Special
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "*", ConsoleColor.DarkGray)
							End If

						Case TerrainType.Town
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "o", ConsoleColor.DarkYellow)
							End If

						Case TerrainType.Volcano
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "^", ConsoleColor.Red)
							End If

						Case TerrainType.Water
							If .Observed Then
								WriteAt(intCtrX + 1, intCtrY + 3, "=", ConsoleColor.Blue)
							End If
					End Select
				End With
			Next
		Next
	End Sub

	Friend Sub OverlandLOS()
		Dim MinX As Int16, MinY As Int16, _
			MaxX As Int16, MaxY As Int16, _
			xCtr As Int16, yCtr As Int16

		' adjust sight radius for when we get to close to the edges
		MinX = TheHero.OverX - (TheHero.Sight - 1)
		If MinX < 0 Then MinX = 0
		MaxX = TheHero.OverX + (TheHero.Sight - 1)
		If MaxX > 72 Then MaxX = 72

		MinY = TheHero.OverY - (TheHero.Sight - 2)
		If MinY < 0 Then MinY = 0
		MaxY = TheHero.OverY + (TheHero.Sight - 2)
		If MaxY > 17 Then MaxY = 17

		For yCtr = MinY To MaxY
			For xCtr = MinX To MaxX
				With OverlandMap(xCtr, yCtr)
					If .Observed = False Then
						.Observed = True
					End If
					If xCtr = TheHero.OverX AndAlso yCtr = TheHero.OverY Then
						' skip this tile, it's the hero
					Else
						FixGround(xCtr, yCtr)
					End If
				End With
			Next
		Next
		For xCtr = MinX + 1 To MaxX - 1
			For yCtr = MinY - 1 To MaxY + 1
				With OverlandMap(xCtr, yCtr)
					If .Observed = False Then
						.Observed = True
					End If
					If xCtr = TheHero.OverX AndAlso yCtr = TheHero.OverY Then
						' skip this tile, it's the hero
					Else
						FixGround(xCtr, yCtr)
					End If
				End With
			Next
		Next
	End Sub

	Friend Sub FixGround(ByVal x As Int16, _
					 ByVal y As Int16)

		Select Case OverlandMap(x, y).TerrainType
			Case TerrainType.Forest
				WriteAt(x + 1, y + 3, "&", ConsoleColor.DarkGreen)
			Case TerrainType.Hills
				WriteAt(x + 1, y + 3, "~", ConsoleColor.DarkYellow)
			Case TerrainType.Impassable
				WriteAt(x + 1, y + 3, "^", ConsoleColor.Gray)
			Case TerrainType.Mountain
				WriteAt(x + 1, y + 3, "^", ConsoleColor.DarkGray)
			Case TerrainType.Plains	' chr(34) = "
				WriteAt(x + 1, y + 3, Chr(34), ConsoleColor.Green)
			Case TerrainType.Road
				WriteAt(x + 1, y + 3, ".", ConsoleColor.DarkYellow)
			Case TerrainType.Special
				WriteAt(x + 1, y + 3, "*", ConsoleColor.DarkGray)
			Case TerrainType.Town
				WriteAt(x + 1, y + 3, "o", ConsoleColor.DarkYellow)
			Case TerrainType.Void
				WriteAt(x + 1, y + 3, " ", ConsoleColor.Black)
			Case TerrainType.Volcano
				WriteAt(x + 1, y + 3, "^", ConsoleColor.DarkRed)
			Case TerrainType.Water
				WriteAt(x + 1, y + 3, "=", ConsoleColor.Blue)
		End Select

	End Sub

#End Region

#Region " Overland Encounter Functions "

	Friend Function CheckForOverlandEncounter(ByVal strMessage As String) As String
		RND = New DBuild.MersenneTwister

		' invisible heroes escape Overland encounters for now
		If TheHero.Invisible Then Return strMessage

		' Overland encounters have a 10% base chance of occurring each turn.
		' This is modified by +1 % for each turn since the last encounter
		Static Dim TurnsSinceLastEncounter As Integer = 0
		Dim OverlandBase As Integer = 10
		OverlandBase += TurnsSinceLastEncounter

		' this chance is modified by certain items, attributes, spells & in-game effects.
		OverlandBase -= (AbilityMod(TheHero.EDexterity) * 2)
		' TODO: other detection related modifiers here

		' race modifiers
		Select Case TheHero.HeroRace
			Case Race.Pixie
				OverlandBase -= 15
			Case Race.Elf, Race.Halfling
				OverlandBase -= 10
			Case Race.HalfElf
				OverlandBase -= 5
			Case Race.Human
				' no effect for humans (just included for clarity)
			Case Race.HalfOrc, Race.Dwarf
				OverlandBase += 5
			Case Enumerations.Race.Ogre
				OverlandBase += 10
		End Select

		' skill modifiers
		If HasSkill("Hide") >= 0 Then OverlandBase -= HasSkill("Hide")
		If HasSkill("Survival") >= 0 Then OverlandBase -= (HasSkill("Survival") \ 2)
		If HasSkill("Tracking") >= 0 Then OverlandBase -= (HasSkill("Tracking") \ 2)

		' ok, all modifiers applied, now make the roll:
		If D100() <= OverlandBase Then
			' we have an encounter!!
			strMessage = OverlandEncounter(strMessage)
			TurnsSinceLastEncounter = 0

		Else
			TurnsSinceLastEncounter += 1
		End If

		Return strMessage
	End Function

	Friend Function OverlandEncounter(ByVal strMessage As String) As String
		Dim Monsters As Integer, StartingDistance As Integer, MonsterType As New Object

		Select Case RND.Next(1, 100)
			Case 1 To 15 ' skeletons
				Monsters = RND.Next(6, 10)
				MonsterType = New Skeleton
				StartingDistance = 10

			Case 16 To 25 ' necromancer (summons undead)
				' create one necromancer, who summons undead on subsequent rounds
				'Monsters = 1
				'MonsterType = New Necromancer
				Monsters = 8
				MonsterType = New Zombie
				StartingDistance = 9

			Case 26 To 50 ' orcs
				Monsters = RND.Next(6, 10)
				MonsterType = New Orc
				StartingDistance = 12

			Case 51 To 65 ' bandits (chance of unique)
				Monsters = RND.Next(9, 12)
				'MonsterType = New Bandit
				MonsterType = New Dog
				StartingDistance = 10

			Case 65 To 90 ' wolves (chance of Dire)
				Monsters = RND.Next(5, 8)
				MonsterType = New Wolf
				StartingDistance = 8

			Case 91 To 100 ' trolls
				Monsters = RND.Next(2, 3)
				MonsterType = New Troll
				StartingDistance = 6
		End Select

		Dim strPrompt As String = "You encounter "
		If Monsters = 1 Then strPrompt &= "a " & MonsterType.MonsterRace & "."
		If Monsters > 1 Then
			If MonsterType.MonsterRacePlural Is Nothing Then
				strPrompt &= "a pack of " & MonsterType.MonsterRace & "s. Prepare to fight. "
			Else
				strPrompt &= "a pack of " & MonsterType.MonsterRacePlural & ". Prepare to fight. "
			End If
		End If

		MessageHandler(strPrompt)
		ReadKey()

		Debug.WriteLine("Encounter: " & Monsters & " " & MonsterType.ToString & "s")

		' THIS IS COMPLETELY WACK... MUST FIX **************
		Dim iCtr As Integer
		For iCtr = 1 To Monsters
			Dim rndX As Integer = RND.Next(TheHero.LocX - StartingDistance, TheHero.LocX + StartingDistance)
			Dim rndy As Integer = RND.Next(TheHero.LocY - (StartingDistance \ 2), TheHero.LocY + (StartingDistance \ 2))

			MonsterType.LocX = rndX
			MonsterType.LocY = rndy

			m_arrMonster.Add(MonsterType)
		Next

		DoTerrain(OverlandMap(TheHero.OverX, TheHero.OverY).TerrainType)
		' END WACK CODE ************************************

		Return strMessage
	End Function

#End Region

End Module