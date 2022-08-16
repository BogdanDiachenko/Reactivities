using System.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.BusinessLogic.Photos;
using Reactivities.Core.DTOs;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.BusinessLogic.Comments;

public class Create
{
    public class Command : IRequest<ServiceResult<CommentDto>>
    {
        public string Body { get; set; }

        public Guid ActivityId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, ServiceResult<CommentDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;

        public Handler(ApplicationDbContext dbContext, IUserAccessor userAccessor)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
        }

        public async Task<ServiceResult<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities.FindAsync(request.ActivityId);

            if (activity == null) return null;

            var user = await _dbContext.Users.Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null) return null;

            var comment = new Comment
            {
                Author = user,
                Body = request.Body,
                Activity = activity,
            };
            
            activity.Comments.Add(comment);

            return await _dbContext.SaveChangesAsync() > 0
                ? ServiceResult<CommentDto>.CreateSuccess(comment.ToDto())
                : ServiceResult<CommentDto>.CreateFailure("Failed to add comment");
        }
    }
}