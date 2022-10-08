using Definitiv.MicrosoftGraph.Web.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Definitiv.MicrosoftGraph.Web.Queries;

public record GetEmployeesQuery() : IRequest<List<Employee>>;

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeesQuery, List<Employee>>
{
    private readonly LeaveApplicationDbContext dbContext;

    public GetEmployeeQueryHandler(LeaveApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<List<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        return dbContext.Employees.ToListAsync(cancellationToken);
    }
}
