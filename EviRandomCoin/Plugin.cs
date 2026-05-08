using System;
using EviRandomCoin.Handlers;
using EviRandomCoin.Services;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;

namespace EviRandomCoin
{
    public sealed class Plugin : Plugin<RandomCoinConfig>
    {
        private CoinEventHandler _coinEventHandler;

        public override string Name
        {
            get { return "EviRandomCoin"; }
        }

        public override string Author
        {
            get { return "EviZi1337"; }
        }

        public override string Description
        {
            get { return "Weighted random mechanics when a player flips a coin."; }
        }

        public override Version Version
        {
            get { return new Version(1, 0, 0); }
        }

        public override Version RequiredApiVersion
        {
            get { return LabApiProperties.CurrentVersion; }
        }

        public override void Enable()
        {
            _coinEventHandler = new CoinEventHandler(Config, new CoinOutcomeService(Config));
            _coinEventHandler.Register();
        }

        public override void Disable()
        {
            if (_coinEventHandler != null)
            {
                _coinEventHandler.Unregister();
                _coinEventHandler = null;
            }
        }
    }
}
