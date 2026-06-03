using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using MvcAWSExamenZapas.Data;
using MvcAWSExamenZapas.Helpers;
using MvcAWSExamenZapas.Repositories;
using MvcAWSExamenZapas.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


string secretJson = HelperSecretManager.GetSecretAsync().GetAwaiter().GetResult();

// Parseamos el JSON que devuelve AWS para extraer la clave exacta
using (JsonDocument doc = JsonDocument.Parse(secretJson))
{
    JsonElement root = doc.RootElement;

    string connectionString = root.GetProperty("MySQLZapas").GetString();

    builder.Services.AddDbContext<ZapasContext>(options =>
        options.UseMySQL(connectionString));
}

// Add services to the container.
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStorageS3>();
builder.Services.AddTransient<RepositoryZapas>();
builder.Services.AddDbContext<ZapasContext>(x => x.UseMySQL(builder.Configuration.GetConnectionString("MySQLZapatillas")));


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
