using System.Security.Claims;
using MediatR;
using MedicApp.Application.Patient.Command.CreateVisitRequest;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Api.Endpoints.Patient.CreateHelpRequest;

public class CreateMedicalRequestEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/appointment", Handler);
    }

    private async Task<IResult> Handler(IMediator mediator,HttpContext context, VisitRequestDto dto)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }
        
        var command = new CreateVisitRequestCommand(dto, userId);
        var result = await mediator.Send(command);
        
        return Results.Ok(result);
    }
}    