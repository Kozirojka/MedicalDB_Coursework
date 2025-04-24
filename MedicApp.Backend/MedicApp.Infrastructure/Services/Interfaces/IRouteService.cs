using MedicApp.Domain.Dto.Responce;
using MedicApp.Domain.Entities;

namespace MedicApp.Infrastructure.Services.Interfaces;

public interface IRouteService
{
    public Task<RouteResponse?> GetOptimizedRouteAsync(Coordinate start, List<Coordinate> waypoints);
    public Task<double> GetDistanceBetweenTwoPoints(Coordinate startPoint, Coordinate endPoint);

}