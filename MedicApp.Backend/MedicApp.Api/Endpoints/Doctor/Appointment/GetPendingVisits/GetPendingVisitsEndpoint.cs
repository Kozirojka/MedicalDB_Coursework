using System.Security.Claims;
using MediatR;
using MedicApp.Domain.Dto.Responce;

namespace MedicApp.Api.Endpoints.Doctor.Appointment.GetPendingVisits;

public class GetPendingVisitsEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v2/doctor/visits/pending-visits", Handler);
    }

    private static async Task<IResult> Handler(HttpContext context)
    {
        var mediator = context.RequestServices.GetRequiredService<IMediator>();
        
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var dto = new DoctorRequestFilterDto 
        { 
            Id = userId,
            Status = "AssignedToDoctor"
        };

        var command = dto.MapToCommand();
        var result = await mediator.Send(command);

        return TypedResults.Ok(result);
    }
}