using MedicApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Api.Endpoints.Admin.ManipulationWithUsers.GetUsers;

public class GetUser : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users", Handler);
    }

    private async Task<IResult> Handler(
        CourseWork2Context context, 
        HttpContext httpContext,
        string type = null,
        string fullname = null)
    {
        var doctorsQuery = context.Doctors
            .Include(d => d.Account)
            .Include(d => d.Specializations)
            .AsQueryable();
            
        var patientsQuery = context.Patients
            .Include(p => p.Account)
            .AsQueryable();
            
        if (!string.IsNullOrEmpty(fullname))
        {
            var normalizedSearch = fullname.ToLower().Trim();
            
            doctorsQuery = doctorsQuery.Where(d => 
                (d.Account.Firstname + " " + d.Account.Lastname).ToLower().Contains(normalizedSearch) || 
                (d.Account.Lastname + " " + d.Account.Firstname).ToLower().Contains(normalizedSearch));
                
            patientsQuery = patientsQuery.Where(p => 
                (p.Account.Firstname + " " + p.Account.Lastname).ToLower().Contains(normalizedSearch) || 
                (p.Account.Lastname + " " + p.Account.Firstname).ToLower().Contains(normalizedSearch));
        }
        
        if (string.IsNullOrEmpty(type) || type.ToLower() == "all")
        {
            var doctors = await doctorsQuery.ToListAsync();
            var patients = await patientsQuery.ToListAsync();
            
            var results = new
            {
                Doctors = doctors.Select(d => new
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    FullName = $"{d.Account.Firstname} {d.Account.Lastname}",
                    PhoneNumber = d.Account.Phonenumber,
                    Email = d.Account.Email,
                    Specializations = d.Specializations.Select(s => s.Name).ToList(),
                    Type = "Doctor"
                }),
                
                Patients = patients.Select(p => new
                {
                    Id = p.Id,
                    AccountId = p.AccountId,
                    FullName = $"{p.Account.Firstname} {p.Account.Lastname}",
                    PhoneNumber = p.Account.Phonenumber,
                    Email = p.Account.Email,
                    Type = "Patient"
                })
            };
            
            return Results.Ok(results);
        }
        else if (type.ToLower() == "doctor")
        {
            var doctors = await doctorsQuery.ToListAsync();
            
            var results = doctors.Select(d => new
            {
                Id = d.Id,
                AccountId = d.AccountId,
                FullName = $"{d.Account.Firstname} {d.Account.Lastname}",
                PhoneNumber = d.Account.Phonenumber,
                Email = d.Account.Email,
                Specializations = d.Specializations.Select(s => s.Name).ToList(),
                Type = "Doctor"
            });
            
            return Results.Ok(results);
        }
        else if (type.ToLower() == "patient")
        {
            var patients = await patientsQuery.ToListAsync();
            
            var results = patients.Select(p => new
            {
                Id = p.Id,
                AccountId = p.AccountId,
                FullName = $"{p.Account.Firstname} {p.Account.Lastname}",
                PhoneNumber = p.Account.Phonenumber,
                Email = p.Account.Email,
                Type = "Patient"
            });
            
            return Results.Ok(results);
        }
        else
        {
            return Results.BadRequest($"Invalid user type: {type}. Use 'doctor', 'patient', or 'all'");
        }
    }
}