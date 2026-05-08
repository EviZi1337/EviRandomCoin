using System.Collections.Generic;
using System.ComponentModel;
using EviRandomCoin.Models;
using InventorySystem.Items.Usables.Scp330;
using MapGeneration;
using PlayerRoles;

namespace EviRandomCoin
{
    public sealed class RandomCoinConfig
    {
        public RandomCoinConfig()
        {
            Debug = false;
            RequireRoundInProgress = true;
            AlivePlayersOnly = true;
            ConsumeCoinByDefault = true;
            DropItemsWhenInventoryFull = true;
            CooldownSeconds = 1.25f;
            HintDuration = 6f;
            TeleportVerticalOffset = 1.35f;
            MinHealAmount = 18f;
            MaxHealAmount = 60f;
            MinArtificialHealthAmount = 25f;
            MaxArtificialHealthAmount = 80f;
            MinDamageAmount = 12f;
            MaxDamageAmount = 45f;
            BadLuckCanKill = false;
            MinBlackoutDuration = 5f;
            MaxBlackoutDuration = 14f;
            MinBonusCoins = 2;
            MaxBonusCoins = 4;
            MinRerollItems = 2;
            MaxRerollItems = 4;
            MinRainItems = 3;
            MaxRainItems = 6;
            ItemRainRadius = 2.4f;
            LaunchStrength = 9f;
            AllowRoleChanges = true;
            HumansOnlyForRoleChanges = true;
            RoleChangeSpawnFlags = RoleSpawnFlags.AssignInventory;
            AllowTeleport = true;
            HumansOnlyForTeleport = true;
            AllowPlayerSwap = true;
            HumansOnlyForPlayerSwap = true;
            AllowBlackout = true;
            AllowInventoryPunishments = true;
            RerollRareItemChance = 0.2f;
            DamageReason = "The coin chose violence.";
            Hints = new HintConfig();
            Outcomes = CreateDefaultOutcomes();
            GoodEffects = CreateGoodEffects();
            BadEffects = CreateBadEffects();
            CommonItems = CreateCommonItems();
            RareItems = CreateRareItems();
            AmmoRewards = CreateAmmoRewards();
            RoleRewards = CreateRoleRewards();
            CandyRewards = CreateCandyRewards();
            TeleportZones = CreateTeleportZones();
        }

        [Description("Print detailed plugin logs.")]
        public bool Debug { get; set; }

        [Description("Skip random outcomes before the round is actually in progress.")]
        public bool RequireRoundInProgress { get; set; }

        [Description("Skip spectators, dead players and other non-alive roles.")]
        public bool AlivePlayersOnly { get; set; }

        [Description("Used when an outcome does not override coin consumption.")]
        public bool ConsumeCoinByDefault { get; set; }

        [Description("If inventory is full, item rewards spawn as pickups near the player.")]
        public bool DropItemsWhenInventoryFull { get; set; }

        [Description("Per-player cooldown after a coin outcome.")]
        public float CooldownSeconds { get; set; }

        [Description("Default hint duration.")]
        public float HintDuration { get; set; }

        [Description("Upward offset used for room teleport positions.")]
        public float TeleportVerticalOffset { get; set; }

        public float MinHealAmount { get; set; }

        public float MaxHealAmount { get; set; }

        public float MinArtificialHealthAmount { get; set; }

        public float MaxArtificialHealthAmount { get; set; }

        public float MinDamageAmount { get; set; }

        public float MaxDamageAmount { get; set; }

        public bool BadLuckCanKill { get; set; }

        public float MinBlackoutDuration { get; set; }

        public float MaxBlackoutDuration { get; set; }

        public int MinBonusCoins { get; set; }

        public int MaxBonusCoins { get; set; }

        public int MinRerollItems { get; set; }

        public int MaxRerollItems { get; set; }

        public int MinRainItems { get; set; }

        public int MaxRainItems { get; set; }

        public float ItemRainRadius { get; set; }

        public float LaunchStrength { get; set; }

        public bool AllowRoleChanges { get; set; }

        public bool HumansOnlyForRoleChanges { get; set; }

        public RoleSpawnFlags RoleChangeSpawnFlags { get; set; }

        public bool AllowTeleport { get; set; }

        public bool HumansOnlyForTeleport { get; set; }

        [Description("Allow the player_swap outcome.")]
        public bool AllowPlayerSwap { get; set; }

        [Description("When player_swap is selected, both players must be alive humans.")]
        public bool HumansOnlyForPlayerSwap { get; set; }

        public bool AllowBlackout { get; set; }

        public bool AllowInventoryPunishments { get; set; }

        [Description("Chance from 0 to 1 that each inventory_reroll reward uses RareItems instead of CommonItems.")]
        public float RerollRareItemChance { get; set; }

        public string DamageReason { get; set; }

        public HintConfig Hints { get; set; }

        public List<OutcomeConfig> Outcomes { get; set; }

        public List<WeightedEffectConfig> GoodEffects { get; set; }

        public List<WeightedEffectConfig> BadEffects { get; set; }

        public List<WeightedItemConfig> CommonItems { get; set; }

        public List<WeightedItemConfig> RareItems { get; set; }

        public List<AmmoRewardConfig> AmmoRewards { get; set; }

        public List<WeightedRoleConfig> RoleRewards { get; set; }

        public List<WeightedCandyConfig> CandyRewards { get; set; }

        public List<FacilityZone> TeleportZones { get; set; }

        private static List<OutcomeConfig> CreateDefaultOutcomes()
        {
            return new List<OutcomeConfig>
            {
                new OutcomeConfig("good_effect", 14, true, null),
                new OutcomeConfig("bad_effect", 12, true, null),
                new OutcomeConfig("common_item", 14, true, null),
                new OutcomeConfig("rare_item", 5, true, null),
                new OutcomeConfig("ammo", 8, true, null),
                new OutcomeConfig("heal", 7, true, null),
                new OutcomeConfig("artificial_health", 6, true, null),
                new OutcomeConfig("damage", 6, true, null),
                new OutcomeConfig("teleport", 6, true, null),
                new OutcomeConfig("player_swap", 4, true, null),
                new OutcomeConfig("role_change", 4, true, null),
                new OutcomeConfig("candy", 5, true, null),
                new OutcomeConfig("inventory_wipe", 2, true, null),
                new OutcomeConfig("inventory_reroll", 5, true, null),
                new OutcomeConfig("coin_copy", 5, true, true),
                new OutcomeConfig("blackout", 3, true, null),
                new OutcomeConfig("launch", 4, true, null),
                new OutcomeConfig("item_rain", 4, true, null),
                new OutcomeConfig("nothing", 4, true, null)
            };
        }

        private static List<WeightedEffectConfig> CreateGoodEffects()
        {
            return new List<WeightedEffectConfig>
            {
                new WeightedEffectConfig("invisible", 2, 1, 1, 5f, 12f),
                new WeightedEffectConfig("movement_boost", 5, 70, 140, 8f, 18f),
                new WeightedEffectConfig("silent_walk", 4, 1, 1, 12f, 25f),
                new WeightedEffectConfig("invigorated", 4, 1, 1, 8f, 18f),
                new WeightedEffectConfig("night_vision", 3, 1, 1, 15f, 30f),
                new WeightedEffectConfig("damage_reduction", 3, 20, 45, 8f, 15f),
                new WeightedEffectConfig("vitality", 2, 1, 1, 10f, 20f),
                new WeightedEffectConfig("lightweight", 3, 20, 60, 10f, 20f),
                new WeightedEffectConfig("rainbow_taste", 1, 1, 3, 10f, 20f)
            };
        }

        private static List<WeightedEffectConfig> CreateBadEffects()
        {
            return new List<WeightedEffectConfig>
            {
                new WeightedEffectConfig("amnesia_vision", 3, 1, 1, 8f, 18f),
                new WeightedEffectConfig("bleeding", 4, 1, 1, 8f, 18f),
                new WeightedEffectConfig("burned", 2, 1, 1, 5f, 12f),
                new WeightedEffectConfig("poisoned", 2, 1, 1, 6f, 14f),
                new WeightedEffectConfig("flashed", 3, 255, 255, 2f, 5f),
                new WeightedEffectConfig("blindness", 2, 1, 1, 3f, 7f),
                new WeightedEffectConfig("deafened", 3, 1, 1, 6f, 14f),
                new WeightedEffectConfig("concussed", 3, 1, 1, 6f, 12f),
                new WeightedEffectConfig("exhausted", 3, 1, 1, 8f, 18f),
                new WeightedEffectConfig("heavy_footed", 4, 25, 70, 10f, 20f),
                new WeightedEffectConfig("hemorrhage", 2, 1, 1, 6f, 12f),
                new WeightedEffectConfig("stained", 2, 1, 1, 12f, 24f)
            };
        }

        private static List<WeightedItemConfig> CreateCommonItems()
        {
            return new List<WeightedItemConfig>
            {
                new WeightedItemConfig(ItemType.KeycardScientist, 4),
                new WeightedItemConfig(ItemType.KeycardZoneManager, 3),
                new WeightedItemConfig(ItemType.Radio, 4),
                new WeightedItemConfig(ItemType.Medkit, 4),
                new WeightedItemConfig(ItemType.Flashlight, 4),
                new WeightedItemConfig(ItemType.Painkillers, 5),
                new WeightedItemConfig(ItemType.Adrenaline, 3),
                new WeightedItemConfig(ItemType.ArmorLight, 3),
                new WeightedItemConfig(ItemType.GunCOM15, 3),
                new WeightedItemConfig(ItemType.GrenadeFlash, 2),
                new WeightedItemConfig(ItemType.SCP330, 2),
                new WeightedItemConfig(ItemType.Lantern, 2),
                new WeightedItemConfig(ItemType.SurfaceAccessPass, 2)
            };
        }

        private static List<WeightedItemConfig> CreateRareItems()
        {
            return new List<WeightedItemConfig>
            {
                new WeightedItemConfig(ItemType.KeycardMTFCaptain, 2),
                new WeightedItemConfig(ItemType.KeycardFacilityManager, 1),
                new WeightedItemConfig(ItemType.KeycardChaosInsurgency, 2),
                new WeightedItemConfig(ItemType.GunCrossvec, 3),
                new WeightedItemConfig(ItemType.GunE11SR, 2),
                new WeightedItemConfig(ItemType.GunAK, 2),
                new WeightedItemConfig(ItemType.GunShotgun, 2),
                new WeightedItemConfig(ItemType.SCP500, 2),
                new WeightedItemConfig(ItemType.SCP207, 2),
                new WeightedItemConfig(ItemType.AntiSCP207, 2),
                new WeightedItemConfig(ItemType.SCP268, 1),
                new WeightedItemConfig(ItemType.SCP1853, 1),
                new WeightedItemConfig(ItemType.SCP1576, 1),
                new WeightedItemConfig(ItemType.SCP1344, 1),
                new WeightedItemConfig(ItemType.SCP2176, 1),
                new WeightedItemConfig(ItemType.Jailbird, 1),
                new WeightedItemConfig(ItemType.ParticleDisruptor, 1),
                new WeightedItemConfig(ItemType.GunA7, 1),
                new WeightedItemConfig(ItemType.GunSCP127, 1)
            };
        }

        private static List<AmmoRewardConfig> CreateAmmoRewards()
        {
            return new List<AmmoRewardConfig>
            {
                new AmmoRewardConfig(ItemType.Ammo9x19, 4, 20, 80),
                new AmmoRewardConfig(ItemType.Ammo556x45, 3, 20, 70),
                new AmmoRewardConfig(ItemType.Ammo762x39, 3, 20, 70),
                new AmmoRewardConfig(ItemType.Ammo44cal, 2, 12, 36),
                new AmmoRewardConfig(ItemType.Ammo12gauge, 2, 8, 28)
            };
        }

        private static List<WeightedRoleConfig> CreateRoleRewards()
        {
            return new List<WeightedRoleConfig>
            {
                new WeightedRoleConfig(RoleTypeId.ClassD, 4),
                new WeightedRoleConfig(RoleTypeId.Scientist, 4),
                new WeightedRoleConfig(RoleTypeId.FacilityGuard, 3),
                new WeightedRoleConfig(RoleTypeId.NtfPrivate, 2),
                new WeightedRoleConfig(RoleTypeId.NtfSpecialist, 2),
                new WeightedRoleConfig(RoleTypeId.ChaosConscript, 2),
                new WeightedRoleConfig(RoleTypeId.ChaosRifleman, 2),
                new WeightedRoleConfig(RoleTypeId.ChaosMarauder, 1),
                new WeightedRoleConfig(RoleTypeId.Scp0492, 1)
            };
        }

        private static List<WeightedCandyConfig> CreateCandyRewards()
        {
            return new List<WeightedCandyConfig>
            {
                new WeightedCandyConfig(CandyKindID.Blue, 4),
                new WeightedCandyConfig(CandyKindID.Green, 4),
                new WeightedCandyConfig(CandyKindID.Yellow, 4),
                new WeightedCandyConfig(CandyKindID.Red, 3),
                new WeightedCandyConfig(CandyKindID.Purple, 3),
                new WeightedCandyConfig(CandyKindID.White, 2),
                new WeightedCandyConfig(CandyKindID.Rainbow, 1),
                new WeightedCandyConfig(CandyKindID.Pink, 1),
                new WeightedCandyConfig(CandyKindID.Evil, 1)
            };
        }

        private static List<FacilityZone> CreateTeleportZones()
        {
            return new List<FacilityZone>
            {
                FacilityZone.LightContainment,
                FacilityZone.HeavyContainment,
                FacilityZone.Entrance,
                FacilityZone.Surface
            };
        }
    }
}
