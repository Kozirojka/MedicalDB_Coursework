using MediatR;
using MedicApp.Application.Auth.Command.CreateAdminCommand;
using MedicApp.Application.LogReg.Command.CreateDoctor;
using MedicApp.Domain.Dto.Requests;

namespace MedicApp.Api.Endpoints.Auth.CreateAdminEndpoint;

public class CreateAdminEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register/doctor", Handler).AllowAnonymous();
    }

    private async Task<IResult> Handler(IMediator _Mediator, CreatePatientRequest request)
    {
        var command = new CreateAdminCommand(request);
        var result = await _Mediator.Send(command);

        if (result == null)
            return Results.BadRequest("I fuck your life its null");

        return Results.Ok(result);
    }
}