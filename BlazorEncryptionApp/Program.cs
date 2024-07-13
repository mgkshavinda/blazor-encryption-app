using BlazorEncryptionApp.Components;
using BlazorEncryptionApp.Data;
using BlazorEncryptionApp.Domain.Interface;
using BlazorEncryptionApp.Domain.Service;
using BlazorEncryptionApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Retrieve the encryption key and key size from configuration
string encryptionKeyString = builder.Configuration["EncryptionKeySettings:EncryptionKey"];
string keySizeString = builder.Configuration["EncryptionKeySettings:KeySize"];


if (!int.TryParse(keySizeString, out int keySize) || (keySize != 128 && keySize != 192 && keySize != 256))
{
    throw new InvalidOperationException("Key size must be one of the following values: 128, 192, or 256.");
}

byte[] encryptionKeyBytes = Encoding.UTF8.GetBytes(encryptionKeyString);

// Ensure the byte array is of the correct length
int requiredKeyLength = keySize / 8;
byte[] encryptionKey = new byte[requiredKeyLength];

for (int i = 0; i < requiredKeyLength; i++)
{
    encryptionKey[i] = (i < encryptionKeyBytes.Length) ? encryptionKeyBytes[i] : (byte)0;
}

// Add the EncryptionService with the encryption key
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(encryptionKey));

builder.Services.AddScoped<Repository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
