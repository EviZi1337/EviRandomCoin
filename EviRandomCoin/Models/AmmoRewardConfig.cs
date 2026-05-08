namespace EviRandomCoin.Models
{
    public sealed class AmmoRewardConfig
    {
        public AmmoRewardConfig()
        {
            AmmoType = ItemType.Ammo9x19;
            Weight = 1d;
            MinAmount = 10;
            MaxAmount = 40;
        }

        public AmmoRewardConfig(ItemType ammoType, double weight, ushort minAmount, ushort maxAmount)
        {
            AmmoType = ammoType;
            Weight = weight;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public ItemType AmmoType { get; set; }

        public double Weight { get; set; }

        public ushort MinAmount { get; set; }

        public ushort MaxAmount { get; set; }
    }
}
