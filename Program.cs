using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Core.Services;
using PharmaStock.Core.Validators;
using PharmaStock.Core.Validators.Auth;
using PharmaStock.Core.Validators.Location;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Infrastructure.Services;
using PharmaStock.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers & Validation ──────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpsertUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateLocationValidator>();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtKey    = builder.Configuration["Jwt:Key"]    ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtIssuer,
        ValidAudience            = jwtIssuer,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType            = ClaimTypes.Role
    };
});

builder.Services.AddHttpContextAccessor();

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = SecuritySchemeType.Http,
        Scheme      = "Bearer",
        BearerFormat = "JWT",
        In          = ParameterLocation.Header,
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
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<PharmaStockContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PharmaDbConnection")));

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Generic Repository ────────────────────────────────────────────────────────
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ── Auth ──────────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService,    AuthService>();

// ── Users ─────────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,    UserService>();

// ── Drugs ─────────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IDrugRepository, DrugRepository>();
builder.Services.AddScoped<IDrugService,    DrugService>();

// ── Items / Purchase Items ────────────────────────────────────────────────────
builder.Services.AddScoped<IItemRepository,          ItemRepository>();
builder.Services.AddScoped<IItemService,             ItemService>();
builder.Services.AddScoped<IPurchaseItemRepository,  PurchaseItemRepository>();
builder.Services.AddScoped<IPurchaseItemService,     PurchaseItemService>();

// ── Purchase Orders ───────────────────────────────────────────────────────────
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderService,    PurchaseOrderService>();

// ── Vendors ───────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorService,    VendorService>();

// ── Locations ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService,    LocationService>();

// ── Bins ──────────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IBinRepository, BinRepository>();
builder.Services.AddScoped<IBinService,    BinService>();

// ── GRN / Goods Receipt ───────────────────────────────────────────────────────
builder.Services.AddScoped<IGrnRepository,     GrnRepository>();
builder.Services.AddScoped<IGrnService,        GrnService>();
builder.Services.AddScoped<IGRNItemRepository, GRNItemRepository>();
builder.Services.AddScoped<IGRNItemService,    GRNItemService>();

// ── Inventory ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IInventoryLotRepository,     InventoryLotRepository>();
builder.Services.AddScoped<IInventoryLotService,        InventoryLotService>();
builder.Services.AddScoped<IInventoryBalanceRepository, InventoryBalanceRepository>();
builder.Services.AddScoped<IInventoryBalanceService,    InventoryBalanceService>();
builder.Services.AddScoped<IInventoryDashboardService,  InventoryDashboardService>();

// ── Transfer Orders ───────────────────────────────────────────────────────────
builder.Services.AddScoped<ITransferOrderRepository, TransferOrderRepository>();
builder.Services.AddScoped<ITransferOrderService,    TransferOrderService>();

// ── Replenishment ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IReplenishmentRepository, ReplenishmentRepository>();
builder.Services.AddScoped<IReplenishmentService,    ReplenishmentService>();

// ── Expiry Watch ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IExpiryWatchRepository, ExpiryWatchRepository>();
builder.Services.AddScoped<IExpiryWatchService,    ExpiryWatchService>();

// ── Audit ─────────────────────────────────────────────────────────────────────
builder.Services.AddTransient<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddTransient<IAuditLogService,    AuditLogService>();

// ── QCO (Quality & Compliance Officer) ───────────────────────────────────────
builder.Services.AddScoped<IQuarantineRepository,      QuarantineRepository>();
builder.Services.AddScoped<IQuarantineService,         QuarantineService>();
builder.Services.AddScoped<IRecallNoticeRepository,    RecallNoticeRepository>();
builder.Services.AddScoped<IRecallNoticeService,       RecallNoticeService>();
builder.Services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
builder.Services.AddScoped<IQCODashboardService,       QCODashboardService>();

// ── Pharmacist ────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IDispenseRepository,        DispenseRepository>();
builder.Services.AddScoped<IDispenseService,           DispenseService>();
builder.Services.AddScoped<IPharmacistDashboardService, PharmacistDashboardService>();

// ── Build & Pipeline ──────────────────────────────────────────────────────────
var app = builder.Build();

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
