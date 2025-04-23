using MediatR;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.LogReg.Command.CreateDoctor;

public class CreateDoctorCommand : IRequest<AuthResult>
{
    public CreateDoctorCommand(CreatePatientRequest driverRequest)
    {
        DriverRequest = driverRequest;
    }

    public CreatePatientRequest DriverRequest { get; set; }
    
}



