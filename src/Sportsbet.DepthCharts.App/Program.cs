using Sportsbet.DepthCharts.App.Endpoints;
using Sportsbet.DepthCharts.Domain.Configuration;
using Sportsbet.DepthCharts.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<NFLConfiguration>();
builder.Services.AddSingleton<MLBConfiguration>();

builder.Services.AddKeyedSingleton<IDepthChartService, DepthChartService>(
    Sports.NFL,
    (sp, key) => new DepthChartService(sp.GetRequiredService<NFLConfiguration>())
);

builder.Services.AddKeyedSingleton<IDepthChartService, DepthChartService>(
    Sports.MLB,
    (sp, key) => new DepthChartService(sp.GetRequiredService<MLBConfiguration>())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDepthChartEndpoints();

app.Run();
