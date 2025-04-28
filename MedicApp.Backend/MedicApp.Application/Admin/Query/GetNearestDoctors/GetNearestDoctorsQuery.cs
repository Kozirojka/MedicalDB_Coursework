using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Admin.Query.GetNearestDoctors;

public class GetNearestDoctorsQuery(int visitId) : IRequest<List<DoctorProfileWithDistance>>
{
    
    //here parameter is id of VisitRequest

    public int requestId { get; set; } = visitId;
}
