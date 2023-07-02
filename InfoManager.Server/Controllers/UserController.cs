using InfoManager.Server.Controllers.Requests;
using InfoManager.Server.Dtos;
using InfoManager.Server.Models;
using InfoManager.Server.Services;
using InfoManager.Server.Services.Repositorys.Interfaces;
using InfoManager.Server.Ulitis;
using InfoManager.Shared;
using InfoManager.Shared.Requests;
using InfoManager.Shared.Respones;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace InfoManager.Server.Controllers;

[ApiController]
[Route("/api/user/[action]")]
[Produces("application/json")]

public class UserController : ControllerBase
{
    private readonly MainDbUnitOfWork unitOfWork;

    public UserController(MainDbUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// ثبت نام
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">ثبت نام موفق</response>
    /// <response code="409">تکراری بودن نام کاربری</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SignUp([FromForm] SignUpRequest request)
    {
        if(await unitOfWork.UserRepository.AnyAsync(request.Username))
        {
            return Conflict();
        }

        User newUser = new User()
        {
            Name = request.Name,
            UserName = request.Username,
            Password = request.Password,
        };
        await unitOfWork.UserRepository.AddAsync(newUser);
        await unitOfWork.SaveChangesAsync();
        return Ok();
    }
    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <response code="401">ورود ناموفق</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromForm] LoginRequest request)
    {
        User? currentUser = await unitOfWork.UserRepository.FindAsync(request.Username, request.Password);
        if (currentUser is null)
        {
            return Unauthorized();
        }
        byte[] key = RandomNumberGenerator.GetBytes(Session.KeyLength);
        string stringKey = string.Create(key.Length, key, (chars, y) =>
        {
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)y[i];
            }
        });
        Session session = new Session()
        {
            Key = stringKey,
            User = currentUser,
        };
        await unitOfWork.SessionRepository.AddAsync(session);
        await unitOfWork.SaveChangesAsync();

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim("loginKey",stringKey)
        },CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(new[] { claimsIdentity });
        AuthenticationProperties properties = new AuthenticationProperties()
        {
            
        };
        return base.SignIn(principal,properties);
    }
    /// <summary>
    /// گرفتن نام و نام کاربری (احراز درستی ورود موفق )
    /// </summary>
    /// <response code="403">کاربر وارد نشده یا اینکه دیگر قابل قبول نیست</response>
    [HttpGet]
    [Authorize("User")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetMeRespone>> GetMe()
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);

        return new GetMeRespone()
        {
            Name = session.User.Name,
            Username = session.User.UserName
        };
    }
    [HttpGet]
    [Authorize("User")]
    public async Task<SpaceDto[]> GetSpaces()
    {
        var session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        await unitOfWork.UserRepository.LoadSpaces(session.User);
        return  session.User.Spaces.Select(x=>new SpaceDto()
        {
            Id = x.Id,
            Name = x.Name
        }).ToArray();
    }
}
