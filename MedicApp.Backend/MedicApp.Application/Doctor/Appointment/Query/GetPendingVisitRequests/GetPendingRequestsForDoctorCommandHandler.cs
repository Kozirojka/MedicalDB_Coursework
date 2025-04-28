using MediatR;
using MedicApp.Application.Doctor.GetPendingVisitRequests;
using MedicApp.Domain.Dto.Responce;
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

        var result = await dbContext.MedicalHelpRequests
            .Where(u => u.DoctorId == u.DoctorId)
            .Where(u => u.Status.Name == request.Doctor.Status)
            .Include(medicalHelpRequest => medicalHelpRequest.Status)
            .ToListAsync(cancellationToken);

        var visitRequestDtos = new List<VisitRequestResponce>();

        foreach (var vr in result)
        {
            var patientProfile = await dbContext.Patients
                .Include(p => p.Account) 
                .ThenInclude(u => u.Addresses) 
                .FirstOrDefaultAsync(p => p.Account.Id == vr.PatientId, cancellationToken);
 
            var patientAddress = patientProfile?.Account?.Addresses;

            visitRequestDtos.Add(new VisitRequestResponce
            {
                Id = vr.Id,
                PatientId = vr.PatientId,
                Description = vr.Description,
                Address = patientAddress.FirstOrDefault().ToDto(),
                Status = vr.Status.Name
            });
        }

        return visitRequestDtos;
    }


  
}


