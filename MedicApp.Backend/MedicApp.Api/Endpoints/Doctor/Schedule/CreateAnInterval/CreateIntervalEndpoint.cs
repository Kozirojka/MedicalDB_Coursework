using System.Security.Claims;
using MediatR;
using MedicApp.Application.Doctor.Interval.Command.CreateTimeInterval;
using MedicApp.Domain.Dto.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.CreateAnInterval
{
    public class CreateIntervalEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/doctor/schedule/interval", Handler);
        }

        private async Task<IResult> Handler(HttpContext context, IMediator mediator, int scheduleId, [FromBody] IntervalDto intervals)
        {
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }
            
            
            var command = new CreateTimeIntervalCommand(scheduleId, userId, intervals);
            var result = await mediator.Send(command);

            if (result is false)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
    }
}