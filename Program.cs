using MatchMasterWEB.Database;
using MatchMasterWebAPI.ControllerServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------- Cors -------------------------------------
var allowFrontendURL = "_originsForDevelopment";
//Get the frontend development URL from appsettings.json
string? frontendURL = builder.Configuration["DevelopmentFrontendURL"];

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services and define the policy
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: allowFrontendURL,
					  policy =>
					  {
						  policy.WithOrigins(frontendURL)
								.AllowAnyHeader()
								.AllowAnyMethod();
					  });
});

// Configuration
IConfiguration configuration = builder.Configuration;

// MySQL Connection
builder.Services.AddDbContext<MatchMasterMySqlDatabaseContext>(options =>
	options.UseMySql(configuration.GetConnectionString("MySQLConnection"), ServerVersion.Parse("8.0.25-mysql")));

var app = builder.Build();

// Use CORS policy
app.UseCors(allowFrontendURL);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
