using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Application.Admin.Query.GetNearestDoctors;

public class GetNearestDoctorsQuery : IRequest<List<DoctorProfileWithDistance>>
{
    
    //here parameter is id of VisitRequest
    public GetNearestDoctorsQuery(int visitId)
    {
        requestId = visitId;
    }
    
    public int requestId { get; set; }
    
}
