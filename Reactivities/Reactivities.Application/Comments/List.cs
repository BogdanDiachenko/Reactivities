using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Comments;

public class List
{
    public class Query : IRequest<ServiceResult<List<CommentDto>>>
    {
        public Guid ActivityId { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ServiceResult<List<CommentDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .Include(x =>x.Activity)
                .Include(x => x.Author)
                .Where(c => c.Activity.Id == request.ActivityId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return comments.Any()
                ? ServiceResult<List<CommentDto>>.CreateSuccess(comments.Select(x => x.ToDto()).ToList())
                : ServiceResult<List<CommentDto>>.CreateSuccess(new List<CommentDto>());
        }
    }
}