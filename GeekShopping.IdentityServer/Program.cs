using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Initializer.Interface;
using GeekShopping.IdentityServer.ModelDB;
using GeekShopping.IdentityServer.ModelDB.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB config
var connection = builder.Configuration["MySqlConnection:MysqlConnectionString"];

builder.Services.AddDbContext<MySqlContext>(options =>
options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 32))));

// Identity config
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySqlContext>()
    .AddDefaultTokenProviders();

// Security config 
var builderSecurity = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(IdentityConfig.IdentityResource)
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    .AddInMemoryClients(IdentityConfig.Clients)
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbinitializer, Dbinitializer>();

builderSecurity.AddDeveloperSigningCredential();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetService<IDbinitializer>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection(); // Https Config
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

service.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
