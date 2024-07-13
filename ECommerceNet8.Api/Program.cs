using ECommerceNet8.Core.DTOS.PaymentDTo;
using ECommerceNet8.Core.Reposiatories.AddressRepository;
using ECommerceNet8.Core.Reposiatories.AuthReposaitory;
using ECommerceNet8.Core.Reposiatories.BaseProductsReposaitory;
using ECommerceNet8.Core.Reposiatories.MainCategoryReposaitory;
using ECommerceNet8.Core.Reposiatories.MaterialReposaitory;
using ECommerceNet8.Core.Reposiatories.OrderRepository;
using ECommerceNet8.Core.Reposiatories.PaymentReposaitory;
using ECommerceNet8.Core.Reposiatories.RedisCartReposaitory;

using ECommerceNet8.Core.Reposiatories.ShoppingcartReposaitory;

using ECommerceNet8.Core.Services;
using ECommerceNet8.Core.Settings;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Description = "Please Enter Token",
        In = ParameterLocation.Header,
        Name = "Authorization",

        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailingService, MailingService>();
builder.Services.AddScoped<IAuthReposaitory, AuthReposaitory>();
builder.Services.AddScoped<IMainCategory, MainCategory>();
builder.Services.AddScoped<IMaterialReposaitory, MaterialReposaitory>();
builder.Services.AddScoped<IBaseProducts, BaseProducts>();
//builder.Services.AddScoped<IShoppingcartReposaitory, ShoppingcartReposaitory>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddTransient<IRedisCartService, RedisCartService>();

builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(op =>
{
    op.Password.RequireNonAlphanumeric = true;
    op.Password.RequireUppercase = true;
    op.Password.RequireLowercase = true;
    op.Password.RequiredLength = 8;
    op.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("ProtoflioLogin")
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
