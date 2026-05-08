using InventorySystem.Items.Usables.Scp330;

namespace EviRandomCoin.Models
{
    public sealed class WeightedCandyConfig
    {
        public WeightedCandyConfig()
        {
            Candy = CandyKindID.Blue;
            Weight = 1d;
        }

        public WeightedCandyConfig(CandyKindID candy, double weight)
        {
            Candy = candy;
            Weight = weight;
        }

        public CandyKindID Candy { get; set; }

        public double Weight { get; set; }
    }
}
