using Reactivities.Core.Models.Identity;

namespace Reactivities.BusinessLogic.TokenService;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}