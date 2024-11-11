using Azure.Identity;
using ShippingProvider.Services;

var builder = WebApplication.CreateBuilder(args);

var vaultUri = new Uri($"{builder.Configuration["KeyVault"]!}");
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureKeyVault(
        vaultUri,
        new VisualStudioCredential());
}
else
{
    builder.Configuration.AddAzureKeyVault(
        vaultUri,
        new DefaultAzureCredential());
}

var secretKey = builder.Configuration["PostnordApiKey"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IShippingService>(sp => 
    new ShippingService(sp.GetRequiredService<HttpClient>(), secretKey!, sp.GetRequiredService<IConfiguration>()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();