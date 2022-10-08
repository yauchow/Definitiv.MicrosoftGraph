using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Services;
using MediatR;

namespace Definitiv.MicrosoftGraph.Web.Commands;

public record DeleteLeaveApplicationCommand(Guid employeeId, Guid leaveApplicationId) : IRequest;

public class DeleteLeaveApplicationCommandHandler: IRequestHandler<DeleteLeaveApplicationCommand>
{
	private readonly LeaveApplicationDbContext dbContext;
	private readonly MicrosoftGraphService microsoftGraphService;

	public DeleteLeaveApplicationCommandHandler(
        LeaveApplicationDbContext dbContext, 
        MicrosoftGraphService microsoftGraphService)
	{
		this.dbContext = dbContext;
		this.microsoftGraphService = microsoftGraphService;
	}

	public async Task<Unit> Handle(
        DeleteLeaveApplicationCommand request, 
        CancellationToken cancellationToken)
	{
        var leaveApplication =
            dbContext.LeaveApplications.Find(
                request.leaveApplicationId, 
                request.employeeId) ?? throw new Exception("!!!");

        await DeleteOutlookCalendarEvent(leaveApplication, cancellationToken);

        dbContext.Remove(leaveApplication);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task DeleteOutlookCalendarEvent(LeaveApplication leaveApplication, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(leaveApplication.OutlookEventId)) return;

        var employee = dbContext.Employees.Find(leaveApplication.EmployeeId) ?? throw new Exception("!!!");

        var userId = await microsoftGraphService.GetActiveDirectoryUserId(
            employee.EmailAddress,
            cancellationToken);

        var calendarId = await microsoftGraphService.GetUserCalendarId(
            userId,
            cancellationToken);

        await microsoftGraphService.DeleteUserCalendarEvent(
            userId,
            calendarId,
            leaveApplication.OutlookEventId,
            cancellationToken);
    }
}
