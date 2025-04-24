using MediatR;

namespace MedicApp.Application.Admin.AttachVisitToDoctor;

public class AttachVisitToDoctorCommand : IRequest<ResultOfAttach>
{
    public int VisitRequestId { get; init; }
    public int DoctorId { get; init; }
}


public class ResultOfAttach
{
    public bool Result { get; set; }
    public string Message { get; set; }
}