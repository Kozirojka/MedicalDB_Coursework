using MediatR;
using MedicApp.Application.Patient.Command.CreateVisitRequest;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Api.Endpoints.Patient.CreateHelpRequest;

public class CreateMedicalRequestEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/appointment", Handler);
    }

    private async Task<IResult> Handler(IMediator mediator,HttpContext context, VisitRequestDto dto)
    {
        var command = new CreateVisitRequestCommand(dto, 3);
        var result = await mediator.Send(command);
        
        return Results.Ok(result);
    }
}