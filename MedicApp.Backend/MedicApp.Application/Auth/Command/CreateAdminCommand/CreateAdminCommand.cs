using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Auth.Command.CreateAdminCommand;

public class CreateAdminCommand(CreatePatientRequest driverRequest) : IRequest<AuthResult>
{
    public CreatePatientRequest DriverRequest { get; set; } = driverRequest;
}



