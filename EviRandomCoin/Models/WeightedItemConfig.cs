namespace EviRandomCoin.Models
{
    public sealed class WeightedItemConfig
    {
        public WeightedItemConfig()
        {
            Item = ItemType.None;
            Weight = 1d;
        }

        public WeightedItemConfig(ItemType item, double weight)
        {
            Item = item;
            Weight = weight;
        }

        public ItemType Item { get; set; }

        public double Weight { get; set; }
    }
}
