using AllPhi.HoGent.Datalake.Data.Context;
using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddDbContextFactory<AllPhiDatalakeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Development")));

builder.Services.AddDbContext<AllPhiDatalakeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Development")));

builder.Services.AddScoped<IFuelCardStore, FuelCardStore>();
builder.Services.AddScoped<IVehicleStore, VehicleStore>();
builder.Services.AddScoped<IDriverStore, DriverStore>();
builder.Services.AddScoped<IFuelCardDriverStore, FuelCardDriverStore>();
builder.Services.AddScoped<IDriverVehicleStore, DriverVehicleStore>();

builder.Services.AddRateLimiter(l => l.AddFixedWindowLimiter(policyName: "AllPhiFixedLimiter", Options =>
{
    Options.PermitLimit = 30;
    Options.Window = TimeSpan.FromSeconds(10);
    Options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    Options.QueueLimit = 10;
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();