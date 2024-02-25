using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using temperatureVariationAnalysis.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuration
IConfiguration configuration = builder.Configuration;

//MySQL Connection
builder.Services.AddDbContext<MatchMasterMySqlDatabaseContext>
	(options => options.UseMySql(configuration.GetConnectionString("MySQLConnection"),
	ServerVersion.Parse("8.0.25-mysql")));

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
