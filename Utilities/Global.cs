using System.Runtime.CompilerServices;

namespace PuppyLearn.Utilities
{
    public static class Global
    {
        public static string GetAccountTypeStr(int index)
        {
            if (index == 1)
            {
                return Roles.normalUser;
            }
            else if (index == 2) {
                return Roles.vip;
            }
            else if (index == 3)
            {
                return Roles.teacher;
            }
            else if (index == 4)
            {
                return Roles.admin;
            }
            else
            {
                return Roles.superAdmin;
            }
        }
    }

    
}
