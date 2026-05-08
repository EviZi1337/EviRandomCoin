using System;
using System.Collections.Generic;
using EviRandomCoin.Services;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace EviRandomCoin.Handlers
{
    public sealed class CoinEventHandler
    {
        private readonly RandomCoinConfig _config;
        private readonly CoinOutcomeService _outcomeService;
        private readonly Dictionary<int, DateTime> _lastFlipByPlayer;

        public CoinEventHandler(RandomCoinConfig config, CoinOutcomeService outcomeService)
        {
            _config = config;
            _outcomeService = outcomeService;
            _lastFlipByPlayer = new Dictionary<int, DateTime>();
        }

        public void Register()
        {
            PlayerEvents.FlippedCoin += OnFlippedCoin;
            PlayerEvents.Left += OnPlayerLeft;
        }

        public void Unregister()
        {
            PlayerEvents.FlippedCoin -= OnFlippedCoin;
            PlayerEvents.Left -= OnPlayerLeft;
            _lastFlipByPlayer.Clear();
        }

        private void OnFlippedCoin(PlayerFlippedCoinEventArgs ev)
        {
            try
            {
                if (ev == null || ev.Player == null)
                    return;

                Player player = ev.Player;

                if (_config.RequireRoundInProgress && !Round.IsRoundInProgress)
                {
                    player.SendHint(_config.Hints.RoundLocked, _config.HintDuration);
                    return;
                }

                if (_config.AlivePlayersOnly && !player.IsAlive)
                {
                    player.SendHint(_config.Hints.InvalidPlayerState, _config.HintDuration);
                    return;
                }

                if (IsOnCooldown(player))
                {
                    player.SendHint(_config.Hints.Cooldown, _config.HintDuration);
                    return;
                }

                _lastFlipByPlayer[player.PlayerId] = DateTime.UtcNow;
                _outcomeService.Apply(player, ev.CoinItem, ev.IsTails);
            }
            catch (Exception ex)
            {
                Logger.Error("EviRandomCoin failed to process coin flip: " + ex);
            }
        }

        private void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            if (ev == null || ev.Player == null)
                return;

            _lastFlipByPlayer.Remove(ev.Player.PlayerId);
        }

        private bool IsOnCooldown(Player player)
        {
            if (_config.CooldownSeconds <= 0f)
                return false;

            DateTime lastFlip;
            if (!_lastFlipByPlayer.TryGetValue(player.PlayerId, out lastFlip))
                return false;

            return (DateTime.UtcNow - lastFlip).TotalSeconds < _config.CooldownSeconds;
        }
    }
}
