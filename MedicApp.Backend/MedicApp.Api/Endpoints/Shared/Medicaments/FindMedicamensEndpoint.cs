namespace MedicApp.Api.Endpoints.Shared.Medicaments;

public class FindMedicamensEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/appointment/medic", Handler);
    }

    private Task Handler(HttpContext context)
    {
        throw new NotImplementedException();
    }
}