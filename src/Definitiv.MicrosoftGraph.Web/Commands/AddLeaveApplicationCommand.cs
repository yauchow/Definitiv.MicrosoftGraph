using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Services;
using MediatR;
using Microsoft.Graph;

namespace Definitiv.MicrosoftGraph.Web.Commands;

public record AddLeaveApplicationCommand(Guid EmployeeId, LeaveApplication LeaveApplication) : IRequest;

public class AddLeaveApplicationCommandHandler : IRequestHandler<AddLeaveApplicationCommand>
{
    private readonly LeaveApplicationDbContext dbContext;
    private readonly MicrosoftGraphService microsoftGraphService;

    public AddLeaveApplicationCommandHandler(
        LeaveApplicationDbContext dbContext, 
        MicrosoftGraphService microsoftGraphService)
    {
        this.dbContext = dbContext;
        this.microsoftGraphService = microsoftGraphService;
    }

    public async Task<Unit> Handle(
        AddLeaveApplicationCommand request, 
        CancellationToken cancellationToken)
    {
        var leaveApplication = request.LeaveApplication;
        leaveApplication.EmployeeId = request.EmployeeId;

        leaveApplication.OutlookEventId = await AddNewOutlookEvent(leaveApplication, cancellationToken); ;

        dbContext.Add(leaveApplication);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<string> AddNewOutlookEvent(
        LeaveApplication leaveApplication,
        CancellationToken cancellationToken)
    {
        var employee = dbContext.Employees.Find(leaveApplication.EmployeeId) ?? throw new Exception("!!!");

        var userId = await microsoftGraphService.GetActiveDirectoryUserId(employee.EmailAddress, cancellationToken);

        var calendarId = await microsoftGraphService.GetUserCalendarId(userId, cancellationToken);

        var @event = new Event
        {
            Subject = leaveApplication.LeaveType == LeaveType.AnnualLeave ? "Annual Leave" : "Sick Leave",
            Start = new DateTimeTimeZone
            {
                DateTime = leaveApplication.From.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                TimeZone = "W. Australia Standard Time"
            },
            End = new DateTimeTimeZone
            {
                DateTime = leaveApplication.To.Date.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                TimeZone = "W. Australia Standard Time"
            },
            IsAllDay = true,
        };

        @event = await microsoftGraphService.CreateUserCalendarEvent(
            userId,
            calendarId,
            @event,
            cancellationToken);

        return @event.Id;
    }
}
