﻿using MedicalVisits.Models.Configurations;
using MedicApp.Domain.Entities;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace MedicApp.Infrastructure.Services.GoogleMapsApi;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
     public readonly IOptions<GoogleMapsServiceSettings> _settings;


    public GeocodingService(HttpClient httpClient, IOptions<GoogleMapsServiceSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
    }
    
    public async Task<Coordinate> GeocodeAddressAsync(Address address)
    {
        var addressString = string.Join(", ", address.Street, address.Building, address.City, address.Region, address.Country);
        
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(addressString)}&key={_settings.Value.ApiKey}";

        try
        {
            var response = await _httpClient.GetAsync(url);
        
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            if (data["status"]?.ToString() != "OK")
            {
                var errorMessage = data["status"]?.ToString() ?? "Unknown error";
                throw new Exception($"Geocoding API error: {errorMessage}");
            }

            var location = data["results"]?[0]?["geometry"]?["location"];
            if (location == null)
                throw new Exception("Address not found");

            double latitude = (double)location["lat"];
            double longitude = (double)location["lng"];

            var coordinate = new Coordinate()
            {
                Latitude = latitude,
                Longitude = longitude
            };
            
            return coordinate;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error connecting to Geocoding API: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred: " + ex.Message);
        }
    }
}
