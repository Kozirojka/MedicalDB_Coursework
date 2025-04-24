using MediatR;
using MedicalVisits.Application.Admin.Queries.FindAppendingRequests;
using MedicApp.Application.Admin.Query.FindAppendingRequests;

namespace MedicApp.Api.Endpoints.Admin.PendingVisitRequest;

public sealed record ListOfUsers(List<VisitResponceDto> Visit);

public class GetPendingVisitRequests() : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v2/admin/visit-requests", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, IMediator mediator)
    {
        var command = new FindAppendingRequestsQuery();
        var result = await mediator.Send(command);

        if (result == null || result.Count == 0)
        {
            return Results.NotFound();
        }

        return Results.Ok(new ListOfUsers(result));
    }
}