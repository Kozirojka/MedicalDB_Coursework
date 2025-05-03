using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Auth.Command.CreateDoctor;

public class CreateDoctorCommand(CreateDoctorRequest driverRequest) : IRequest<AuthResult>
{
    public CreateDoctorRequest DriverRequest { get; set; } = driverRequest;
}



