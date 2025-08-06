using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OfflineDemo.Core.Repositories;
using OfflineDemo.Core.Services;
using OfflineDemo.Data;
using OfflineDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Bind config to POCO and register it
builder.Services.Configure<AzureStorageConfig>(
    builder.Configuration.GetSection("AzureStorage"));

// Register the StorageService
builder.Services.AddScoped<IStorageService, StorageService>();

// Register the ShortsService
builder.Services.AddScoped<IShortsService, ShortsService>();

// Register the ShortsRepository
builder.Services.AddScoped<IShortsRepository, ShortsRepository>();

// Configure Kestrel limits
builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 524288000*2; // Example: 500 MB
	options.Limits.MaxRequestBufferSize = 524288000 * 2; // Example: 100 MB
});

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAnyOrigin", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();