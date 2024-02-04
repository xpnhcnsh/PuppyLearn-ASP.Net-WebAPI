namespace PuppyLearn.Utilities
{
    public static class Global
    {
        public static string GetAccountTypeStr(int index)
        {
            if (index == 1)
            {
                return "normalUser";
            }
            else if(index == 2){
                return "vip";
            }
            else if(index == 3)
            {
                return "teacher";
            }
            else if(index == 4)
            {
                return "admin";
            }
            else
            {
                return "superAdmin";
            }
        }
            
        
    }
}
