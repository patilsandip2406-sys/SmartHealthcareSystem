using Npgsql;
using RagService.API.Services;
using RagService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Http clients
builder.Services.AddHttpClient("openrouter", client =>
{
    // base URL handled in service methods
});

builder.Services.AddHttpClient("patient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PatientService:BaseUrl"] ?? "https://localhost:5002");
});

// DB & DI
builder.Services.AddScoped<IEmbeddingClient, OpenRouterEmbeddingClient>();
builder.Services.AddScoped<IDocumentStore, PostgresDocumentStore>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("postgresql");
    if (string.IsNullOrWhiteSpace(connString))
        throw new InvalidOperationException("Postgres connection string is not configured.");

    return new PostgresDocumentStore(connString);
});
builder.Services.AddScoped<IRetrievalService, RetrievalService>();
builder.Services.AddScoped<IRagService, RagServices>();

builder.Services.AddMemoryCache(); // optional caching

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
