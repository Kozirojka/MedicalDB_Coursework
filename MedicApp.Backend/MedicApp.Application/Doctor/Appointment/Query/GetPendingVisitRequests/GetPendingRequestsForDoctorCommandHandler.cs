using MediatR;
using MedicApp.Domain.Dto.Responce;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Extension;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Doctor.Appointment.Query.GetPendingVisitRequests;

public class GetPendingRequestsForDoctorCommandHandler(CourseWork2Context dbContext)
    : IRequestHandler<GetPendingRequestsForDoctorCommand, List<VisitRequestResponce>>
{
    public async Task<List<VisitRequestResponce>> Handle(GetPendingRequestsForDoctorCommand request, CancellationToken cancellationToken)
    {
        // var result = await _dbContext.MedicalHelpRequests
        //     .Where(u => request.Doctor.Id.Equals(u.DoctorId)) 
        //     .Where(u => u.Status == request.Doctor.Status)   
        //     .ToListAsync(cancellationToken);

        var doctor = await dbContext.Doctors.SingleOrDefaultAsync(u => u.AccountId == request.Doctor.Id, cancellationToken: cancellationToken);
        var result = await dbContext.MedicalHelpRequests
            .Where(u => u.DoctorId == doctor.Id)
            .Where(u => u.Status.Name == request.Doctor.Status)
            .Include(medicalHelpRequest => medicalHelpRequest.Status)
            .Include(patient => patient.Patient)
            .ThenInclude(addres => addres.Account)
            .ThenInclude(add => add.Addresses)
            .ToListAsync(cancellationToken);

        var visitRequestDtos = new List<VisitRequestResponce>();

        foreach (var vr in result)
        {
           

            var newWay = vr.Patient.Account?.Addresses.FirstOrDefault();


            if (newWay != null)
                visitRequestDtos.Add(new VisitRequestResponce
                {
                    Id = vr.Id,
                    PatientId = vr.PatientId,
                    Description = vr.Description,
                    Address = newWay.ToDto(),
                    Status = vr.Status.Name
                });
        }

        return visitRequestDtos;
    }


  
}


