using System.Reflection;
using MedicApp.Api.Extension;
using MedicApp.Application.LogReg.Command;
using MedicApp.Application.LogReg.Command.CreatePatient;
using MedicApp.Domain.Configurations;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CourseWorkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSwaggerWithJwtSupport();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));  // Це автоматично зареєструє всі обробники в поточній збірці
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


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.RegisterAllEndpoints();

app.Run();

