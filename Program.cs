using MatchMasterWEB.Database;
using MatchMasterWebAPI.ControllerServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------- Cors -------------------------------------
// Cors 
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

// Register the endpoint services
builder.Services.AddScoped<UpdateDatabaseControllerService>();

// Configure Kestrel to listen on all interfaces
builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ListenAnyIP(5000); // Listen for HTTP connections on port 5000
									 // serverOptions.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Uncomment to listen for HTTPS connections
});

var app = builder.Build();

//Apply cors if in development
if (app.Environment.IsDevelopment())
{
	//Apply cors policy
	Console.WriteLine("CORS policy applied: " + frontendURL);
	app.UseCors(allowFrontendURL);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
