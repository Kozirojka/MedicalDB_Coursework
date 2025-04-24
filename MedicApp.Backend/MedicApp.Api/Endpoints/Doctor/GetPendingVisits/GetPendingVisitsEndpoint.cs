using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Api.Endpoints.Doctor.GetPendingVisits;

public class GetPendingVisitsEndpoint(IMediator mediator) : IEndpoint
{
    
       
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v2/doctor/visits/pending-visits", Handler);
    }

    private async Task<IResult> Handler(HttpContext context)
    {
        
        var dto = new DoctorRequestFilterDto 
        { 
            Id = 4,
            Status = "AssignedToDoctor"
        };

        var listOfPendingRequestsCommand = dto.MapToCommand();
        
        var result = await mediator.Send(listOfPendingRequestsCommand);
        
        return TypedResults.Ok(result);
    }
}