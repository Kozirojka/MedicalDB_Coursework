using MediatR;
using MedicApp.Domain.Dto;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Application.LogReg.Command.CreatePatient;

public class CreatePatientCommand : IRequest<AuthResult>
{
    public CreatePatientCommand(RegisterPatientDto driverRequest)
    {
        DriverRequest = driverRequest;
    }

    public RegisterPatientDto DriverRequest { get; set; }
    
}

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string Error { get; set; }
    public AuthResponseDto Response { get; set; }
}

public class RegisterPatientDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Address Address { get; set; }
}
