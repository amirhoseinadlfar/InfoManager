using InfoManager.Server.Dtos;
using InfoManager.Server.Models;
using InfoManager.Server.Services;
using InfoManager.Server.Ulitis;
using InfoManager.Shared.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoManager.Server.Controllers;

[ApiController]
[Route("/api/space/[action]")]
[Authorize("User")]
[Produces("application/json")]

[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class SpaceController : ControllerBase
{
    private readonly MainDbUnitOfWork unitOfWork;

    public SpaceController(MainDbUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateNew([FromForm] CreateNewSpaceRequest request)
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        Space space = new Space()
        {
            Name = request.Name,
        };
        await unitOfWork.SpaceRepository.AddAsync(space);
        SpaceMember member = new SpaceMember()
        {
            User = session.User,
            Space = space,
            Type = SpaceMemberType.Owner
        };
        await unitOfWork.SpaceMemberRepository.AddAsync(member);
        await unitOfWork.SaveChangesAsync();
        return Created("",new SpaceDto
        {
            Id = space.Id,
            Name = request.Name,
        });
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddMember([FromForm] AddMemberRequest request)
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        Space? space = await unitOfWork.SpaceRepository.FindAsync(request.SpaceId);
        if (space is null)
        {
            return NotFound();
        }
        SpaceMember? member = await unitOfWork.SpaceMemberRepository.FindAsync(space, session.User);
        if(member is null)
        {
            return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
        }
        return Created(string.Empty, new SpaceMemberDto
        {
            Id = member.Id,
            SpaceId = member.SpaceId,
        });
    }
}
