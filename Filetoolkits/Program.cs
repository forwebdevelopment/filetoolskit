using Filetoolkits.application;
using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.infrastructure.Persistance;
using Filetoolkits.infrastructure.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Syncfusion.Licensing;
using Filetoolkits.RequestServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddMediatR(m =>
{
    m.RegisterServicesFromAssembly(typeof(ApplicationClass).Assembly);
});

builder.Services.AddServices();
builder.Services.AddScoped<ILockUnlockService, LockUnlockService>();
builder.Services.AddScoped<ILockFiles, LockFile>();
builder.Services.AddScoped<IUnlockFiles, UnlockFile>();
builder.Services.AddScoped<IFileConversion, FileConversion>();
builder.Services.AddScoped<IfileConversionService, fileConversionService>();
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5fcHRURmFeVkBwXERWYU4=");
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
//if (app.Environment.IsDevelopment())
//{

//}
// Configure the HTTP request pipeline.

app.UseCors("policy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
