using Definitiv.MicrosoftGraph.Web.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Definitiv.MicrosoftGraph.Web.Queries;

public record GetLeaveApplicationsQuery() : IRequest<List<LeaveApplication>>;

public class GetLeaveApplicationQueryHandler : IRequestHandler<GetLeaveApplicationsQuery, List<LeaveApplication>>
{
    private readonly LeaveApplicationDbContext dbContext;

    public GetLeaveApplicationQueryHandler(LeaveApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<LeaveApplication>> Handle(GetLeaveApplicationsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.LeaveApplications.ToListAsync(cancellationToken);
    }
}
