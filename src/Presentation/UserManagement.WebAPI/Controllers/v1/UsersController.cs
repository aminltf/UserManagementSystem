using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Common.Models.Filtering;
using UserManagement.Application.Common.Models.Pagination;
using UserManagement.Application.Common.Models.Sorting;
using UserManagement.Application.Features.Users.Commands;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Application.Features.Users.Queries;
using UserManagement.WebAPI.Controllers.Base;

namespace UserManagement.WebAPI.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public class UsersController : ApiControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<UserDto>> Create(CreateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("update/{id:guid}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, UpdateUserCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("soft-delete/{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id, [FromQuery] string deletedBy)
    {
        await Mediator.Send(new DeleteUserCommand(id, deletedBy));
        return NoContent();
    }

    [HttpPost("restore/{id:guid}")]
    public async Task<IActionResult> Restore(Guid id, [FromQuery] string modifiedBy)
    {
        await Mediator.Send(new RestoreUserCommand (id, modifiedBy));
        return NoContent();
    }

    [HttpPost("toggle-status/{id:guid}")]
    public async Task<IActionResult> Toggle(Guid id, [FromQuery] string modifiedBy)
    {
        await Mediator.Send(new ToggleUserStatusCommand (id, modifiedBy));
        return NoContent();
    }

    [HttpPost("change-password/{id:guid}")]
    public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordCommand command)
    {
        if (id != command.UserId) return BadRequest("ID mismatch");
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("get-by-id/{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id, [FromQuery] bool includeDeleted = false)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery (id, includeDeleted)));
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<PageResponse<UserDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10,
                                                                  [FromQuery] string? role = null,
                                                                  [FromQuery] bool? isActive = null,
                                                                  [FromQuery] string? search = null,
                                                                  [FromQuery] string? orderBy = "Username",
                                                                  [FromQuery] string? direction = "asc")
    {
        var query = new GetAllUsersQuery
        {
            Pagination = new PageRequest { PageNumber = page, PageSize = size },
            Sort = new SortOptions { OrderBy = orderBy, Direction = direction },
            Filter = new UserFilter { Role = role, IsActive = isActive },
            SearchTerm = search
        };

        return Ok(await Mediator.Send(query));
    }

    [HttpGet("get-deleted")]
    public async Task<ActionResult<PageResponse<UserDto>>> GetDeleted([FromQuery] int page = 1, [FromQuery] int size = 10,
                                                                      [FromQuery] string? role = null,
                                                                      [FromQuery] bool? isActive = null,
                                                                      [FromQuery] string? search = null,
                                                                      [FromQuery] string? orderBy = "Username",
                                                                      [FromQuery] string? direction = "asc")
    {
        var query = new GetAllDeletedUsersQuery
        {
            Pagination = new PageRequest { PageNumber = page, PageSize = size },
            Sort = new SortOptions { OrderBy = orderBy, Direction = direction },
            Filter = new UserFilter { Role = role, IsActive = isActive },
            SearchTerm = search
        };

        return Ok(await Mediator.Send(query));
    }
}
