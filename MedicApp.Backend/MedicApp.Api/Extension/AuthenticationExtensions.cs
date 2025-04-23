using MedicApp.Domain.Configurations;

namespace MedicApp.Api.Extension;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
                    }
                };
            });
        
        
        Console.WriteLine(jwtSettings.Issuer);
        Console.WriteLine(jwtSettings.Audience);
        Console.WriteLine(jwtSettings.SecretKey);
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("patient", policy =>
                policy.RequireRole("Patient"));

            options.AddPolicy("doctor", policy =>
                policy.RequireRole("Doctor"));
        });

        return services;
    }
}
