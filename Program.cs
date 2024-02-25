using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using temperatureVariationAnalysis.Database;

var builder = WebApplication.CreateBuilder(args);

//Configuration
IConfiguration configuration = builder.Configuration;

//MySQL Connection
builder.Services.AddDbContext<MatchMasterMySqlDatabaseContext>
	(options => options.UseMySql(configuration.GetConnectionString("MySQLConnection"),
	ServerVersion.Parse("8.0.25-mysql")));

// Add services to the container.
//builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	//app.UseHsts();
}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

app.Run();
