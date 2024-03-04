using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.ModelDB
{
    public class ApplicationUser : IdentityUser
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}
