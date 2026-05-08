namespace EviRandomCoin.Models
{
    public sealed class WeightedEffectConfig
    {
        public WeightedEffectConfig()
        {
            Id = string.Empty;
            Weight = 1d;
            MinIntensity = 1;
            MaxIntensity = 1;
            MinDuration = 5f;
            MaxDuration = 10f;
        }

        public WeightedEffectConfig(string id, double weight, byte minIntensity, byte maxIntensity, float minDuration, float maxDuration)
        {
            Id = id;
            Weight = weight;
            MinIntensity = minIntensity;
            MaxIntensity = maxIntensity;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
        }

        public string Id { get; set; }

        public double Weight { get; set; }

        public byte MinIntensity { get; set; }

        public byte MaxIntensity { get; set; }

        public float MinDuration { get; set; }

        public float MaxDuration { get; set; }
    }
}
