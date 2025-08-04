using Azure.Identity;
using Kierkels.Knaken.Infrastructure;
using Kierkels.Knaken.Infrastructure.Persistence;
using Kierkels.Knaken.Web.Components;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add Key Vault configuration in non-development environments
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUrl = builder.Configuration["AzureKeyVault:Url"];
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl!),
        new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, _) => { module.EnableSqlCommandTextInstrumentation = true; });

var app = builder.Build();

// Ensure the database is seeded
await SetupDatabase(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
return;

static async Task SetupDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        // Apply pending migrations (if any)
        await context.Database.MigrateAsync();

        // Seed transactions
        // var csvFilePath = Path.Combine(AppContext.BaseDirectory, "../../../../Data", "NL96INGB0681922745_01-01-2024_31-12-2024.csv");
        // await TransactionSeeder.SeedTransactionsAsync(context, csvFilePath);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}
