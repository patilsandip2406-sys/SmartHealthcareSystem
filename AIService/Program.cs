using AIService.Services;
using Infrastructure.Library;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React app
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Register AI services
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddSingleton<OpenRouterService>();
builder.Services.AddScoped<IAIService>(sp =>
{
    var useProvider = builder.Configuration["AIProvider"] ?? "OpenAI"; // OpenAI | OpenRouter
    var memoryCache = sp.GetRequiredService<IMemoryCache>();

    IAIService inner = useProvider switch
    {
        "OpenAI" => sp.GetRequiredService<OpenAIService>(),
        "OpenRouter" => sp.GetRequiredService<OpenRouterService>(),
        _ => throw new InvalidOperationException("Unknown AI provider: " + useProvider)
    };

    // wrap with caching decorator
    return new CachingAIService(inner, memoryCache);
});

builder.Services.AddScoped<IPatientInfo, PatientInfoServices>();

builder.Services.AddMemoryCache();

var app = builder.Build();
app.UseCors("AllowReactApp");

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
