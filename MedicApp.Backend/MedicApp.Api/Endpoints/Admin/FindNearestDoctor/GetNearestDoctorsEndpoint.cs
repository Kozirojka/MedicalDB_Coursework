using MediatR;
using MedicApp.Application.Admin.Query.GetNearestDoctors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MedicApp.Api.Endpoints.Admin.FindNearestDoctor;

public class GetNearestDoctorsEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/admin/doctors/nearest/{VisitRequestId}", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, IMediator _mediator, int VisitRequestId)
    {

        if (VisitRequestId == null)
        {
            return Results.NotFound();
        }
        
        var query = new GetNearestDoctorsQuery(VisitRequestId);
        var result = await _mediator.Send(query);
        
        if (result == null || result.Count == 0)
        {
            
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }
}   