using OnlineShop.RestApi.Configs.MigrationConfigs;
using OnlineShop.RestApi.Configs.ServiceConfigs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

builder.UpdateDataBases();
builder.UpdateReadableDataBases();

app.Run("http://localhost:5104");