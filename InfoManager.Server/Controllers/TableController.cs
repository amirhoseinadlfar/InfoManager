using InfoManager.Server.Controllers.Requests;
using InfoManager.Server.Models;
using InfoManager.Server.Services;
using InfoManager.Server.Services.TableHandlers;
using InfoManager.Server.Ulitis;

using InfoManagerShared.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfoManager.Server.Controllers
{
    [ApiController]
    [Route("/api/table/[action]")]
    [Authorize("User")]
    public class TableController : ControllerBase
    {
        private readonly MainDbUnitOfWork unitOfWork;
        private readonly ITableHandler tableHandler;

        public TableController(MainDbUnitOfWork unitOfWork, ITableHandler tableHandler)
        {
            this.unitOfWork = unitOfWork;
            this.tableHandler = tableHandler;
        }
        [HttpPut]
        public async Task<ActionResult<TableDto>> Create([FromForm] CreateTableRequest request)
        {
            Session session = HttpContext.GetSession();
            await unitOfWork.SessionRepository.LoadUserAsync(session);
            var result = await unitOfWork.GetMember(session.User, request.SpaceId);
            if(result.IsT0 == false)
            {
                return NotFound();
            }

            Table table = new Table
            {
                Name = request.TableName,
                Space = result.AsT0.Item2
            };
            await unitOfWork.TableRepository.AddTable(table);
            await unitOfWork.SaveChangesAsync();
            await tableHandler.CreateTableAsync(table);
            
            return Ok(new TableDto
            {
                Id = table.Id,
                Name = table.Name,
                SpaceId = table.SpaceId
            });
        }
    }
}
