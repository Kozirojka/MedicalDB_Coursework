using MediatR;
using MedicApp.Domain.Dto;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.LogReg.Command.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, AuthResult>
{
    private readonly IMediator _mediator;
    private readonly CourseWorkDbContext _context;

    public CreatePatientCommandHandler(IMediator mediator,
        CourseWorkDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }


    public async Task<AuthResult> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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

        if (await _context.Accounts.SingleOrDefaultAsync(a => a.Email == request.DriverRequest.Email) != null)
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


        var role = await _context.Roles.SingleOrDefaultAsync(u => u.Name == "Patient");
        var user = new Account()
        {
            Email = request.DriverRequest.Email,
            Firstname = request.DriverRequest.Firstname,
            Lastname = request.DriverRequest.Lastname,
            Phonenumber = request.DriverRequest.Phonenumber,
            RoleId = role.Id,
            Addresses = new List<Address> { address }
        };
        
        
        
        await _context.Accounts.AddAsync(user, cancellationToken);
        
        var result = await _context.SaveChangesAsync(cancellationToken) > 0; 
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
            token = await _mediator.Send(new GenerateAccessTokenCommand
            {
                User = user,
                Role = "Patient"
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

        var patientProfile = new Patient()
        {
            AccountId = user.Id,
        };


        
            _context.Patients.Add(patientProfile);
            await _context.SaveChangesAsync(cancellationToken);
        


        return new AuthResult
        {
            Succeeded = true,
            Response = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
                Role = "Patient",
                FirstName = user.Firstname,
                LastName = user.Lastname,
            }
        };
    }
}
