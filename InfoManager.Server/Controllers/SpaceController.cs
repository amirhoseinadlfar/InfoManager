using Azure.Core;

using InfoManager.Server.Controllers.Requests;
using InfoManager.Server.Dtos;
using InfoManager.Server.Models;
using InfoManager.Server.Services;
using InfoManager.Server.Services.Repositorys;
using InfoManager.Server.Ulitis;
using InfoManager.Shared.Requests;

using InfoManagerShared.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OneOf;

using System.ComponentModel.DataAnnotations.Schema;

namespace InfoManager.Server.Controllers;

[ApiController]
[Route("/api/space/[action]")]
[Authorize("User")]
[Produces("application/json")]

[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class SpaceController : ControllerBase
{
    // TODO : Add perimissions logic
    private readonly MainDbUnitOfWork unitOfWork;

    public SpaceController(MainDbUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Create([FromForm] CreateSpaceRequest request)
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpaceDto>> Get(int spaceId)
    {
        var session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        var space =  await unitOfWork.SpaceRepository.FindAsync(spaceId);
        if (space is null)
            return NotFound();
        var memberShip = unitOfWork.SpaceMemberRepository.FindAsync(space, session.User);
        if (memberShip is null)
            return NotFound();
        return new SpaceDto
        {
            Id = space.Id,
            Name = space.Name
        };
    }
    [HttpPatch]
    public async Task<IActionResult> Edit([FromForm] EditSpaceRequest request)
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        var result = await unitOfWork.GetMember(session.User, request.SpaceId);
        if(result.IsT0 == false)
        {
            return NotFound();
        }
        Space space = result.AsT0.Item2;
        if(request.Name is not null)
        {
            space.Name = request.Name;
        }

        await unitOfWork.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromForm] DeleteSpaceRequest request)
    {
        var sesssion = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(sesssion);

        var result = await unitOfWork.GetMember(sesssion.User, request.SpaceId);
        if(result.IsT0 == false)
        {
            return NotFound();
        }
        var mem = result.AsT0;
        await unitOfWork.SpaceRepository.DeleteAsync(mem.Item2);
        await unitOfWork.SaveChangesAsync();

        return Ok();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <response code="404">فضا پیدا نشد یا اینکه کاربر عضو فضا نیست</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddMember([FromForm] AddSpaceMemberRequest request)
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
            return NotFound();
        }
        

        SpaceMember spaceMember = new SpaceMember()
        {
            Space = space,
            User = session.User,
            Type = SpaceMemberType.Member
        };
        await unitOfWork.SpaceMemberRepository.AddAsync(spaceMember);
        await unitOfWork.SaveChangesAsync();

        return Ok();
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMember([FromForm] DeleteMemberRequest request)
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        var mem = await unitOfWork.GetMember(session.User,0);
        if(mem.IsT0 == false)
        {
            return NotFound();
        }
        // TODO : return diffrent code for each not found
        User? delUser = await unitOfWork.UserRepository.FindAsync(request.MemberId);
        if(delUser is null)
        {
            return NotFound();
        }
        SpaceMember? delMember = await unitOfWork.SpaceMemberRepository.FindAsync(mem.AsT0.Item2, delUser);
        if(delMember is null)
        {
            return NotFound();
        }
        await unitOfWork.SpaceMemberRepository.DeleteAsync(delMember);
        await unitOfWork.SaveChangesAsync();

        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<TableDto[]>> GetTables([FromForm] GetSpaceTablesRequest request)
    {
        Session session = HttpContext.GetSession();
        await unitOfWork.SessionRepository.LoadUserAsync(session);
        var result = await unitOfWork.GetMember(session.User, request.SpaceId);
        if (result.IsT0 == false)
            return NotFound();
        await unitOfWork.SpaceRepository.LoadTables(result.AsT0.Item2);
        return result.AsT0.Item2.Tables.Select(x =>
        new TableDto
        {
            Id = x.Id,
            Name = x.Name,
            SpaceId = x.SpaceId
        }
        ).ToArray();
    }
}
