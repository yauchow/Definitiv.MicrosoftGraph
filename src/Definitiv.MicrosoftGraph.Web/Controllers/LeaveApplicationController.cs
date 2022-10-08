using Definitiv.MicrosoftGraph.Web.Commands;
using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Definitiv.MicrosoftGraph.Web.Controllers;

[ApiController]
public class LeaveApplicationController : ControllerBase
{
    private readonly IMediator mediator;

    public LeaveApplicationController(
        IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("api/leave-applications")]
    public Task<List<LeaveApplication>> GetLeaveApplications(
        CancellationToken cancellationToken) 
        => mediator.Send(
            new GetLeaveApplicationsQuery(), 
            cancellationToken);

    [HttpPost("api/employees/{employeeId}/leave-applications")]
    public Task AddNewLeaveApplication(
        Guid employeeId, 
        LeaveApplication leaveApplication, 
        CancellationToken cancellationToken)
        => mediator.Send(
            new AddLeaveApplicationCommand(
                employeeId, 
                leaveApplication), 
            cancellationToken);


    [HttpPut("api/employees/{employeeId}/leave-applications/{leaveApplicationId}")]
    public Task UpdateLeaveApplication(
        Guid employeeId,
        Guid leaveApplicationId,
        LeaveApplication leaveApplication,
        CancellationToken cancellationToken)
        => mediator.Send(
            new UpdateLeaveApplicationCommand(
                employeeId, 
                leaveApplicationId, 
                leaveApplication), 
            cancellationToken);

    [HttpDelete("api/employees/{employeeId}/leave-applications/{leaveApplicationId}")]
    public Task DeleteLeaveApplication(
        Guid employeeId,
        Guid leaveApplicationId,
        CancellationToken cancellationToken)
        => mediator.Send(
            new DeleteLeaveApplicationCommand(
                employeeId, 
                leaveApplicationId), 
            cancellationToken);
}
