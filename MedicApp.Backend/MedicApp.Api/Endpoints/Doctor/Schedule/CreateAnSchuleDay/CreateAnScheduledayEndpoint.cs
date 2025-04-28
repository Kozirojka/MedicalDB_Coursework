using System.Security.Claims;
using MediatR;
using MedicApp.Application.Doctor.Interval.Command.CreateScheduleDay;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.CreateAnSchuleDay;

public class CreateAnScheduledayEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/doctor/schedule", Handler);
    }

    private async Task<IResult> Handler(HttpContext context,IMediator mediator, DateOnly dateTime)
    {
        
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        var command = new CreateScheduleDayCommand(userId, dateTime);
        var result = await mediator.Send(command);

        
        if (result is false)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }
}