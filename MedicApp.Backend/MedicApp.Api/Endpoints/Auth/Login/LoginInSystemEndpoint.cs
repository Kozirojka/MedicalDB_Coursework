using MediatR;
using MedicApp.Application.LogReg.Command;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Auth.Login;

public class LoginInSystemEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/auth/login", Handler);
    }

    private async Task<IResult> Handler(CourseWork2Context dbcontext, IPasswordService passwordService, IMediator mediator, HttpContext context, LoginRequest loginRequest)
    {
        var user = await dbcontext.Accounts
            .Include(u => u.Role) 
            .SingleOrDefaultAsync(u => u.Email == loginRequest.Email);
        
        
        if (user == null) return Results.Unauthorized();
        
        var isPasswordValid = passwordService.VerifyPassword(user.PasswordHash, loginRequest.Password);
        
        
        if (!isPasswordValid)
        {
            return Results.Unauthorized();
        }

        Console.WriteLine(user.Role.Name);
        var accessToken = await mediator.Send(new GenerateAccessTokenCommand 
        { 
            User = user, 
            Role = user.Role.Name,
        });
        
        return Results.Ok(new AuthResponse
        {
            AccessToken = accessToken,
        });
        
    }
}