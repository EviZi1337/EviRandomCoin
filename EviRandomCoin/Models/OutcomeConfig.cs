namespace EviRandomCoin.Models
{
    public sealed class OutcomeConfig
    {
        public OutcomeConfig()
        {
            Id = string.Empty;
            Weight = 1d;
            Enabled = true;
            ConsumeCoin = null;
        }

        public OutcomeConfig(string id, double weight, bool enabled, bool? consumeCoin)
        {
            Id = id;
            Weight = weight;
            Enabled = enabled;
            ConsumeCoin = consumeCoin;
        }

        public string Id { get; set; }

        public double Weight { get; set; }

        public bool Enabled { get; set; }

        public bool? ConsumeCoin { get; set; }
    }
}
