using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reactivities.Core.Models.Identity;
using Reactivities.Extensions;
using Reactivities.Hubs;
using Reactivities.Middleware;
using Reactivities.Persistence;
using Reactivities.Persistence.AppDbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch(Exception ex) 
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseXContentTypeOptions();

app.UseReferrerPolicy(opt => opt.NoReferrer());

app.UseXXssProtection(opt => opt.EnabledWithBlockMode());

app.UseXfo(opt => opt.Deny());

app.UseCspReportOnly(opt => opt
    .BlockAllMixedContent()
    .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com"))
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self())
    .ScriptSources(s => s.Self())
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

app.MapHub<ChatHub>("/chat");

app.UseResponseCaching();

await app.RunAsync();