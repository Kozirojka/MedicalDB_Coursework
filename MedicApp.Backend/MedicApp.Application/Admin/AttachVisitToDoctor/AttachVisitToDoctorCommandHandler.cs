using MediatR;
using MedicApp.Infrastructure.Data;
using MedicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicApp.Application.Admin.AttachVisitToDoctor;

public class AttachVisitToDoctorCommandHandler : IRequestHandler<AttachVisitToDoctorCommand, ResultOfAttach>
{
    private readonly IMediator _mediator;
    private readonly CourseWork2Context _context; 

    public AttachVisitToDoctorCommandHandler(IMediator mediator,
        CourseWork2Context context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<ResultOfAttach> Handle(AttachVisitToDoctorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"VisitId: {request.VisitRequestId} ----------------------------");
            Console.WriteLine($"DoctorId: {request.DoctorId} ---------------------------------"); 
            
            int idTemp = request.VisitRequestId;
            
           var visit = await _context.MedicalHelpRequests.FindAsync(idTemp);
           
            
            if (visit == null)
            {
                return new ResultOfAttach
                {
                    Result = false,
                    Message = "VisitRequest not found"
                };
            }

            var status = await _context.HelpRequestStatuses.SingleOrDefaultAsync(u => u.Name == "AssignedToDoctor");
            
            visit.DoctorId = request.DoctorId;
            visit.StatusId = status.Id;
            
            
           
            Console.WriteLine("Saving changes...");

            await _context.SaveChangesAsync(cancellationToken);

            return new ResultOfAttach
            {
                Result = true,
                Message = "Doctor assigned successfully"
            };
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error in Handle method: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            return new ResultOfAttach
            {
                Result = false,
                Message = "An error occurred while assigning the doctor"
            };
        }
    }
}
