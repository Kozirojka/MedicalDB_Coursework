using MediatR;
using MedicApp.Application.Admin.Query.FindAppendingRequests;

namespace MedicalVisits.Application.Admin.Queries.FindAppendingRequests;

public class FindAppendingRequestsQuery : IRequest<List<VisitResponceDto>>
{
    
}
