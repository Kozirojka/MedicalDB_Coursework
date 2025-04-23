using MediatR;
using MedicApp.Domain.Dto;
using MedicApp.Domain.Dto.Requests;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Application.LogReg.Command.CreatePatient;

public class CreatePatientCommand : IRequest<AuthResult>
{
    public CreatePatientCommand(CreatePatientRequest driverRequest)
    {
        DriverRequest = driverRequest;
    }

    public CreatePatientRequest DriverRequest { get; set; }
    
}



