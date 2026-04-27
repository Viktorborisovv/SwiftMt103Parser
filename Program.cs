using SwiftMt103Parser.Api.Data;
using SwiftMt103Parser.Api.Services;
using NLog.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddScoped<SwiftMessageRepository>();
builder.Services.AddScoped<SwiftParserService>();
builder.Services.AddScoped<SwiftMessageService>();

WebApplication app = builder.Build();

DatabaseInitializer databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
databaseInitializer.Initialize();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();