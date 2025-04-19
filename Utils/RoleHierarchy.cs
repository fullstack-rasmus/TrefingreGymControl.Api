using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Utils
{
    public class RoleHierarchy
    {
        private static readonly Dictionary<string, int> RoleRanks = new()
        {
            { "Admin", 3 },
            { "Manager", 2 },
            { "User", 1 }
        };

        public static bool HasAtLeastRole(string userRole, string requiredRole)
        {
            return RoleRanks[userRole] >= RoleRanks[requiredRole];
        }
    }
}