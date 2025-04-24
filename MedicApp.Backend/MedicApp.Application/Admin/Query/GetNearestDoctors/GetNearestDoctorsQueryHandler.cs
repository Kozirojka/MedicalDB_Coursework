using MediatR;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Domain.Entities;
using MedicApp.Infrastructure.Models;
using MedicApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicApp.Application.Admin.Query.GetNearestDoctors;

public class GetNearestDoctorsQueryHandler : IRequestHandler<GetNearestDoctorsQuery, List<DoctorProfileWithDistance>>
{
    private readonly CourseWorkDbContext _dbContext;
    private readonly IGeocodingService _geocodingService;
    private readonly IRouteService _routeService;
    private readonly ILogger<GetNearestDoctorsQueryHandler> _logger;

    public GetNearestDoctorsQueryHandler(
        CourseWorkDbContext dbContext,
        IGeocodingService geocodingService,
        IRouteService routeService,
        ILogger<GetNearestDoctorsQueryHandler> logger)
    {
        _dbContext = dbContext;
        _geocodingService = geocodingService;
        _routeService = routeService;
        _logger = logger;
    }

    public async Task<List<DoctorProfileWithDistance>> Handle(GetNearestDoctorsQuery request, CancellationToken cancellationToken)
    {
        var visitRequest = await _dbContext.MedicalHelpRequests
            .Include(v => v.Patient)
            .ThenInclude(p => p.Account.Addresses)
            .FirstOrDefaultAsync(v => v.Id == request.requestId, cancellationToken);

        
        if (visitRequest == null)
        {
            _logger.LogWarning("Visit request with ID {RequestId} not found.", request.requestId);
            return new List<DoctorProfileWithDistance>();
        }

        var patient = await _dbContext.Patients
            .Include(patient => patient.Account)
            .ThenInclude(account => account.Addresses)
            .SingleOrDefaultAsync(u => u.Id == visitRequest.PatientId, cancellationToken: cancellationToken);
        
        
        
        var patientAddress = patient?.Account.Addresses ?? throw new Exception("Patient address not found.");

        var patientCoordinates = await _geocodingService.GeocodeAddressAsync(patientAddress.First());
        ValidateCoordinates(patientCoordinates, "Patient coordinates not found.");

        var doctors = await _dbContext.Doctors
            .Include(d => d.Account)
            .Where(d => d.Account.Addresses.First().Country == patientAddress.First().Country &&
                        d.Account.Addresses.First().City == patientAddress.First().City)
            .ToListAsync(cancellationToken);

        if (!doctors.Any())
        {
            _logger.LogWarning("No doctors found in the city {City}.", patientAddress.First().City);
            return new List<DoctorProfileWithDistance>();
        }

        var doctorDistances = await CalculateDistancesAsync(doctors, patientCoordinates);
        
        return doctorDistances.OrderBy(d => d.Distance).ToList();
    }

    private async Task<List<DoctorProfileWithDistance>> CalculateDistancesAsync(
        List<Infrastructure.Models.Doctor> doctors,
        Coordinate patientCoordinates)
    {
        var doctorDistances = new List<DoctorProfileWithDistance>();

        foreach (var doctor in doctors)
        {
            var doctorCoordinates = await _geocodingService.GeocodeAddressAsync(doctor.Account.Addresses.First());
            if (!IsValidCoordinates(doctorCoordinates)) continue;

            var distance = await _routeService.GetDistanceBetweenTwoPoints(patientCoordinates, doctorCoordinates);
            if (distance == null) continue;

            doctorDistances.Add(new DoctorProfileWithDistance
            {
                DoctorId = doctor.Account.Id,
                FirstName = doctor.Account.Firstname,
                LastName = doctor.Account.Lastname,
                Specialization = doctor.Specializations.First().ToString(),
                Distance = distance
            });
        }

        return doctorDistances;
    }

    private void ValidateCoordinates(Coordinate coordinates, string errorMessage)
    {
        if (!IsValidCoordinates(coordinates))
        {
            throw new Exception(errorMessage);
        }
    }

    private bool IsValidCoordinates(Coordinate coordinates)
    {
        return coordinates.Latitude != 0 && coordinates.Longitude != 0;
    }
}
