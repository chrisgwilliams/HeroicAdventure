Namespace Common

	Public Module Structures

		Public Structure BodyLocations
			Dim Helmet As Object
			Dim Neck As Object
			Dim Cloak As Object
			Dim Girdle As Object
			Dim Armor As Object
			Dim LeftHand As Object
			Dim RightHand As Object
			Dim LeftRing As Object
			Dim RightRing As Object
			Dim Gloves As Object
			Dim Bracers As Object
			Dim Boots As Object

			Dim MissleWeapon As Object
			Dim Missles As Object

			Dim Tool As Object
			Dim BackPack As ArrayList
		End Structure

		Public Structure MonsterAttackType
			Dim Name As String
			Dim Verb As String
			Dim Bonus As Integer
			Dim MinDamage As Integer
			Dim MaxDamage As Integer
			Dim Poison As Boolean
		End Structure

		Public Structure WeaponMastery
			Public WeaponType As WeaponType
			Public Crits As Integer
			Public Hits As Integer
			Public Swings As Integer
			Public Fumbles As Integer
		End Structure

		Public Structure Award
			Public Name As String
			Public Points As Integer
			Public Achieved As Boolean
		End Structure
	End Module

End Namespace