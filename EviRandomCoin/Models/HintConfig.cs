using System.ComponentModel;

namespace EviRandomCoin.Models
{
    public sealed class HintConfig
    {
        public HintConfig()
        {
            Outcome = "<b><color=#F8D66D>Random Coin</color></b>\n{detail}";
            Cooldown = "<b><color=#F8D66D>Random Coin</color></b>\n<color=#BDBDBD>Give the coin a second.</color>";
            RoundLocked = "<b><color=#F8D66D>Random Coin</color></b>\n<color=#BDBDBD>The coin is asleep until the round starts.</color>";
            InvalidPlayerState = "<b><color=#F8D66D>Random Coin</color></b>\n<color=#BDBDBD>Nothing happens.</color>";
            UnknownOutcome = "<color=#FF6B6B>The coin hit an unknown outcome: {id}</color>";
            Error = "<color=#FF6B6B>The coin cracked mid-flip.</color>";
            GoodEffect = "<color=#7CFF7C>{effect}</color> for <color=#8EC5FF>{duration}s</color>.";
            BadEffect = "<color=#FF7C7C>{effect}</color> for <color=#8EC5FF>{duration}s</color>.";
            CommonItem = "<color=#FFE083>You found {item}.</color>";
            RareItem = "<color=#FFB84D>Rare pull: {item}.</color>";
            Ammo = "<color=#B5E48C>{amount} ammo: {ammo}.</color>";
            Heal = "<color=#7CFF7C>Recovered {amount} HP.</color>";
            ArtificialHealth = "<color=#7CFFDD>Gained {amount} artificial HP.</color>";
            Damage = "<color=#FF7C7C>The coin bites for {amount} damage.</color>";
            Teleport = "<color=#8EC5FF>Teleported to {zone}.</color>";
            PlayerSwap = "<color=#8EC5FF>Swapped places with {player}.</color>";
            RoleChange = "<color=#FFD166>Role shifted into {role}.</color>";
            Candy = "<color=#FF9CEE>Candy granted: {candy}.</color>";
            InventoryWipe = "<color=#FF7C7C>Your inventory vanished.</color>";
            InventoryReroll = "<color=#FFE083>Inventory rerolled into {count} items.</color>";
            CoinCopy = "<color=#F8D66D>The coin multiplied into {count} coins.</color>";
            Blackout = "<color=#BDB2FF>Lights out for {duration}s.</color>";
            Launch = "<color=#8EC5FF>The coin launches you upward.</color>";
            ItemRain = "<color=#FFE083>{count} items spilled onto the floor.</color>";
            Nothing = "<color=#BDBDBD>The coin lands dramatically. Nothing else.</color>";
            InventoryFull = "<color=#FFB84D>Your inventory is full, so the reward dropped nearby.</color>";
            Failed = "<color=#BDBDBD>The coin tried, but the room said no.</color>";
        }

        [Description("Main wrapper for every successful outcome. Use {detail} for the outcome-specific message.")]
        public string Outcome { get; set; }

        [Description("Shown when the player flips again before CooldownSeconds expires.")]
        public string Cooldown { get; set; }

        [Description("Shown when RequireRoundInProgress blocks the coin.")]
        public string RoundLocked { get; set; }

        [Description("Shown when AlivePlayersOnly blocks the coin.")]
        public string InvalidPlayerState { get; set; }

        [Description("Shown when an outcome or effect id is unknown. Use {id}.")]
        public string UnknownOutcome { get; set; }

        [Description("Shown when an unexpected exception happens while applying an outcome.")]
        public string Error { get; set; }

        [Description("Shown after a positive status effect. Use {effect} and {duration}.")]
        public string GoodEffect { get; set; }

        [Description("Shown after a negative status effect. Use {effect} and {duration}.")]
        public string BadEffect { get; set; }

        [Description("Shown after a common item reward. Use {item}.")]
        public string CommonItem { get; set; }

        [Description("Shown after a rare item reward. Use {item}.")]
        public string RareItem { get; set; }

        [Description("Shown after an ammo reward. Use {amount} and {ammo}.")]
        public string Ammo { get; set; }

        [Description("Shown after healing. Use {amount}.")]
        public string Heal { get; set; }

        [Description("Shown after artificial health is granted. Use {amount}.")]
        public string ArtificialHealth { get; set; }

        [Description("Shown after coin damage. Use {amount}.")]
        public string Damage { get; set; }

        [Description("Shown after teleporting. Use {zone}.")]
        public string Teleport { get; set; }

        [Description("Shown after swapping positions with another player. Use {player}.")]
        public string PlayerSwap { get; set; }

        [Description("Shown after a role change. Use {role}.")]
        public string RoleChange { get; set; }

        [Description("Shown after SCP-330 candy is granted. Use {candy}.")]
        public string Candy { get; set; }

        [Description("Shown when the inventory is wiped.")]
        public string InventoryWipe { get; set; }

        [Description("Shown when the inventory is rerolled. Use {count}.")]
        public string InventoryReroll { get; set; }

        [Description("Shown when bonus coins are granted. Use {count}.")]
        public string CoinCopy { get; set; }

        [Description("Shown after a blackout. Use {duration}.")]
        public string Blackout { get; set; }

        [Description("Shown after the launch outcome.")]
        public string Launch { get; set; }

        [Description("Shown after item rain. Use {count}.")]
        public string ItemRain { get; set; }

        [Description("Shown when the random outcome intentionally does nothing.")]
        public string Nothing { get; set; }

        [Description("Appended when an item reward was dropped because inventory was full.")]
        public string InventoryFull { get; set; }

        [Description("Shown when an enabled outcome cannot find a valid target or configured reward.")]
        public string Failed { get; set; }
    }
}
