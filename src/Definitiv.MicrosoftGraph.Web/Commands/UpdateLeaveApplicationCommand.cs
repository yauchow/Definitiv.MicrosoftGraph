using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Services;
using MediatR;
using Microsoft.Graph;

namespace Definitiv.MicrosoftGraph.Web.Commands;

public record UpdateLeaveApplicationCommand(Guid EmployeeId, Guid LeaveApplicationId, LeaveApplication LeaveApplication) : IRequest;

public class UpdateLeaveApplicationCommandHandler : IRequestHandler<UpdateLeaveApplicationCommand>
{
    private readonly LeaveApplicationDbContext dbContext;
    private readonly MicrosoftGraphService microsoftGraphService;

    public UpdateLeaveApplicationCommandHandler(
        LeaveApplicationDbContext dbContext,
        MicrosoftGraphService microsoftGraphService)
    {
        this.dbContext = dbContext;
        this.microsoftGraphService = microsoftGraphService;
    }

    public async Task<Unit> Handle(
        UpdateLeaveApplicationCommand request, 
        CancellationToken cancellationToken)
    {
        var leaveApplication = request.LeaveApplication;

        leaveApplication.Id = request.LeaveApplicationId;
        leaveApplication.EmployeeId = request.EmployeeId;

        await UpdateOutlookCalendarEvent(leaveApplication, cancellationToken);

        dbContext.Update(leaveApplication);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task UpdateOutlookCalendarEvent(LeaveApplication leaveApplication, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(leaveApplication.OutlookEventId)) return;

        var employee = dbContext.Employees.Find(leaveApplication.EmployeeId) ?? throw new Exception("!!!");

        var userId = await microsoftGraphService.GetActiveDirectoryUserId(
            employee.EmailAddress, 
            cancellationToken);

        var calendarId = await microsoftGraphService.GetUserCalendarId(
            userId, 
            cancellationToken);

        var @event = await microsoftGraphService.GetUserCalendarEvent(
            userId, 
            calendarId, 
            leaveApplication.OutlookEventId, 
            cancellationToken);

        @event.Subject = leaveApplication.LeaveType == LeaveType.AnnualLeave ? "Annual Leave" : "Sick Leave";

        @event.Start = new DateTimeTimeZone
        {
            DateTime = leaveApplication.From.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
            TimeZone = "W. Australia Standard Time"
        };

        @event.End = new DateTimeTimeZone
        {
            DateTime = leaveApplication.To.Date.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            TimeZone = "W. Australia Standard Time"
        };

        await microsoftGraphService.UpdateUserCalendarEvent(
            userId,
            calendarId,
            @event,
            cancellationToken);
    }
}
