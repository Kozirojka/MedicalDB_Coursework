using System.Reflection;
using MedicalVisits.Models.Configurations;
using MedicApp.Api.Extension;
using MedicApp.Application.LogReg.Command;
using MedicApp.Application.LogReg.Command.CreatePatient;
using MedicApp.Domain.Configurations;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.GoogleMapsApi;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CourseWork2Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSwaggerWithJwtSupport();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddHttpClient();
        
builder.Services.Configure<GoogleMapsServiceSettings>(
    builder.Configuration.GetSection("GoogleMaps"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                .AllowAnyMethod() 
                .AllowAnyHeader() 
                .AllowCredentials(); 
        });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));  
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GenerateAccessTokenCommand).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreatePatientCommand).Assembly));


builder.Services.AddOpenApi();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JWT"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowLocalhost3000");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.RegisterAllEndpoints();

app.Run();

