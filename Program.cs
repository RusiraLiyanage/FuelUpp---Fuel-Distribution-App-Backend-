using Fuel_App.Models.Shed_Owner_Model;
using Fuel_App.Models.User;
using Fuel_App.Services.Shed_Owner_Service;
using Fuel_App.Services.User_Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ------------------ User Database Service Settings ---------------------------------------------------
builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection(nameof(UserDatabaseSettings)));

builder.Services.AddSingleton<IUserDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("UserDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IUserService, UserService>();
// -----------------------------------------------------------------------------------------------------

// -------------------- Shed Owner Service Settings ----------------------------------------------------
builder.Services.Configure<ShedOwnerDatabaseSettings>(
    builder.Configuration.GetSection(nameof(ShedOwnerDatabaseSettings)));

builder.Services.AddSingleton<IShedOwnerDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ShedOwnerDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("ShedOwnerDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IShedOwnerService, ShedOwnerService>();
// -----------------------------------------------------------------------------------------------------

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
