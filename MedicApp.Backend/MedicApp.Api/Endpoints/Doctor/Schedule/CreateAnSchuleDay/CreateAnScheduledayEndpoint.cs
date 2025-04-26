using MediatR;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.CreateAnSchuleDay;

public class CreateAnScheduledayEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/doctor/schedule", Handler);
    }

    private async Task<IResult> Handler(IMediator mediator)
    {
        

        return Results.Ok();
    }
}