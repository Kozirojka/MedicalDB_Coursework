using System.Security.Claims;
using MediatR;
using MedicApp.Application.Doctor.AssignDoctorToVisit;

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
        endpoints.MapPut("/api/doctor/assign/{visitId}/{scheduleId}", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, IMediator mediator, int visitId, int scheduleId)
    {
        
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var command = new AssignDoctorToVisitCommand(visitId, userId, scheduleId);
        
        var result = await mediator.Send(command);

        return Results.Ok(result);
    }
}