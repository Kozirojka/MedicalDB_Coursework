using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Doctor.GetPendingVisitRequests;

public class GetPendingRequestsForDoctorCommand : IRequest<List<VisitRequestResponce>>
{
    public GetPendingRequestsForDoctorCommand(DoctorRequestFilterDto doctor)
    {
        Doctor = doctor;
    }
    
    public DoctorRequestFilterDto Doctor { get; set; }
    
}



