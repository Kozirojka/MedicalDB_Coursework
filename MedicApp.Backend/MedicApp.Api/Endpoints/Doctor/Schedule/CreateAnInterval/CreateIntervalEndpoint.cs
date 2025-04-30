using System.Security.Claims;
using MediatR;
using MedicApp.Application.Doctor.Interval.Command.CreateTimeInterval;
using MedicApp.Domain.Dto.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MedicApp.Api.Endpoints.Doctor.Schedule.CreateAnInterval
{
    /// <summary>
    /// ЦЕ Є ЕНЖПЛІГЬ ДДЯ СТВОРЕнНЯ ІНТВРВАЛУ
    ///
    /// Що ж це за фукнція, якщо користувач шле запит
    /// у цьому запиті немає інформації про візит, то це й
    /// запит просто створить інтверал
    /// 1. Якщо розкал для цього інтервалу існує, то створиться тілкьи інтервал
    /// 2. Якщо розладу немєа. то створиься розклад спочатку, а потім уже для нього створиться інтервал
    ///
    ///
    /// ЯКЩО У ЗАПИТі Є ІНФОРМАЦІЯ ПРО ВІЗТ
    /// ТО ПОВТОРЯЄТЬСЯ ДВА ПОПЕРЕДНІ ПУНКТИ
    /// АЛЕ ДО НОВОСТВОРЕНОГО ІНТЕРВАЛУ МИ ПРИКРІПЛУЄМ ЛІКАРЯ ТА ВІЗИТ
    /// </summary>
    public class CreateIntervalEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/doctor/schedule/interval", Handler);
        }

        private async Task<IResult> Handler(HttpContext context, IMediator mediator, [FromBody] IntervalDto intervals)
        {
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }
            
            
            var command = new CreateTimeIntervalCommand(userId, intervals);
            var result = await mediator.Send(command);

            if (result is false)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
    }
}