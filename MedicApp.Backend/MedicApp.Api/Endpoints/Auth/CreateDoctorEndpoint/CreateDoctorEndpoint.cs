using MediatR;
using MedicApp.Application.Auth.Command.CreateDoctor;
using MedicApp.Application.LogReg.Command.CreatePatient;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Api.Endpoints.Auth.CreateDoctorEndpoint;

public class CreateDoctorEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register/doctor", Handler).AllowAnonymous();
    }

    private async Task<IResult> Handler(IMediator _Mediator, CreateDoctorRequest request)
    {
        var command = new CreateDoctorCommand(request);
        var result = await _Mediator.Send(command);

        if (result == null)
            return Results.BadRequest("I fuck your life its null");

        return Results.Ok(result);
    }
}