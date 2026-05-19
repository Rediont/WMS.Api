using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Interfaces;
using Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );
});


builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAwNDg5NjAwIiwiaWF0IjoiMTc2ODk4NTgzNSIsImFjY291bnRfaWQiOiIwMTliZGZjNTRhMjU3YWJhYmM3MjNmMDQxNWM1Yzk0NyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2Zmd2JlYnY4NGtoazluNnI1OThmYXc2Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.09FMeRc4FMA9MomKT1o_pDEpo1D6BNI8177EoWfLy-WPD-tbUs6jhEgXfVw928tjKoReuzwHy8DF3gN6qklaBDag0cotiAkOzuYlvM1OcQ7Eim7fJXipKTMAYIQN65M2Bvp31mS__hgkqA5KHOXlvGzdAIJP5bmnWIBzfGX4rm6-zF_McWiVMmyDD5XOCULVIzjFm99l5R7-DI-WfdrKCazui6o8sJFlolke5uEVlufQgcGZqkcT9NvERJ-Ufpq9pK0LkCYDa7MkEy-MNM80BKV5ObhMoXQxI972lUl4zgBFdPfpLoWlowOvasJw0LC_-LvFdlXO4kHPujvyxZ48cw";
    cfg.AddMaps(typeof(Services.Mappers.MapperProfile).Assembly);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    //o => o.MapEnum<ContractStatus>()));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Тут можна налаштувати вимоги до пароля (довжина, цифри тощо)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Заміни на ім'я твого DbContext
.AddDefaultTokenProviders();


var jwtSettings = builder.Configuration.GetSection("Jwt"); // Дані будемо брати з appsettings.json
var secretKey = jwtSettings["Key"]; // Це секретний ключ для підпису токенів

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // В продакшені має бути true
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidIssuer = jwtSettings["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IAlleyService, AlleyService>();
builder.Services.AddScoped<ICellService, CellService>();
builder.Services.AddScoped<IClientService,ClientService>();
builder.Services.AddScoped<IWmsDocumentService, WmsDocumentService>();
builder.Services.AddScoped<IPalletTypeService, PalletTypeService>();
builder.Services.AddScoped<IPalletService, PalletService>();
builder.Services.AddScoped<IPalletBindingService, PalletBindingService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IInventoryBalanceService, InventoryBalanceService>();
builder.Services.AddScoped<ISectorService, SectorService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IWarehouseSettingsService, WarehouseSettingsService>();
builder.Services.AddScoped<IWarehouseSlottingService, WarehouseSlottingService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddMemoryCache();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await RoleSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Помилка під час створення початкових ролей.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
