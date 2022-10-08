using Azure.Identity;
using Definitiv.MicrosoftGraph.Web.Configurations;
using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System.Reflection;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

void InitialiseDatabase(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();

    using var dbContext = scope.ServiceProvider.GetRequiredService<LeaveApplicationDbContext>()!;

    dbContext.Database.Migrate();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MicrosoftGraphConfiguration>(builder.Configuration.GetSection("MicrosoftGraph"));

// Add services to the container.
builder.Services.AddSqlite<LeaveApplicationDbContext>(
    builder.Configuration.GetConnectionString("LeaveApplicationDatabase"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton(src =>
{
    var microsoftGraphConfigurationOption = src.GetRequiredService<IOptions<MicrosoftGraphConfiguration>>();

    var microsoftGraphConfiguration = microsoftGraphConfigurationOption.Value ?? throw new Exception("!!!");

    var scopes = new string[] { $"{microsoftGraphConfiguration.ApiUrl}.default" };

    var clientSecretCredential = new ClientSecretCredential(
        microsoftGraphConfiguration.Tenant,
        microsoftGraphConfiguration.ClientId,
        microsoftGraphConfiguration.ClientSecret,
        new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        });

    return new GraphServiceClient(
        clientSecretCredential,
        scopes);
});

builder.Services.AddSingleton<MicrosoftGraphService>();

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

app.MapFallbackToFile("index.html");

app.UseCors(configurePolicy => configurePolicy
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

InitialiseDatabase(app);

app.Run();
