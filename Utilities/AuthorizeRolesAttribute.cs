using Microsoft.AspNetCore.Authorization;

namespace PuppyLearn.Utilities
{
    public class AuthorizeRolesAttribute: AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }

    }
}
