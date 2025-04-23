using MediatR;
using MedicApp.Application.LogReg.Command.CreatePatient;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Api.Endpoints.CreatePatientEndpoint;



public class RegisterStudentEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register/patient", Handler).AllowAnonymous();
    }

    private async Task<IResult> Handler(IMediator _Mediator, CreatePatientRequest request)
    {
        var command = new CreatePatientCommand(request);
        var result = await _Mediator.Send(command);

        if (result == null)
            return Results.BadRequest("I fuck your life its null");

        return Results.Ok(result);
    }
}