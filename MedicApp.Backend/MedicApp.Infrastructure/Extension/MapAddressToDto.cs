using MedicApp.Domain.Dto;
using MedicApp.Infrastructure.Models;

namespace MedicApp.Infrastructure.Extension;

public static class MapAddressToDto
{
    public static AddressDto ToDto(this Address address)
    {
        if (address == null) 
            throw new ArgumentNullException(nameof(address));

        return new AddressDto
        {
            Street = address.Street,
            Building = address.Building,
            Appartaments = address.Appartaments,
            Country = address.Country,
            City = address.City
        };
    }
}