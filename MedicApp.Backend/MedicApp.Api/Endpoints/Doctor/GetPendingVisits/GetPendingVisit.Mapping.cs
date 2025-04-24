using MedicApp.Application.Doctor.GetPendingVisitRequests;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Api.Endpoints.Doctor.GetPendingVisits;

public static class MyApiRequestExtensions
{
    public static GetPendingRequestsForDoctorCommand MapToCommand(this DoctorRequestFilterDto request)
    {
        
        return new GetPendingRequestsForDoctorCommand(request);
    }
}