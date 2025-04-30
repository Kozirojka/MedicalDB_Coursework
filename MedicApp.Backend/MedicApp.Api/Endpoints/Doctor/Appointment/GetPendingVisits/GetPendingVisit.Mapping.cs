using MedicApp.Application.Doctor.Appointment.Query.GetPendingVisitRequests;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Api.Endpoints.Doctor.Appointment.GetPendingVisits;

public static class MyApiRequestExtensions
{
    public static GetPendingRequestsForDoctorCommand MapToCommand(this DoctorRequestFilterDto request)
    {
        
        return new GetPendingRequestsForDoctorCommand(request);
    }
}