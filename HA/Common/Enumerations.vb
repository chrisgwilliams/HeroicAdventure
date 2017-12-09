Namespace Common
    Public Module Enumerations

#Region " Items "
        Public Enum ArmorType
            Robes = 1
            Clothing = 2
            Padded = 3
            Leather = 4
            Furs = 5
            StuddedLeather = 6
            ChainShirt = 7
            ScaleMail = 8
            ChainMail = 9
            Breastplate = 10
            ElvenChain = 11
            SplintMail = 12
            BandedMail = 13
            HalfPlate = 14
            FullPlate = 15
        End Enum
        Public Enum ShieldType
            Buckler = 1
            Small = 1
            Large = 2
        End Enum
        Public Enum WeaponType
            Dagger = 0
            LightMace = 1
            Club = 2
            HalfSpear = 3
            HeavyMace = 4
            Quarterstaff = 5
            ShortSpear = 6
            ThrowingAxe = 7
            LightHammer = 8
            HandAxe = 9
            Longsword = 10
            HeavyPick = 11
            Rapier = 12
            Scimitar = 13
            Trident = 14
            Warhammer = 15
            Falchion = 16
            GreatAxe = 17
            GreatClub = 18
            GreatSword = 19
            Halberd = 20
            Scythe = 21
            Morningstar = 22
            ShortSword = 23
            BattleAxe = 24

            Unarmed = 50
        End Enum
        Public Enum ItemType
            Unspecified = 0
            Potion = 1
            Scroll = 2
            Gold = 3
            Tool = 4
            Armor = 5
            Weapon = 6
            Helmet = 7
            Boots = 8
            Shield = 9
            Neck = 10
            Girdle = 11
            Cloak = 12
            Ring = 13
            Gloves = 14
            Bracers = 15
            MissleWeapon = 16
            Missiles = 17
            Book = 18
            Wand = 19
            Gem = 20
            Food = 21
        End Enum
        Public Enum PotionType
            Healing = 1
            ExtraHealing = 2
            Poison = 3
            Water = 4
            Invisibility = 5
        End Enum
        Public Enum DivineState
            Cursed = -1
            Normal = 0
            Blessed = 1
        End Enum
        Public Enum CookState
            Raw
            Cooked
            Burnt
            Inedible
        End Enum
        Public Enum MissleType
            arrow = 1
            rock = 2
            slingstone = 3
        End Enum
        Public Enum AttackType
            Bludgeoning = 1
            Piercing = 2
            Slashing = 3
            SlashPierce = 4
        End Enum
#End Region

#Region " Avatar "
        Public Enum Race
            Human = 1
            Elf = 2
            HalfElf = 3
            Dwarf = 4
            Gnome = 5
            Halfling = 6
            HalfOrc = 7
            Ogre = 8
            Pixie = 9
        End Enum
        Public Enum PCClass
            None = 0
            Warrior = 1
            Barbarian = 5
            Paladin = 9
            Wizard = 2
            Sorceror = 6
            Thief = 3
            Assassin = 7
            Priest = 4
            Druid = 8
        End Enum
        Public Enum PCStats
            strength = 1
            intelligence
            wisdom
            dexterity
            constitution
            charisma
        End Enum
        Public Enum Gender
            female = 1
            male = 2
        End Enum
        Public Enum PietyState
            Hated = -3
            Unfavored = -2
            Displeased = -1
            Normal = 0
            Pleased = 1
            Favored = 2
            Loved = 3
        End Enum
        Public Enum HungerState
            Starving
            Hungry
            Normal
            Satiated
            Bloated
        End Enum
        Public Enum ActionType
            None = 0
            Drink = 1
            Drop = 2
            [Throw] = 3
            Use = 4
            Zap = 5
        End Enum
        Public Enum StarSign
            Raven = 1
            Book = 2
            Wand = 3
            Unicorn = 4
            Salamander = 5
            Dragon = 6
            Sword = 7
            Falcon = 8
            Cup = 9
            Candle = 10
            Wolf = 11
            Tree = 12
        End Enum
#End Region

#Region " Monster / NPC "
        Public Enum Disposition
            Charmed = -2
            Friendly = -1
            Neutral = 0
            Hostile = 1
            Enraged = 2
        End Enum
        Public Enum QuestStatus
            Unassigned
            Assigned
            Failed
            Completed
        End Enum
#End Region

#Region " Metadata "
        Public Enum ColorList
            Black = 0
            Navy = 1
            DarkGreen = 2
            Aquamarine = 3
            Maroon = 4
            Purple = 5
            Olive = 6
            LightGray = 7
            DarkGray = 8
            Blue = 9
            Green = 10
            Cyan = 11
            Red = 12
            Magenta = 13
            Yellow = 14
            White = 15
        End Enum
        Public Enum MetalList
            Silver = 1
            Gold = 2
            Copper = 3
            Brass = 4
            Bronze = 5
            Electrum = 6
            Platinum = 7
            Steel = 8
            Aluminum = 9
            Mithril = 10
        End Enum
        Public Enum GemList
            Amethyst = 1
            Diamond = 2
            Emerald = 3
            Garnet = 4
            Ruby = 5
            Sapphire = 6
            Onyx = 7
            Opal = 8
            Pearl = 9
            Topaz = 10
        End Enum
        Public Enum WandList
            Pine = 1
            Larch = 2
            Petrified = 3
            Twisted = 4
            Willow = 5
            Oak = 6
            Bone = 7
            Cherry = 8
            Spruce = 9
            Walnut = 10
        End Enum
        Public Enum MagicType
            None = 0
            Arcane = 1
            Divine = 2
        End Enum
        Public Enum TrapType
            None = 0
            sleep = 1
            poison = 2
            explosion = 3
            pit = 4
            snake = 5
            rock = 6
            confusion = 7
            teleport = 8
        End Enum
        Public Enum PoisonType
            none = 0
            sleep = 1
            confusion = 2
            paralytic = 3
            strength = 4
            constitution = 5
            mild = 6
            medium = 7
            strong = 8
        End Enum
#End Region

#Region " Travel "
        Public Enum CompassDirection
            NorthWest = 7
            North = 8       '7 8 9
            NorthEast = 9   '4 5 6
            West = 4        '1 2 3
            StandStill = 5
            East = 6
            SouthWest = 1
            South = 2
            SouthEast = 3
        End Enum
        Public Enum Heading
            North = 1
            NorthEast = 2   '8 1 2
            East = 3        '7 x 3
            SouthEast = 4   '6 5 4
            South = 5
            SouthWest = 6
            West = 7
            NorthWest = 8
        End Enum
        Public Enum MovementType
            Walking = 1
            Running = 2
            Flying = 0
        End Enum
        Public Enum RoadAlignment
            EastWest
            NorthSouth
            NorthEast
            NorthWest
            SouthEast
            SouthWest
        End Enum
        Public Enum Town
            AbandonedVillage
            Fincastle
            Lakeside
            Sawtooth
            StoneGate
        End Enum
#End Region

#Region " World "
        Public Enum WindState
            None
            Light
            Moderate
            Heavy
        End Enum
        Public Enum CloudState
            None
            Light
            Moderate
            Heavy
        End Enum
        Public Enum PrecipitationState
            None
            Light
            Moderate
            Heavy
        End Enum
        Public Enum SeasonState
            Winter
            Spring
            Summer
            Fall
        End Enum
        Public Enum DayNightState
            Dawn
            Day
            Dusk
            Night
        End Enum
#End Region

    End Module
End Namespace
