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
        var command = new CreateScheduleDayCommand(1, dateTime);
        var result = await mediator.Send(command);

        if (result is false)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }
}