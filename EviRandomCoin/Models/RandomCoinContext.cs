using LabApi.Features.Wrappers;

namespace EviRandomCoin.Models
{
    public sealed class RandomCoinContext
    {
        public RandomCoinContext(RandomCoinConfig config, Player player, CoinItem coinItem, bool isTails, OutcomeConfig outcome, System.Random random)
        {
            Config = config;
            Player = player;
            CoinItem = coinItem;
            IsTails = isTails;
            Outcome = outcome;
            Random = random;
            Detail = string.Empty;
            Success = true;
        }

        public RandomCoinConfig Config { get; private set; }

        public Player Player { get; private set; }

        public CoinItem CoinItem { get; private set; }

        public bool IsTails { get; private set; }

        public OutcomeConfig Outcome { get; private set; }

        public System.Random Random { get; private set; }

        public string Detail { get; set; }

        public bool Success { get; set; }
    }
}
