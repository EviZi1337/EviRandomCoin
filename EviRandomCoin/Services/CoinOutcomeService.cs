using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using EviRandomCoin.Models;
using InventorySystem.Items;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace EviRandomCoin.Services
{
    public sealed class CoinOutcomeService
    {
        private delegate void OutcomeHandler(RandomCoinContext context);

        private delegate void EffectApplier(Player player, byte intensity, float duration);

        private readonly RandomCoinConfig _config;
        private readonly System.Random _random;
        private readonly Dictionary<string, OutcomeHandler> _handlers;
        private readonly Dictionary<string, EffectApplier> _effectAppliers;
        private readonly List<OutcomeConfig> _activeOutcomes;

        public CoinOutcomeService(RandomCoinConfig config)
        {
            _config = config;
            _random = new System.Random();
            _handlers = new Dictionary<string, OutcomeHandler>(StringComparer.OrdinalIgnoreCase);
            _effectAppliers = new Dictionary<string, EffectApplier>(StringComparer.OrdinalIgnoreCase);
            _activeOutcomes = new List<OutcomeConfig>();
            RegisterHandlers();
            RegisterEffects();
            CacheActiveOutcomes();
            ValidateEffectPool("GoodEffects", _config.GoodEffects);
            ValidateEffectPool("BadEffects", _config.BadEffects);
        }

        public void Apply(Player player, CoinItem coinItem, bool isTails)
        {
            OutcomeConfig outcome = PickOutcome();
            if (outcome == null)
            {
                player.SendHint(Format(_config.Hints.Outcome, _config.Hints.Failed), _config.HintDuration);
                return;
            }

            OutcomeHandler handler;
            if (!_handlers.TryGetValue(outcome.Id, out handler))
            {
                player.SendHint(Format(_config.Hints.Outcome, Replace(_config.Hints.UnknownOutcome, "{id}", outcome.Id)), _config.HintDuration);
                Logger.Warn("Unknown EviRandomCoin outcome: " + outcome.Id);
                return;
            }

            RandomCoinContext context = new RandomCoinContext(_config, player, coinItem, isTails, outcome, _random);

            try
            {
                handler(context);

                if (string.IsNullOrEmpty(context.Detail))
                    context.Detail = _config.Hints.Failed;

                if (context.Success && ShouldConsumeCoin(outcome))
                    ConsumeCoin(player, coinItem);

                player.SendHint(Format(_config.Hints.Outcome, context.Detail), _config.HintDuration);
                Logger.Debug("Outcome " + outcome.Id + " applied to " + player.LogName + ".", _config.Debug);
            }
            catch (Exception ex)
            {
                Logger.Error("EviRandomCoin outcome failed: " + ex);
                player.SendHint(Format(_config.Hints.Outcome, _config.Hints.Error), _config.HintDuration);
            }
        }

        private void RegisterHandlers()
        {
            _handlers["good_effect"] = ApplyGoodEffect;
            _handlers["bad_effect"] = ApplyBadEffect;
            _handlers["common_item"] = ApplyCommonItem;
            _handlers["rare_item"] = ApplyRareItem;
            _handlers["ammo"] = ApplyAmmo;
            _handlers["heal"] = ApplyHeal;
            _handlers["artificial_health"] = ApplyArtificialHealth;
            _handlers["damage"] = ApplyDamage;
            _handlers["teleport"] = ApplyTeleport;
            _handlers["player_swap"] = ApplyPlayerSwap;
            _handlers["role_change"] = ApplyRoleChange;
            _handlers["candy"] = ApplyCandy;
            _handlers["inventory_wipe"] = ApplyInventoryWipe;
            _handlers["inventory_reroll"] = ApplyInventoryReroll;
            _handlers["coin_copy"] = ApplyCoinCopy;
            _handlers["blackout"] = ApplyBlackout;
            _handlers["launch"] = ApplyLaunch;
            _handlers["item_rain"] = ApplyItemRain;
            _handlers["nothing"] = ApplyNothing;
        }

        private void RegisterEffects()
        {
            _effectAppliers["invisible"] = (p, i, d) => p.EnableEffect<Invisible>(i, d);
            _effectAppliers["movement_boost"] = (p, i, d) => p.EnableEffect<MovementBoost>(i, d);
            _effectAppliers["silent_walk"] = (p, i, d) => p.EnableEffect<SilentWalk>(i, d);
            _effectAppliers["invigorated"] = (p, i, d) => p.EnableEffect<Invigorated>(i, d);
            _effectAppliers["night_vision"] = (p, i, d) => p.EnableEffect<NightVision>(i, d);
            _effectAppliers["damage_reduction"] = (p, i, d) => p.EnableEffect<DamageReduction>(i, d);
            _effectAppliers["vitality"] = (p, i, d) => p.EnableEffect<Vitality>(i, d);
            _effectAppliers["lightweight"] = (p, i, d) => p.EnableEffect<Lightweight>(i, d);
            _effectAppliers["rainbow_taste"] = (p, i, d) => p.EnableEffect<RainbowTaste>(i, d);
            _effectAppliers["amnesia_vision"] = (p, i, d) => p.EnableEffect<AmnesiaVision>(i, d);
            _effectAppliers["bleeding"] = (p, i, d) => p.EnableEffect<Bleeding>(i, d);
            _effectAppliers["burned"] = (p, i, d) => p.EnableEffect<Burned>(i, d);
            _effectAppliers["poisoned"] = (p, i, d) => p.EnableEffect<Poisoned>(i, d);
            _effectAppliers["flashed"] = (p, i, d) => p.EnableEffect<Flashed>(i, d);
            _effectAppliers["blindness"] = (p, i, d) => p.EnableEffect<Blindness>(i, d);
            _effectAppliers["deafened"] = (p, i, d) => p.EnableEffect<Deafened>(i, d);
            _effectAppliers["concussed"] = (p, i, d) => p.EnableEffect<Concussed>(i, d);
            _effectAppliers["exhausted"] = (p, i, d) => p.EnableEffect<Exhausted>(i, d);
            _effectAppliers["heavy_footed"] = (p, i, d) => p.EnableEffect<HeavyFooted>(i, d);
            _effectAppliers["hemorrhage"] = (p, i, d) => p.EnableEffect<Hemorrhage>(i, d);
            _effectAppliers["stained"] = (p, i, d) => p.EnableEffect<Stained>(i, d);
        }

        private void CacheActiveOutcomes()
        {
            _activeOutcomes.Clear();

            if (_config.Outcomes == null)
                return;

            foreach (OutcomeConfig outcome in _config.Outcomes)
            {
                if (outcome == null || !outcome.Enabled || outcome.Weight <= 0d || string.IsNullOrEmpty(outcome.Id))
                    continue;

                if (!_handlers.ContainsKey(outcome.Id))
                {
                    Logger.Warn("EviRandomCoin config contains unknown outcome id: " + outcome.Id);
                    continue;
                }

                _activeOutcomes.Add(outcome);
            }

            if (_activeOutcomes.Count == 0)
                Logger.Warn("EviRandomCoin has no enabled valid outcomes.");
        }

        private void ValidateEffectPool(string poolName, List<WeightedEffectConfig> effects)
        {
            if (effects == null)
                return;

            foreach (WeightedEffectConfig effect in effects)
            {
                if (effect == null || string.IsNullOrEmpty(effect.Id) || effect.Weight <= 0d)
                    continue;

                if (!_effectAppliers.ContainsKey(effect.Id))
                    Logger.Warn("EviRandomCoin " + poolName + " contains unknown effect id: " + effect.Id);
            }
        }

        private OutcomeConfig PickOutcome()
        {
            return RandomSelector.Pick(_activeOutcomes, outcome => outcome.Weight, _random);
        }

        private void ApplyGoodEffect(RandomCoinContext context)
        {
            ApplyEffectPool(context, _config.GoodEffects, _config.Hints.GoodEffect);
        }

        private void ApplyBadEffect(RandomCoinContext context)
        {
            ApplyEffectPool(context, _config.BadEffects, _config.Hints.BadEffect);
        }

        private void ApplyCommonItem(RandomCoinContext context)
        {
            WeightedItemConfig item = RandomSelector.Pick(_config.CommonItems, reward => reward.Weight, _random);
            if (item == null || item.Item == ItemType.None)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            bool dropped = false;
            if (!GiveItem(context.Player, item.Item, out dropped))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            string detail = Replace(_config.Hints.CommonItem, "{item}", item.Item.ToString());
            context.Detail = dropped ? detail + "\n" + _config.Hints.InventoryFull : detail;
        }

        private void ApplyRareItem(RandomCoinContext context)
        {
            WeightedItemConfig item = RandomSelector.Pick(_config.RareItems, reward => reward.Weight, _random);
            if (item == null || item.Item == ItemType.None)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            bool dropped = false;
            if (!GiveItem(context.Player, item.Item, out dropped))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            string detail = Replace(_config.Hints.RareItem, "{item}", item.Item.ToString());
            context.Detail = dropped ? detail + "\n" + _config.Hints.InventoryFull : detail;
        }

        private void ApplyAmmo(RandomCoinContext context)
        {
            AmmoRewardConfig reward = RandomSelector.Pick(_config.AmmoRewards, ammo => ammo.Weight, _random);
            if (reward == null)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            ushort amount = NextUShort(reward.MinAmount, reward.MaxAmount);
            context.Player.AddAmmo(reward.AmmoType, amount);
            context.Detail = Replace(Replace(_config.Hints.Ammo, "{amount}", amount.ToString()), "{ammo}", reward.AmmoType.ToString());
        }

        private void ApplyHeal(RandomCoinContext context)
        {
            float amount = NextFloat(_config.MinHealAmount, _config.MaxHealAmount);
            context.Player.Heal(amount);
            context.Detail = Replace(_config.Hints.Heal, "{amount}", FormatAmount(amount));
        }

        private void ApplyArtificialHealth(RandomCoinContext context)
        {
            float current = context.Player.ArtificialHealth;
            float amount = NextFloat(_config.MinArtificialHealthAmount, _config.MaxArtificialHealthAmount);
            float target = Math.Min(context.Player.MaxArtificialHealth, current + amount);
            float gained = Math.Max(0f, target - current);
            context.Player.ArtificialHealth = target;
            context.Detail = Replace(_config.Hints.ArtificialHealth, "{amount}", FormatAmount(gained));
        }

        private void ApplyDamage(RandomCoinContext context)
        {
            float amount = NextFloat(_config.MinDamageAmount, _config.MaxDamageAmount);
            if (!_config.BadLuckCanKill && context.Player.Health - amount < 1f)
                amount = Math.Max(0f, context.Player.Health - 1f);

            if (amount > 0f)
                context.Player.Damage(amount, _config.DamageReason);

            context.Detail = Replace(_config.Hints.Damage, "{amount}", FormatAmount(amount));
        }

        private void ApplyTeleport(RandomCoinContext context)
        {
            if (!_config.AllowTeleport || (_config.HumansOnlyForTeleport && !context.Player.IsHuman))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            List<Room> rooms = GetTeleportRooms(context.Player);
            Room room = RandomSelector.Pick(rooms, candidate => 1d, _random);
            if (room == null)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Player.Position = room.Position + Vector3.up * _config.TeleportVerticalOffset;
            context.Detail = Replace(_config.Hints.Teleport, "{zone}", room.Zone.ToString());
        }

        private void ApplyPlayerSwap(RandomCoinContext context)
        {
            if (!_config.AllowPlayerSwap || (_config.HumansOnlyForPlayerSwap && !context.Player.IsHuman))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            List<Player> candidates = new List<Player>();
            foreach (Player other in Player.ReadyList)
            {
                if (other == null || other.PlayerId == context.Player.PlayerId || !other.IsAlive)
                    continue;

                if (_config.HumansOnlyForPlayerSwap && !other.IsHuman)
                    continue;

                candidates.Add(other);
            }

            Player target = RandomSelector.Pick(candidates, candidate => 1d, _random);
            if (target == null)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            Vector3 original = context.Player.Position;
            context.Player.Position = target.Position;
            target.Position = original;
            context.Detail = Replace(_config.Hints.PlayerSwap, "{player}", target.DisplayName);
        }

        private void ApplyRoleChange(RandomCoinContext context)
        {
            if (!_config.AllowRoleChanges || (_config.HumansOnlyForRoleChanges && !context.Player.IsHuman))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            WeightedRoleConfig role = RandomSelector.Pick(_config.RoleRewards, reward => reward.Weight, _random);
            if (role == null)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Player.SetRole(role.Role, RoleChangeReason.RemoteAdmin, _config.RoleChangeSpawnFlags);
            context.Detail = Replace(_config.Hints.RoleChange, "{role}", role.Role.ToString());
        }

        private void ApplyCandy(RandomCoinContext context)
        {
            WeightedCandyConfig candy = RandomSelector.Pick(_config.CandyRewards, reward => reward.Weight, _random);
            if (candy == null || candy.Candy == CandyKindID.None)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Player.GiveCandy(candy.Candy, ItemAddReason.AdminCommand);
            context.Detail = Replace(_config.Hints.Candy, "{candy}", candy.Candy.ToString());
        }

        private void ApplyInventoryWipe(RandomCoinContext context)
        {
            if (!_config.AllowInventoryPunishments)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Player.ClearInventory(true, true);
            context.Detail = _config.Hints.InventoryWipe;
        }

        private void ApplyInventoryReroll(RandomCoinContext context)
        {
            if (!_config.AllowInventoryPunishments)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            int count = NextInt(_config.MinRerollItems, _config.MaxRerollItems);
            int given = 0;
            context.Player.ClearInventory(true, true);

            for (int i = 0; i < count; i++)
            {
                List<WeightedItemConfig> pool = _config.CommonItems;
                if (_config.RerollRareItemChance > 0f && _config.RareItems != null && _config.RareItems.Count > 0 && _random.NextDouble() < _config.RerollRareItemChance)
                    pool = _config.RareItems;

                WeightedItemConfig item = RandomSelector.Pick(pool, reward => reward.Weight, _random);
                if (item == null || item.Item == ItemType.None)
                    continue;

                bool dropped;
                if (GiveItem(context.Player, item.Item, out dropped))
                    given++;
            }

            if (given == 0)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Detail = Replace(_config.Hints.InventoryReroll, "{count}", given.ToString());
        }

        private void ApplyCoinCopy(RandomCoinContext context)
        {
            int count = NextInt(_config.MinBonusCoins, _config.MaxBonusCoins);
            int given = 0;

            for (int i = 0; i < count; i++)
            {
                bool dropped;
                if (GiveItem(context.Player, ItemType.Coin, out dropped))
                    given++;
            }

            if (given == 0)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Detail = Replace(_config.Hints.CoinCopy, "{count}", given.ToString());
        }

        private void ApplyBlackout(RandomCoinContext context)
        {
            if (!_config.AllowBlackout)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            float duration = NextFloat(_config.MinBlackoutDuration, _config.MaxBlackoutDuration);
            FacilityZone zone = context.Player.Zone;

            if (zone == FacilityZone.LightContainment || zone == FacilityZone.HeavyContainment || zone == FacilityZone.Entrance)
                Map.TurnOffLights(duration, zone);
            else
                Map.TurnOffLights(duration);

            context.Detail = Replace(_config.Hints.Blackout, "{duration}", FormatAmount(duration));
        }

        private void ApplyLaunch(RandomCoinContext context)
        {
            context.Player.Jump(_config.LaunchStrength);
            context.Detail = _config.Hints.Launch;
        }

        private void ApplyItemRain(RandomCoinContext context)
        {
            int count = NextInt(_config.MinRainItems, _config.MaxRainItems);
            int spawned = 0;

            for (int i = 0; i < count; i++)
            {
                WeightedItemConfig item = RandomSelector.Pick(_config.CommonItems, reward => reward.Weight, _random);
                if (item == null || item.Item == ItemType.None)
                    continue;

                if (SpawnPickupNear(context.Player, item.Item))
                    spawned++;
            }

            if (spawned == 0)
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            context.Detail = Replace(_config.Hints.ItemRain, "{count}", spawned.ToString());
        }

        private void ApplyNothing(RandomCoinContext context)
        {
            context.Detail = _config.Hints.Nothing;
        }

        private void ApplyEffectPool(RandomCoinContext context, List<WeightedEffectConfig> effects, string template)
        {
            WeightedEffectConfig effect = RandomSelector.Pick(effects, item => item.Weight, _random);
            if (effect == null || string.IsNullOrEmpty(effect.Id))
            {
                context.Detail = _config.Hints.Failed;
                context.Success = false;
                return;
            }

            EffectApplier applier;
            if (!_effectAppliers.TryGetValue(effect.Id, out applier))
            {
                context.Detail = Replace(_config.Hints.UnknownOutcome, "{id}", effect.Id);
                context.Success = false;
                return;
            }

            byte intensity = NextByte(effect.MinIntensity, effect.MaxIntensity);
            float duration = NextFloat(effect.MinDuration, effect.MaxDuration);
            applier(context.Player, intensity, duration);
            context.Detail = Replace(Replace(template, "{effect}", PrettyName(effect.Id)), "{duration}", FormatAmount(duration));
        }

        private bool GiveItem(Player player, ItemType item, out bool dropped)
        {
            dropped = false;

            if (!player.IsInventoryFull)
                return player.AddItem(item) != null;

            if (!_config.DropItemsWhenInventoryFull)
                return false;

            dropped = true;
            return SpawnPickupNear(player, item);
        }

        private bool SpawnPickupNear(Player player, ItemType item)
        {
            Vector3 offset = new Vector3(NextFloat(-_config.ItemRainRadius, _config.ItemRainRadius), 0.4f, NextFloat(-_config.ItemRainRadius, _config.ItemRainRadius));
            Pickup pickup = Pickup.Create(item, player.Position + offset, Quaternion.identity);
            if (pickup == null)
                return false;

            if (!pickup.IsSpawned)
                pickup.Spawn();

            return true;
        }

        private List<Room> GetTeleportRooms(Player player)
        {
            List<Room> rooms = new List<Room>();
            foreach (Room room in Map.Rooms)
            {
                if (room == null || room.IsDestroyed)
                    continue;

                if (_config.TeleportZones != null && _config.TeleportZones.Count > 0 && !_config.TeleportZones.Contains(room.Zone))
                    continue;

                if (player.Room != null && player.Room == room)
                    continue;

                rooms.Add(room);
            }

            return rooms;
        }

        private void ConsumeCoin(Player player, CoinItem coinItem)
        {
            if (coinItem != null && !coinItem.IsDestroyed && coinItem.CurrentOwner == player)
            {
                player.RemoveItem(coinItem);
                return;
            }

            player.RemoveItem(ItemType.Coin);
        }

        private bool ShouldConsumeCoin(OutcomeConfig outcome)
        {
            if (outcome.ConsumeCoin.HasValue)
                return outcome.ConsumeCoin.Value;

            return _config.ConsumeCoinByDefault;
        }

        private int NextInt(int min, int max)
        {
            if (max < min)
            {
                int tmp = min;
                min = max;
                max = tmp;
            }

            return _random.Next(min, max + 1);
        }

        private ushort NextUShort(ushort min, ushort max)
        {
            if (max < min)
            {
                ushort tmp = min;
                min = max;
                max = tmp;
            }

            return (ushort)_random.Next(min, max + 1);
        }

        private byte NextByte(byte min, byte max)
        {
            if (max < min)
            {
                byte tmp = min;
                min = max;
                max = tmp;
            }

            return (byte)_random.Next(min, max + 1);
        }

        private float NextFloat(float min, float max)
        {
            if (max < min)
            {
                float tmp = min;
                min = max;
                max = tmp;
            }

            return min + (float)_random.NextDouble() * (max - min);
        }

        private static string Format(string template, string detail)
        {
            return Replace(template, "{detail}", detail);
        }

        private static string Replace(string template, string key, string value)
        {
            if (template == null)
                return string.Empty;

            return template.Replace(key, value ?? string.Empty);
        }

        private static string PrettyName(string id)
        {
            if (string.IsNullOrEmpty(id))
                return string.Empty;

            string[] parts = id.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0)
                    continue;

                parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
            }

            return string.Join(" ", parts);
        }

        private static string FormatAmount(float amount)
        {
            return Math.Round(amount, 1).ToString("0.#");
        }
    }
}
