Namespace Common

    Public Structure BodyLocations
        Dim Helmet As Helmet
        Dim Neck As Amulet
        Dim Cloak As Cloak
        Dim Girdle As Girdle
        Dim Armor As Armor
        Dim LeftHand As Object
        Dim RightHand As Object
        Dim LeftRing As Ring
        Dim RightRing As Ring
        Dim Gloves As Gloves
        Dim Bracers As Bracers
        Dim Boots As Boots

        Dim MissleWeapon As Object
        Dim Missles As Object

        Dim Tool As Tool
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

End Namespace