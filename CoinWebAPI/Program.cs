using System.Globalization;
using CoinWebAPI.Models;
using CoinWebAPI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//SQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CurrencyService>();

var app = builder.Build();

//Swagger-UI
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();

//Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    SupportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("zh-TW") },
    DefaultRequestCulture = new RequestCulture("en-US")
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
