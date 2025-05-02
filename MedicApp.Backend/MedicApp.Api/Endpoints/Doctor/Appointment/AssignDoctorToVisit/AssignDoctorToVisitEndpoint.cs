using System.Security.Claims;
using MediatR;
using MedicApp.Application.Doctor.Appointment.Command.AssignDoctorToVisit;

namespace MedicApp.Api.Endpoints.Doctor.Appointment.AssignDoctorToVisit;


/// <summary>
/// This method will assign doctor to visits
/// One doctor and assign himself to visit
/// </summary>
/// <param name="mediator"></param>
public class AssignDoctorToVisitEndpoint : IEndpoint
{
    

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/doctor/assign/{visitId}/{slotId}", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, IMediator mediator, int visitId, int slotId)
    {
        
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var command = new AssignDoctorToVisitCommand(visitId, userId, slotId);
        
        var result = await mediator.Send(command);

        return Results.Ok(result);
    }
}