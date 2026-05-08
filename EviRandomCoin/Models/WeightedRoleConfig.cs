using PlayerRoles;

namespace EviRandomCoin.Models
{
    public sealed class WeightedRoleConfig
    {
        public WeightedRoleConfig()
        {
            Role = RoleTypeId.ClassD;
            Weight = 1d;
        }

        public WeightedRoleConfig(RoleTypeId role, double weight)
        {
            Role = role;
            Weight = weight;
        }

        public RoleTypeId Role { get; set; }

        public double Weight { get; set; }
    }
}
