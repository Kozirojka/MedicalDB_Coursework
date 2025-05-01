using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Doctor.Appointment.Command.AssignDoctorToVisit;

public class AssignDoctorToVisitCommand(int visitId, int doctorId, int slotTimeId) : IRequest<AssignmentResult>
{
    public int VisitId { get; } = visitId;
    public int DoctorId { get; } = doctorId;
    public int SlotTimeId { get; set;  } = slotTimeId;
}


