using Reactivities.Core.DTOs;
using Reactivities.Core.DTOs.User;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Identity;
using Reactivities.Core.Models.Relations;

namespace Reactivities.Core.Helpers;

public static class Mapper
{
    public static UserDto ToDto(this ApplicationUser user, string token)
    {
        return new UserDto
        {
            DisplayName = user.DisplayName,
            Username = user.UserName,
            Image = user.Photos.FirstOrDefault(ph => ph.IsMain)?.Url,
            Token = token
        };
    }
    
    public static ActivityDto ToDto(this Activity activity, string username)
    {
        return new ActivityDto
        {
            IsCancelled = activity.IsCancelled,
            Id = activity.Id,
            Title = activity.Title,
            Category = activity.Category,
            Description = activity.Description,
            City = activity.City,
            Venue = activity.Venue,
            Date = activity.Date,
            HostUsername = activity.Attendees.SingleOrDefault(x => x.IsHost)?.ApplicationUser.UserName,
            Attendees = activity.Attendees.Select(x => x.ToDto(username)).ToList(),
        };
    }

    public static Profile ToProfile(this ApplicationUser user, string username)
    {
        return  new Profile
        {
            DisplayName = user.DisplayName,
            UserName = user.UserName,
            Bio = user.Bio,
            Photos =  user.Photos,
            Image = user.Photos.FirstOrDefault(ph => ph.IsMain)?.Url,
            FollowingCount = user.Followings.Count,
            FollowersCount = user.Followers.Count,
            Following = user.Followers.Select(x => x.Observer.UserName).Contains(username)
        };
    }
    
    public static AttendeeDto ToDto(this ActivityAttendee activityAttendee, string username)
    {
        return  new AttendeeDto
        {
            DisplayName = activityAttendee.ApplicationUser.DisplayName,
            UserName = activityAttendee.ApplicationUser.UserName,
            Bio = activityAttendee.ApplicationUser.Bio,
            Image = activityAttendee.ApplicationUser.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            FollowingCount = activityAttendee.ApplicationUser.Followings.Count,
            FollowersCount = activityAttendee.ApplicationUser.Followers.Count,
            Following = activityAttendee.ApplicationUser.Followers.Select(x => x.Observer.UserName).Contains(username)
        };
    }

    public static CommentDto ToDto(this Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            CreatedAt = comment.CreatedAt,
            Body = comment.Body,
            DisplayName = comment.Author.DisplayName,
            Username = comment.Author.UserName,
            Image = comment.Author.Photos.SingleOrDefault(x => x.IsMain)?.Url
        };
    }

    public static UserActivityDto ToUserActivityDto(this ActivityAttendee attendee)
    {
        return new()
        {
            Date = attendee.Activity.Date,
            Title = attendee.Activity.Title,
            Id = attendee.Activity.Id,
            HostUsername = attendee.Activity.Attendees.SingleOrDefault(x => x.IsHost)?.ApplicationUser.UserName,
            Category = attendee.Activity.Category,
        };
    }
}