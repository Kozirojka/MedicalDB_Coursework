using MediatR;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Application.Patient.Command.CreateVisitRequest;

public record CreateVisitRequestCommand : IRequest<CreateVisitRequestResponse>
{
    public string Description { get; init; }
    public int PatientId { get; init; }

    public CreateVisitRequestCommand(VisitRequestDto dto, int patientId)
    {
        Description = dto.description;
        PatientId = patientId;
    }
}


public record CreateVisitRequestResponse
{
    public int? RequestId { get; init; }
    public string Message { get; init; }
}

