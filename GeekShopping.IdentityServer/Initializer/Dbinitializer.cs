using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer.Interface;
using GeekShopping.IdentityServer.ModelDB;
using GeekShopping.IdentityServer.ModelDB.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class Dbinitializer : IDbinitializer
    {
        private readonly MySqlContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _role;

        public Dbinitializer(MySqlContext context, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> role)
        {
            _context = context;
            _userManager = userManager;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfig.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfig.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfig.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "sandro.admin",
                Email = "sandro.admin@goncalves.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (51)91234-5678",
                FirstName = "Sandro",
                LastName = "Admin"
            };

            // To register adm and password
            _userManager.CreateAsync(admin, "Sandro123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(admin, IdentityConfig.Admin).GetAwaiter().GetResult();

            var adminsClaims = _userManager.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfig.Admin)
            }).Result;
            
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "sandro.client",
                Email = "sandro.client@goncalves.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (51)91234-5678",
                FirstName = "Sandro",
                LastName = "Client"
            };

            // To register adm and password
            _userManager.CreateAsync(client, "Sandro123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(client, IdentityConfig.Client).GetAwaiter().GetResult();

            var clientsClaims = _userManager.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfig.Client)
            }).Result;
        }
    }
}
