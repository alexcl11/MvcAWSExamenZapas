using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using MvcAWSExamenZapas.Data;
using MvcAWSExamenZapas.Helpers;
using MvcAWSExamenZapas.Repositories;
using MvcAWSExamenZapas.Services;
using MvcAWSExamenZapas.Models; // Namespace donde guardes KeysModel
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. RECUPERAR SECRETO Y DESERIALIZAR CON NEWTONSOFT (Tu estilo)
// ============================================================
string miSecret = await HelperSecretManager.GetSecretAsync();

// Mapeamos el string JSON completo con nuestro modelo KeysModel
KeysModel model = JsonConvert.DeserializeObject<KeysModel>(miSecret);

// Configuramos la base de datos usando la propiedad mapeada del modelo
builder.Services.AddDbContext<ZapasContext>(options =>
    options.UseMySQL(model.MySqlZapas));

// INYECTAMOS EL MODELO COMPLETO COMO SINGLETON (Igual que en tu ejemplo)
// Esto te permitirá recibir "KeysModel" en cualquier controlador o servicio si lo necesitas
builder.Services.AddSingleton<KeysModel>(x => model);

// ============================================================
// 2. REGISTRO DEL RESTO DE SERVICIOS
// ============================================================
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStorageS3>();
builder.Services.AddTransient<RepositoryZapas>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ============================================================
// 3. PIPELINE DE PETICIONES HTTP (MIDDLEWARES)
// ============================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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