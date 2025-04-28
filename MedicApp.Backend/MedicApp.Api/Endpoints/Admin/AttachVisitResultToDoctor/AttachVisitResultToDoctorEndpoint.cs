using MediatR;
using MedicApp.Application.Admin.AttachVisitToDoctor;

namespace MedicApp.Api.Endpoints.Admin.AttachVisitResultToDoctor;

public sealed record VisitDoctorInfoDto(int DoctorId, int VisitId);

public class AttachVisitResultToDoctorEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/admin/visit-requests/attach", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, IMediator _mediator, VisitDoctorInfoDto req)
    {
        var command = new AttachVisitToDoctorCommand()
        {
            DoctorId = req.DoctorId,
            VisitRequestId = req.VisitId
        };

        ResultOfAttach result = await _mediator.Send(command);


        if (result == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);
    }
}