using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.BusinessLogic.TokenService;
using Reactivities.Core.DTOs;
using Reactivities.Core.DTOs.User;
using Reactivities.Core.Helpers;
using Reactivities.Core.Models.Identity;

namespace Reactivities.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var user = await _userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        return result.Succeeded ? Ok(user.ToDto(_tokenService.CreateToken(user))) : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "This email is already taken");
            return ValidationProblem();
        }
        
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "This username is already taken");
            return ValidationProblem();
        }
        
        var user = new ApplicationUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        return result.Succeeded ? Ok(user.ToDto(_tokenService.CreateToken(user))) : BadRequest("Problem registering user");
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("No active user");
        }
        
        var user = await _userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user == null ? BadRequest("No active user") : Ok(user.ToDto(_tokenService.CreateToken(user)));
    }
}