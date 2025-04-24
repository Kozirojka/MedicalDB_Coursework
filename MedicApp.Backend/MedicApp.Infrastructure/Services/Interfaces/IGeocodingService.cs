using MedicApp.Domain.Entities;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Infrastructure.Services.Interfaces;

public interface IGeocodingService
{
    public Task<Coordinate> GeocodeAddressAsync(Address address);
}
