using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reactivities.BusinessLogic.Accessors.PhotoAccessor;
using Reactivities.BusinessLogic.Accessors.UserAccessor;
using Reactivities.BusinessLogic.Activities;
using Reactivities.Core.Helpers;
using Reactivities.Core.Options;
using Reactivities.Persistence.AppDbContext;
using StackExchange.Redis;

namespace Reactivities.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(config => 
                config.RegisterValidatorsFromAssemblyContaining<Create>()
            );
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000", "http://localhost:5000");
            });
        });
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddMediatR(typeof(List.Handler).Assembly);
        services.AddResponseCaching();
        services.AddIdentityServices(config);
        services.AddSignalR();
        
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IPhotoAccessor, PhotoAccessor>();
        services.AddScoped<ICacheService, CacheService>();

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = config.GetConnectionString("Redis");
            opt.InstanceName = "Reactivities_";
        });

        services.AddSingleton<IConnectionMultiplexer>(x =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")));
        
        services.Configure<CloudinaryOptions>(config.GetSection("Cloudinary"));
        return services;
    }
}