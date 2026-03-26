using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Infrastructure.Services;
using PharmaStock.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(PharmaStock.Core.Interfaces.IGenericRepository<>), typeof(PharmaStock.Infrastructure.Repositories.GenericRepository<>)); 
//if not typeof, you would have to specify the type of repository you want to use, but with typeof, you can use any repository you want by just passing the type of it as T.

builder.Services.AddTransient<IAuditLogService, AuditLogService>();
builder.Services.AddTransient<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddDbContext<PharmaStock.Models.PharmaStockContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PharmaDbConnection"))
);

var app = builder.Build();

// 3. Configure the HTTP request pipeline
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();