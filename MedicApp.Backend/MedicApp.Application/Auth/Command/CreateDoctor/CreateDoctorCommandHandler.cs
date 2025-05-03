using MediatR;
using MedicApp.Application.LogReg.Command;
using MedicApp.Domain.Dto;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Auth.Command.CreateDoctor;

public class CreateDoctorCommandHandler(
    IMediator mediator,
    CourseWork2Context context,
    IPasswordService passwordService)
    : IRequestHandler<CreateDoctorCommand, AuthResult>
{
    public async Task<AuthResult> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        if (request == null || request.DriverRequest == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Request is null"
            };
        }


        if (string.IsNullOrWhiteSpace(request.DriverRequest.Email) ||
            string.IsNullOrWhiteSpace(request.DriverRequest.Password) ||
            string.IsNullOrWhiteSpace(request.DriverRequest.Firstname) ||
            string.IsNullOrWhiteSpace(request.DriverRequest.Lastname))
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "All fields are required"
            };
        }

        if (await context.Accounts.SingleOrDefaultAsync(a => a.Email == request.DriverRequest.Email, cancellationToken: cancellationToken) != null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Email is already registered"
            };
        }


        var address = new Address
        {
            Street = request.DriverRequest.Address.Street,
            Building = request.DriverRequest.Address.Building,
            Appartaments = request.DriverRequest.Address.Appartaments,
            Country = request.DriverRequest.Address.Country,
            City = request.DriverRequest.Address.City,
        };


        var role = await context.Roles.SingleOrDefaultAsync(u => u.Name == "Doctor",
            cancellationToken: cancellationToken);
        var user = new Account()
        {
            Email = request.DriverRequest.Email,
            Firstname = request.DriverRequest.Firstname,
            Lastname = request.DriverRequest.Lastname,
            Phonenumber = request.DriverRequest.Phonenumber,
            RoleId = role.Id,
            Addresses = new List<Address> { address },
            PasswordHash = passwordService.HashPassword(request.DriverRequest.Password),
        };


        await context.Accounts.AddAsync(user, cancellationToken);

        var result = await context.SaveChangesAsync(cancellationToken) > 0;
        if (!result)
        {
            return new AuthResult
            {
                Succeeded = false,
            };
        }


        string token;
        try
        {
            token = await mediator.Send(new GenerateAccessTokenCommand
            {
                User = user,
                Role = "Doctor"
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = $"Failed to generate access token: {ex.Message}"
            };
        }
        

        var specializations = await context.Specializations
            .Where(x => request.DriverRequest.Specialization.Contains(x.Name))
            .ToListAsync(cancellationToken: cancellationToken);
        
        var education = await context.Educations
            .Where(x => request.DriverRequest.Education.Contains(x.Name))
            .ToListAsync(cancellationToken: cancellationToken);
        
        
        
        var doctorProfile = new Infrastructure.Models.Doctor()
        {
            AccountId = user.Id,
            Specializations = specializations,
            Educations = education
        };


        context.Doctors.Add(doctorProfile);
        await context.SaveChangesAsync(cancellationToken);


        return new AuthResult
        {
            Succeeded = true,
            Response = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
                Role = "Doctor",
                FirstName = user.Firstname,
                LastName = user.Lastname,
            }
        };
    }
}