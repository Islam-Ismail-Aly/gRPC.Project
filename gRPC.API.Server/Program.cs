
using gRPC.API.Entites.Interfaces;
using gRPC.API.Server.Handler;
using gRPC.API.Server.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddGrpc();

builder.Services.AddScoped<IApiKeyAuthenticationService, ApiKeyAuthenticationService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ApiKey";
}).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", configureOptions => { });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProductService>();

app.Run();

