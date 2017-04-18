Namespace Common
    Module m_RandomTables

        Friend Sub RandomMonster(ByVal rndX As Integer, ByVal rndY As Integer, ByVal intZ As Integer)
            Dim mon As Monster

            ' this way sucks and should be replaced
            'RANGE: (hero levels / 2) + {-4...-2}  TO  8 + hero levels + {-3...3}
            Dim MonsterRoll As Integer '= RND.Next(-2 + (TheHero.TotalLevels \ 2) + (RND.Next(-2, 0)), 8 + TheHero.TotalLevels + RND.Next(-3, 3))

            ' new way should use dungeon level as difficulty base and ramp up if hero is sandbagging 
            ' (or on his/her way out of the dungeon... either way, turn up the heat.)
            With TheHero
                If .TotalLevels > .CurrentLevel + 5 Then
                    Dim LevelDiff As Integer = .TotalLevels - .CurrentLevel
                    MonsterRoll = RND.Next(.CurrentLevel - 1, .CurrentLevel + LevelDiff + 2)
                Else
                    MonsterRoll = RND.Next(.CurrentLevel - 3, .CurrentLevel + 2)
                End If
            End With

            Select Case MonsterRoll
                Case Is < -3
                    mon = New BabyLizard
                Case -3 To -2
                    mon = New Puppy
                Case -1 To 0
                    mon = New Rat
                    ' small rat
                    With mon
                        .HP = .HP \ 2
                        .CurrentHP = .HP
                        .AC = .AC - 3
                        .MonsterRace = "small " & .MonsterRace
                        .CR = 0.22
                    End With
                Case 1
                    mon = New Rat
                Case 2 To 3
                    mon = New Lizard
                Case 4 To 5
                    mon = New Dog
                Case 6 To 7
                    mon = New Kobold
                Case 8 To 9
                    mon = New Wolf
                Case 10 To 11
                    mon = New Zombie
                Case 12 To 13
                    mon = New DireRat
                Case 14 To 15
                    mon = New Goblin
                Case 16 To 17
                    mon = New Orc
                Case 18 To 19
                    mon = New Skeleton
                Case 20 To 21
                    mon = New LargeSpider
                Case 22 To 23
                    mon = New Ogre
                Case 24
                    mon = New DireWolf
                Case 25
                    mon = New OgreMage
                Case 26
                    mon = New Troll
                Case 27
                    mon = New HugeSpider
                Case 28
                    mon = New OgrePrince
                Case Else
                    mon = New Orc
            End Select

            With mon
                .LocX = rndX
                .LocY = rndY
                .LocZ = intZ
            End With

            m_arrMonster.Add(mon)
        End Sub

#Region " Random Items "

        Friend Function RandomPotion(Optional ByVal Modifier As Integer = 0) As Potion

            'ToDo: re-sort potion table by "rarity"
            Select Case (D10() + Modifier)
                Case 1 To 4
                    RandomPotion = New PoWater
                Case 5 To 7
                    RandomPotion = New PoHealing
                Case 8
                    RandomPotion = New PoInvisibility
                Case 9
                    RandomPotion = New PoExtraHealing
                Case 10
                    RandomPotion = New PoPoison
                Case Else
                    RandomPotion = RandomPotion()
            End Select

            'ToDo: add check for Lucky/Unlucky state when checking for blessed / cursed state of Potion
            'ToDo: spread out distribution of cursed/blessed Potions
            RandomPotion.ItemState = DirectCast(RND.Next(-1, 1), ItemBase.DivineState)

        End Function
        Friend Function RandomScroll(Optional ByVal Modifier As Integer = 0) As Scroll

            'ToDo: re-sort scroll table by "rarity"
            Select Case (RND.Next(1, 6) + Modifier)
                Case 1
                    RandomScroll = New SoEnchantWeapon
                Case 2
                    RandomScroll = New SoMagicMap
                Case 3
                    RandomScroll = New SoAmnesia
                Case 4
                    RandomScroll = New SoCureLight
                Case 5
                    RandomScroll = New SoIdentify
                Case 6
                    RandomScroll = New SoMagicMissile
                Case Else
                    RandomScroll = New BlankScroll
            End Select

            'ToDo: add check for Lucky/Unlucky state when checking for blessed / cursed state of Scroll
            'ToDo: spread out distribution of cursed/blessed scrolls
            RandomScroll.ItemState = DirectCast(RND.Next(-1, 1), ItemBase.DivineState)

        End Function
        Friend Function RandomTool(Optional ByVal Modifier As Integer = 0) As ItemBase
            Select Case (RND.Next(1, 20) + Modifier)
                Case 1 To 19
                    RandomTool = New Rock
                Case Else
                    RandomTool = New PickAxe
            End Select

            'ToDo: add check for Lucky/Unlucky state when checking for blessed / cursed state of tools
            'ToDo: spread out distribution of cursed/blessed tools
            RandomTool.ItemState = DirectCast(RND.Next(-1, 1), ItemBase.DivineState)

        End Function
        Friend Function RandomArmor(Optional ByVal Modifier As Integer = 0) As Armor
            'ToDo: At some point, we need Artifact or Legendary armor or whatever we're going to call it.
            Select Case (RND.Next(1, 15) + Modifier)
                Case ArmorType.Robes
                    RandomArmor = New Robes
                Case ArmorType.Clothing
                    RandomArmor = New Clothing
                Case ArmorType.Padded
                    RandomArmor = New Padded
                Case ArmorType.Leather
                    RandomArmor = New Leather
                Case ArmorType.Furs
                    RandomArmor = New Furs
                Case ArmorType.StuddedLeather
                    RandomArmor = New StuddedLeather
                Case ArmorType.ChainShirt
                    RandomArmor = New ChainShirt
                Case ArmorType.ScaleMail
                    RandomArmor = New ScaleMail
                Case ArmorType.ChainMail
                    RandomArmor = New ChainMail
                Case ArmorType.Breastplate
                    RandomArmor = New BreastPlate
                Case ArmorType.ElvenChain
                    RandomArmor = New ElvenChain
                Case ArmorType.SplintMail
                    RandomArmor = New SplintMail
                Case ArmorType.BandedMail
                    RandomArmor = New BandedMail
                Case ArmorType.HalfPlate
                    RandomArmor = New HalfPlate
                Case ArmorType.FullPlate
                    RandomArmor = New FullPlate
                Case Else
                    RandomArmor = New Clothing
            End Select

            'ToDo: add check for Lucky/Unlucky state when checking for blessed / cursed state of armor
            'ToDo: spread out distribution of cursed/blessed armor
            RandomArmor.ItemState = DirectCast(RND.Next(-1, 1), ItemBase.DivineState)

        End Function
        Friend Function RandomWeapon(Optional ByVal Modifier As Integer = 0) As Weapon
            Select Case (RND.Next(0, 24) + Modifier)
                Case WeaponType.Dagger
                    RandomWeapon = New Dagger
                Case WeaponType.LightMace
                    RandomWeapon = New LightMace
                Case WeaponType.Club
                    RandomWeapon = New Club
                Case WeaponType.HalfSpear
                    RandomWeapon = New HalfSpear
                Case WeaponType.HeavyMace
                    RandomWeapon = New HeavyMace
                Case WeaponType.Morningstar
                    RandomWeapon = New MorningStar
                Case WeaponType.Quarterstaff
                    RandomWeapon = New Quarterstaff
                Case WeaponType.ShortSpear
                    RandomWeapon = New ShortSpear
                Case WeaponType.ThrowingAxe
                    RandomWeapon = New ThrowingAxe
                Case WeaponType.LightHammer
                    RandomWeapon = New LightHammer
                Case WeaponType.HandAxe
                    RandomWeapon = New HandAxe
                Case WeaponType.ShortSword
                    RandomWeapon = New ShortSword
                Case WeaponType.BattleAxe
                    RandomWeapon = New BattleAxe
                Case WeaponType.Longsword
                    RandomWeapon = New LongSword
                Case WeaponType.HeavyPick
                    RandomWeapon = New HeavyPick
                Case WeaponType.Rapier
                    RandomWeapon = New Rapier
                Case WeaponType.Scimitar
                    RandomWeapon = New Scimitar
                Case WeaponType.Trident
                    RandomWeapon = New Trident
                Case WeaponType.Warhammer
                    RandomWeapon = New Warhammer
                Case WeaponType.Falchion
                    RandomWeapon = New Falchion
                Case WeaponType.GreatAxe
                    RandomWeapon = New GreatAxe
                Case WeaponType.GreatClub
                    RandomWeapon = New GreatClub
                Case WeaponType.GreatSword
                    RandomWeapon = New Greatsword
                Case WeaponType.Halberd
                    RandomWeapon = New Halberd
                Case WeaponType.Scythe
                    RandomWeapon = New Scythe
                Case Else
                    RandomWeapon = New Dagger
            End Select
        End Function
        Friend Function RandomHelmet(Optional ByVal Modifier As Integer = 0) As Helmet
            Select Case (RND.Next(1, 8) + Modifier)
                Case 1
                    RandomHelmet = New FurHat
                Case 2
                    RandomHelmet = New LeatherHelmet
                Case 3
                    RandomHelmet = New SkullCap
                Case 4
                    RandomHelmet = New HornedHelmet
                Case 5
                    RandomHelmet = New ChainHood
                Case 6
                    RandomHelmet = New PlateHelm
                Case 7
                    RandomHelmet = New ScaleHelm
                Case 8
                    RandomHelmet = New OrcishHelm
                Case Else
                    RandomHelmet = New FurHat
            End Select
        End Function
        Friend Function RandomBoots(Optional ByVal Modifier As Integer = 0) As Boots
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1
                    RandomBoots = New Sandles
                Case 2
                    RandomBoots = New FurBoots
                Case 3
                    RandomBoots = New LeatherBoots
                Case 4
                    RandomBoots = New ChainBoots
                Case Else
                    RandomBoots = New Sandles
            End Select
        End Function
        Friend Function RandomShield(Optional ByVal Modifier As Integer = 0) As Shield
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomShield = New Buckler
                Case 3
                    RandomShield = New SmallShield
                Case 4
                    RandomShield = New LargeShield
                Case Else
                    RandomShield = New Buckler
            End Select
        End Function
        Friend Function RandomAmulet(Optional ByVal Modifier As Integer = 0) As Amulet
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomAmulet = New AoStrength
                Case 3 To 4
                    RandomAmulet = New GoldAmulet
                Case Else
                    RandomAmulet = New GoldAmulet
            End Select
        End Function
        Friend Function RandomGirdle(Optional ByVal Modifier As Integer = 0) As Girdle
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomGirdle = New LeatherGirdle
                Case 3 To 4
                    RandomGirdle = New MetalGirdle
                Case Else
                    RandomGirdle = New LeatherGirdle
            End Select
        End Function
        Friend Function RandomCloak(Optional ByVal Modifier As Integer = 0) As Cloak
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomCloak = New UnadornedCloak
                Case 3 To 4
                    RandomCloak = New SturdyCloak
                Case Else
                    RandomCloak = New SturdyCloak
            End Select
        End Function
        Friend Function RandomRing(Optional ByVal Modifier As Integer = 0) As Ring
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomRing = New RoProtection
                Case 3 To 4
                    RandomRing = New RoInvisibility
                Case Else
                    RandomRing = New RoInvisibility
            End Select
        End Function
        Friend Function RandomGloves(Optional ByVal Modifier As Integer = 0) As Gloves
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomGloves = New LeatherGauntlets
                Case 3
                    RandomGloves = New ChainGauntlets
                Case 4
                    RandomGloves = New PlateGauntlets
                Case Else
                    RandomGloves = New LeatherGauntlets
            End Select
        End Function
        Friend Function RandomBracers(Optional ByVal Modifier As Integer = 0) As Bracers
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomBracers = New BoProtection
                Case 3 To 4
                    RandomBracers = New BoStrength
                Case Else
                    RandomBracers = New BoProtection
            End Select
        End Function
        Friend Function RandomMissleWeapon(Optional ByVal Modifier As Integer = 0) As MissleWeapon
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomMissleWeapon = New Sling
                Case 3
                    RandomMissleWeapon = New ShortBow
                Case 4
                    RandomMissleWeapon = New LongBow
                Case Else
                    RandomMissleWeapon = New Sling
            End Select
        End Function
        Friend Function RandomMissle(Optional ByVal Modifier As Integer = 0) As Missle
            Select Case (RND.Next(1, 4) + Modifier)
                Case 1 To 2
                    RandomMissle = New Arrow
                    RandomMissle.Quantity = RND.Next(1, 21)
                Case 3 To 4
                    RandomMissle = New Rock
                    RandomMissle.Quantity = RND.Next(1, 5)
                Case Else
                    RandomMissle = New Rock
                    RandomMissle.Quantity = 1
            End Select
        End Function
        Friend Function RandomWand(Optional ByVal Modifier As Integer = 0) As Wand
            RandomWand = New WoMagicMap
        End Function
        Friend Function RandomGem(Optional ByVal Modifier As Integer = 0) As Gem
            RandomGem = New Gem
        End Function

#End Region


    End Module
End Namespace