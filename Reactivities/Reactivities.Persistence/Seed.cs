using Microsoft.AspNetCore.Identity;
using Reactivities.Core.Models;
using Reactivities.Core.Models.Identity;
using Reactivities.Core.Models.Relations;
using Reactivities.Persistence.AppDbContext;

namespace Reactivities.Persistence;

public class Seed
{
    public static async Task SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<ApplicationUser>
            {
                new() { DisplayName = "Vasya", UserName = "vasya", Email = "vasya@gmail.com", Bio = "" },
                new() { DisplayName = "Anton", UserName = "anton", Email = "anton@gmail.com", Bio = "" },
                new() { DisplayName = "Vitalii", UserName = "vitalya", Email = "vitalya@gmail.com", Bio = "" }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            var activities = new List<Activity>
            {
                new()
                {
                    Title = "Past Activity 1",
                    Date = DateTimeOffset.Now.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = true
                        }
                    }
                },
                new()
                {
                    Title = "Past Activity 2",
                    Date = DateTimeOffset.Now.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "The Louvre",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 1",
                    Date = DateTimeOffset.Now.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "music",
                    City = "London",
                    Venue = "Wembly Stadium",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[2],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 2",
                    Date = DateTimeOffset.Now.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "food",
                    City = "London",
                    Venue = "Jamies Italian",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[2],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 3",
                    Date = DateTimeOffset.Now.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 4",
                    Date = DateTimeOffset.Now.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "culture",
                    City = "London",
                    Venue = "British Museum",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = true
                        }
                    }
                },
                new()
                {
                    Title = "Future Activity 5",
                    Date = DateTimeOffset.Now.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Punch and Judy",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 6",
                    Date = DateTimeOffset.Now.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[2],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 7",
                    Date = DateTimeOffset.Now.AddMonths(7),
                    Description = "Activity 7 months in future",
                    Category = "travel",
                    City = "Berlin",
                    Venue = "All",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[0],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[2],
                            IsHost = false
                        },
                    }
                },
                new()
                {
                    Title = "Future Activity 8",
                    Date = DateTimeOffset.Now.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                    Attendees = new List<ActivityAttendee>
                    {
                        new()
                        {
                            ApplicationUser = users[2],
                            IsHost = true
                        },
                        new()
                        {
                            ApplicationUser = users[1],
                            IsHost = false
                        },
                    }
                }
            };

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
    }
}