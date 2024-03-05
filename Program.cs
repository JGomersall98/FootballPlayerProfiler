using MatchMasterWEB.Database;
using MatchMasterWebAPI.ControllerServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------- Cors -------------------------------------
var allowFrontendURL = "_allowSpecificOrigins";
// Get the frontend URL from appsettings.json or use a sensible default
string? frontendURL = builder.Configuration["FrontendURL"] ?? "http://localhost:3000";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
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

// MySQL Connection
builder.Services.AddDbContext<MatchMasterMySqlDatabaseContext>(options =>
	options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))));

// Register services
builder.Services.AddScoped<UpdateDatabaseControllerService>();

var app = builder.Build();

// CORS
app.UseCors(allowFrontendURL);

// Swagger
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
