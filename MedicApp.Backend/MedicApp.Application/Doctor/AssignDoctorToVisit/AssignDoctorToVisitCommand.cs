using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Doctor.AssignDoctorToVisit;

public class AssignDoctorToVisitCommand : IRequest<AssignmentResult> 
{
    public int VisitId { get; }
    public int DoctorId { get; }
    public int SlotTimeId { get; set;  }
    public AssignDoctorToVisitCommand(int visitId, int doctorId, int slotTimeId)
    {
        VisitId = visitId;
        DoctorId = doctorId;
        SlotTimeId = slotTimeId;
    }
}


