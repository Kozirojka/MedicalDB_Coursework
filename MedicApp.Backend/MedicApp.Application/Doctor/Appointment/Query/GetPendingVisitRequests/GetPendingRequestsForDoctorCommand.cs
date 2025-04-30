using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Doctor.Appointment.Query.GetPendingVisitRequests;

public class GetPendingRequestsForDoctorCommand(DoctorRequestFilterDto doctor) : IRequest<List<VisitRequestResponce>>
{
    public DoctorRequestFilterDto Doctor { get; set; } = doctor;
}



