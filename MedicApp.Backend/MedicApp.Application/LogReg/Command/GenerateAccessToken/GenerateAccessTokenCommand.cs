using MediatR;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Application.LogReg.Command;

public record GenerateAccessTokenCommand : IRequest<string>
{
    public Account User { get; init; }
    public string Role { get; init; }
}
