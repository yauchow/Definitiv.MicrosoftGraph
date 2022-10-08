using Definitiv.MicrosoftGraph.Web.Data;
using Definitiv.MicrosoftGraph.Web.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Definitiv.MicrosoftGraph.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IMediator mediator;

    public EmployeesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public Task<List<Employee>> GetEmployees(
        CancellationToken cancellationToken)
        => mediator.Send(
            new GetEmployeesQuery(), 
            cancellationToken);
}
