using AllPhi.HoGent.Datalake.Data.Context;
using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContextFactory<AllPhiDatalakeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Development")));

builder.Services.AddScoped<IFuelCardStore, FuelCardStore>();
builder.Services.AddScoped<IVehicleStore, VehicleStore>();
builder.Services.AddScoped<IDriverStore, DriverStore>();
builder.Services.AddScoped<IFuelCardDriverStore, FuelCardDriverStore>();
builder.Services.AddScoped<IDriverVehicleStore, DriverVehicleStore>();


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

app.MapControllers();

app.Run();
