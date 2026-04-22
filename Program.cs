using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Core.Services;
using PharmaStock.Core.Validators;
using PharmaStock.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PharmaStock.Core.Validators.Auth;
using PharmaStock.Core.Validators.Location;
using PharmaStock.Infrastructure.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// CORS Policy for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Validator registration
builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();

// JWT Token Registration
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UpsertUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateLocationValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string[] {}
            }
    });
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//if not typeof, you would have to specify the type of repository you want to use, but with typeof, you can use any repository you want by just passing the type of it as T.

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDrugRepository, DrugRepository>();

builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
builder.Services.AddScoped<IPurchaseItemService, PurchaseItemService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddScoped<IBinRepository, BinRepository>();
builder.Services.AddScoped<IBinService, BinService>();

builder.Services.AddScoped<IGrnRepository, GrnRepository>();
builder.Services.AddScoped<IGrnService, GrnService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<ILocationService, LocationService>();


builder.Services.AddTransient<IAuditLogService, AuditLogService>();
builder.Services.AddTransient<IAuditLogRepository, AuditLogRepository>();

builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorService, VendorService>();

builder.Services.AddScoped<ITransferOrderRepository, TransferOrderRepository>();
builder.Services.AddScoped<ITransferOrderService, TransferOrderService>();
builder.Services.AddScoped<IGRNItemRepository, GRNItemRepository>();
builder.Services.AddScoped<IGRNItemService, GRNItemService>();
builder.Services.AddScoped<IInventoryLotService, InventoryLotService>();
builder.Services.AddScoped<IInventoryLotRepository, InventoryLotRepository>();

builder.Services.AddScoped<IReplenishmentRepository, ReplenishmentRepository>();
builder.Services.AddScoped<IReplenishmentService, ReplenishmentService>();

builder.Services.AddScoped<IExpiryWatchRepository, ExpiryWatchRepository>();
builder.Services.AddScoped<IExpiryWatchService, ExpiryWatchService>();

builder.Services.AddScoped<IInventoryBalanceRepository, InventoryBalanceRepository>();
builder.Services.AddScoped<IInventoryBalanceService, InventoryBalanceService>();

builder.Services.AddScoped<IInventoryDashboardService, InventoryDashboardService>();

// QCO
builder.Services.AddScoped<IQuarantineRepository, QuarantineRepository>();
builder.Services.AddScoped<IRecallNoticeRepository, RecallNoticeRepository>();
builder.Services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
builder.Services.AddScoped<IQuarantineService, QuarantineService>();
builder.Services.AddScoped<IRecallNoticeService, RecallNoticeService>();
builder.Services.AddScoped<IQCODashboardService, QCODashboardService>();

// Pharmacist
builder.Services.AddScoped<IDispenseRepository, DispenseRepository>();
builder.Services.AddScoped<IDispenseService, DispenseService>();
builder.Services.AddScoped<IPharmacistDashboardService, PharmacistDashboardService>();

builder.Services.AddDbContext<PharmaStock.Models.PharmaStockContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PharmaDbConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Global FluentValidation exception → 400 middleware ─────────────────────
// When a validator calls ValidateAndThrowAsync, it throws ValidationException.
// This middleware catches it and returns a proper 400 ValidationProblemDetails
// so the Angular frontend can display field-level error messages.
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problem = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(errors)
        {
            Status = 400,
            Title = "Validation failed."
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
});

// 3. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();