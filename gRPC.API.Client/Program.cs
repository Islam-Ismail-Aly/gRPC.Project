using gRPC.API.Client.Services;
using gRPC.API.Entites.Interfaces;
using gRPC.API.Entites.Services;
using gRPC.API.Server.Protos;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure gRPC client
builder.Services.AddGrpcClient<InventoryService.InventoryServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7031");
}).AddCallCredentials((context, metadata, serviceProvider) =>
{
    var apiKeyProvider = serviceProvider.GetRequiredService<IApiKeyProviderService>();
    var apiKey = apiKeyProvider.GetApiKey();
    metadata.Add("X-Api-Key", apiKey);
    return Task.CompletedTask;
}); ;

// Add the GrpcInventoryClient as a service
builder.Services.AddScoped<GrpcInventoryClientService>();

builder.Services.AddScoped<IApiKeyProviderService, ApiKeyProviderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
