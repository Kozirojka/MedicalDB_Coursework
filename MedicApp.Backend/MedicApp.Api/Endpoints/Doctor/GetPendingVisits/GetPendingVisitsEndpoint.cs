using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Api.Endpoints.Doctor.GetPendingVisits;

public class GetPendingVisitsEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v2/doctor/visits/pending-visits", Handler);
    }

    private static async Task<IResult> Handler(HttpContext context)
    {
        var mediator = context.RequestServices.GetRequiredService<IMediator>();

        var dto = new DoctorRequestFilterDto 
        { 
            Id = 4,
            Status = "AssignedToDoctor"
        };

        var command = dto.MapToCommand();
        var result = await mediator.Send(command);

        return TypedResults.Ok(result);
    }
}