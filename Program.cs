using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Core.Services;
using PharmaStock.Core.Validators;
using PharmaStock.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FluentValidation;
using PharmaStock.Core.Validators.Auth;
using FluentValidation.AspNetCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Services;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UpsertUserValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
//if not typeof, you would have to specify the type of repository you want to use, but with typeof, you can use any repository you want by just passing the type of it as T.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDrugRepository, DrugRepositoy>();

builder.Services.AddScoped<IDrugService, DrugService>();


builder.Services.AddTransient<IAuditLogService, AuditLogService>();
builder.Services.AddTransient<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddDbContext<PharmaStock.Models.PharmaStockContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PharmaDbConnection"))
);

var app = builder.Build();

// 3. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();