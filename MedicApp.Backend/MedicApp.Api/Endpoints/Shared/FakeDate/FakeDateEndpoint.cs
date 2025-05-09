namespace MedicApp.Api.Endpoints.Shared.FakeDate;

public class FakeDateEndpoint : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/date/generation", Handler);
    }

    private async Task<IResult> Handler(HttpContext context, MedicApp.FakeDate.FakeDate dataServices)
    {

        var result = dataServices.GenerateTestData();
        
        return Results.Ok(result);
    }
}