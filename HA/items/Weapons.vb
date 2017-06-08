Imports HA
Imports HA.Common

Module m_Weapons

#Region " Weapon Base Class "

    Public MustInherit Class Weapon
        Inherits ItemBase
        Implements iEquippableItem

        Private intMinDamage As Integer
        Private intMaxDamage As Integer
        Private intPrice As Integer
        Private intCritical As Integer
        Private intCritMultiplier As Integer
        Private intMode As Integer

        Public Property MinDamage() As Integer
            Get
                MinDamage = intMinDamage
            End Get
            Set(ByVal Value As Integer)
                intMinDamage = Value
            End Set
        End Property
        Public Property MaxDamage() As Integer
            Get
                MaxDamage = intMaxDamage
            End Get
            Set(ByVal Value As Integer)
                intMaxDamage = Value
            End Set
        End Property
        Public Property Price() As Integer
            Get
                Price = intPrice
            End Get
            Set(ByVal Value As Integer)
                intPrice = Value
            End Set
        End Property
        Public Property Critical() As Integer
            Get
                Critical = intCritical
            End Get
            Set(ByVal Value As Integer)
                intCritical = Value
            End Set
        End Property
        Public Property CritMultiplier() As Integer
            Get
                CritMultiplier = intCritMultiplier
            End Get
            Set(ByVal Value As Integer)
                intCritMultiplier = Value
            End Set
        End Property
        Public Property Mode() As Integer
            Get
                Mode = intMode
            End Get
            Set(ByVal Value As Integer)
                intMode = Value
            End Set
        End Property

        Public Sub New()
            Type = ItemType.Weapon
            Color = ColorList.LightGray
            Symbol = "("
            IsBreakable = True
            Tool = False
            Missle = False ' may be overridden of course
            Quantity = 1
        End Sub

        Public MustOverride Sub activate(whoIsActivating As Avatar) Implements iEquippableItem.activate
        Public MustOverride Sub deactivate(whoIsDeactivating As Avatar) Implements iEquippableItem.deactivate
    End Class

#End Region

#Region " -- Weapon Classes "

    Public Class Dagger
        Inherits Weapon

        Public Sub New()
            Name = "dagger"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 4
            Price = 2
            Critical = 19
            CritMultiplier = 2
            Weight = 1
            Mode = AttackType.Piercing
            Damage = "1d4"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class LightMace
        Inherits Weapon

        Public Sub New()
            Name = "light mace"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 5
            Critical = 20
            CritMultiplier = 2
            Weight = 4
            Mode = AttackType.Bludgeoning
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Club
        Inherits Weapon

        Public Sub New()
            Name = "club"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 0
            Critical = 20
            CritMultiplier = 2
            Weight = 3
            Mode = AttackType.Bludgeoning
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class HalfSpear
        Inherits Weapon

        Public Sub New()
            Name = "half-spear"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 1
            Critical = 20
            CritMultiplier = 3
            Weight = 3
            Mode = AttackType.Piercing
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class HeavyMace
        Inherits Weapon

        Public Sub New()
            Name = "heavy mace"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 12
            Critical = 20
            CritMultiplier = 2
            Weight = 8
            Mode = AttackType.Bludgeoning
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Quarterstaff
        Inherits Weapon

        Public Sub New()
            Name = "staff"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 0
            Critical = 20
            CritMultiplier = 2
            Weight = 4
            Mode = AttackType.Bludgeoning
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class ShortSpear
        Inherits Weapon

        Public Sub New()
            Name = "short-spear"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 2
            Critical = 20
            CritMultiplier = 3
            Weight = 5
            Mode = AttackType.Piercing
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class ThrowingAxe
        Inherits Weapon

        Public Sub New()
            Name = "throwing axe"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 8
            Critical = 20
            CritMultiplier = 2
            Weight = 2
            Mode = AttackType.Piercing
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class LightHammer
        Inherits Weapon

        Public Sub New()
            Name = "light hammer"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 4
            Price = 1
            Critical = 20
            CritMultiplier = 2
            Weight = 2
            Mode = AttackType.Bludgeoning
            Damage = "1d4"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class HandAxe
        Inherits Weapon

        Public Sub New()
            Name = "handaxe"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 6
            Critical = 20
            CritMultiplier = 3
            Weight = 3
            Mode = AttackType.Slashing
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class LongSword
        Inherits Weapon

        Public Sub New()
            Name = "longsword"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 15
            Critical = 19
            CritMultiplier = 2
            Weight = 4
            Mode = AttackType.SlashPierce
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class HeavyPick
        Inherits Weapon

        Public Sub New()
            Name = "heavy pick"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 8
            Critical = 20
            CritMultiplier = 4
            Weight = 6
            Mode = AttackType.Piercing
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Rapier
        Inherits Weapon

        Public Sub New()
            Name = "rapier"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 20
            Critical = 18
            CritMultiplier = 2
            Weight = 2
            Mode = AttackType.SlashPierce
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Scimitar
        Inherits Weapon

        Public Sub New()
            Name = "scimitar"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 15
            Critical = 18
            CritMultiplier = 2
            Weight = 4
            Mode = AttackType.SlashPierce
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Trident
        Inherits Weapon

        Public Sub New()
            Name = "trident"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 15
            Critical = 20
            CritMultiplier = 2
            Weight = 4
            Mode = AttackType.Piercing
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Warhammer
        Inherits Weapon

        Public Sub New()
            Name = "warhammer"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 12
            Critical = 20
            CritMultiplier = 3
            Weight = 5
            Mode = AttackType.Bludgeoning
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Falchion
        Inherits Weapon

        Public Sub New()
            Name = "falchion"
            Walkover = Name
            MinDamage = 2
            MaxDamage = 8
            Price = 75
            Critical = 18
            CritMultiplier = 2
            Weight = 8
            Mode = AttackType.Slashing
            Damage = "2d4"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class GreatAxe
        Inherits Weapon

        Public Sub New()
            Name = "greataxe"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 12
            Price = 20
            Critical = 20
            CritMultiplier = 3
            Weight = 12
            Mode = AttackType.Slashing
            Damage = "1d12"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class GreatClub
        Inherits Weapon

        Public Sub New()
            Name = "greatclub"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 10
            Price = 5
            Critical = 20
            CritMultiplier = 2
            Weight = 8
            Mode = AttackType.Bludgeoning
            Damage = "1d10"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Greatsword
        Inherits Weapon

        Public Sub New()
            Name = "greatsword"
            Walkover = Name
            MinDamage = 2
            MaxDamage = 12
            Price = 50
            Critical = 19
            CritMultiplier = 2
            Weight = 8
            Mode = AttackType.SlashPierce
            Damage = "2d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Halberd
        Inherits Weapon

        Public Sub New()
            Name = "halberd"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 10
            Price = 10
            Critical = 20
            CritMultiplier = 3
            Weight = 12
            Mode = AttackType.Slashing
            Damage = "1d10"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Scythe
        Inherits Weapon

        Public Sub New()
            Name = "scythe"
            Walkover = Name
            MinDamage = 2
            MaxDamage = 8
            Price = 18
            Critical = 20
            CritMultiplier = 4
            Weight = 10
            Mode = AttackType.Slashing
            Damage = "2d4"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class MorningStar
        Inherits Weapon

        Public Sub New()
            Name = "morningstar"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 8
            Price = 8
            Critical = 20
            CritMultiplier = 2
            Weight = 6
            Mode = AttackType.Bludgeoning
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class ShortSword
        Inherits Weapon

        Public Sub New()
            Name = "shortsword"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 10
            Critical = 19
            CritMultiplier = 2
            Weight = 2
            Mode = AttackType.SlashPierce
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class BattleAxe
        Inherits Weapon

        Public Sub New()
            Name = "battleaxe"
            Walkover = Name
            MinDamage = 1
            MaxDamage = 6
            Price = 10
            Critical = 19
            CritMultiplier = 2
            Weight = 6
            Mode = AttackType.Slashing
            Damage = "1d6"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class

#End Region

#Region " -- MissleWeapon Class and Inherited Subclasses "

    Public Class MissleWeapon
        Inherits Weapon

        Private intRange As Integer
        Private intRequiredMissle As Integer

        Public Property Range() As Integer
            Get
                Range = intRange
            End Get
            Set(ByVal Value As Integer)
                intRange = Value
            End Set
        End Property
        Public Property RequiredMissle() As Integer
            Get
                RequiredMissle = intRequiredMissle
            End Get
            Set(ByVal Value As Integer)
                intRequiredMissle = Value
            End Set
        End Property

        Public Sub New()
            MyBase.New()

            Color = ColorList.Olive
            Type = ItemType.MissleWeapon
            Symbol = "}"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class

    Public Class Sling
        Inherits MissleWeapon

        Public Sub New()
            MyBase.New()

            Name = "sling"
            Walkover = Name
            Price = 1
            Range = 3
            Weight = 0.5
            Critical = 20
            CritMultiplier = 2
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class ShortBow
        Inherits MissleWeapon

        Public Sub New()
            MyBase.New()

            Name = "shortbow"
            Walkover = Name
            Price = 10
            Range = 4
            Weight = 6
            RequiredMissle = MissleType.arrow
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class LongBow
        Inherits MissleWeapon

        Public Sub New()
            MyBase.New()

            Name = "longbow"
            Walkover = Name
            Price = 20
            Range = 6
            Weight = 8
            RequiredMissle = MissleType.arrow
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class

#End Region

#Region " -- Missle Class and Inherited Subclasses "
    Public Class Missle
        Inherits Weapon

        Public Sub New()
            MyBase.New()

            Type = ItemType.Missles
            Missle = True
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class

    Public Class Arrow
        Inherits Missle

        Public Sub New()
            MyBase.New()

            Name = "arrow"
            Walkover = Name
            Symbol = "/"
            MinDamage = 1
            MaxDamage = 8
            Price = 0.05
            Critical = 20
            CritMultiplier = 2
            Damage = "1d8"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class
    Public Class Rock
        Inherits Missle

        Public Sub New()
            MyBase.New()

            Name = "rock"
            Walkover = Name
            Symbol = "*"
            Color = ColorList.DarkGray
            MinDamage = 1
            MaxDamage = 4
            Price = 0
            Critical = 20
            CritMultiplier = 2
            Damage = "1d4"
        End Sub

        Public Overrides Sub activate(whoIsActivating As Avatar)
        End Sub

        Public Overrides Sub deactivate(whoIsDeactivating As Avatar)
        End Sub
    End Class

#End Region

End Module
