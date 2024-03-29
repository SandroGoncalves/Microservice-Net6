using AutoMapper;
using GeekShopping.ProductApi.Config;
using GeekShopping.ProductApi.Model.Context;
using GeekShopping.ProductApi.Repository;
using GeekShopping.ProductApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connection = builder.Configuration["MySqlConnection:MysqlConnectionString"];

// DB config
builder.Services.AddDbContext<MySqlContext>(options => 
options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 32))));

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

// Dependence injection config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();

// Security config
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerConnection:IdentityServer"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "geek_shopping");
    });
} );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerOptions =>
{
    SwaggerOptions.EnableAnnotations();
    SwaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.productAPI", Version = "v1" });
    SwaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    SwaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
             new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection(); // Https Config
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();