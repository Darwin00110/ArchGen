using Application;
using Microsoft.AspNetCore.Mvc;

namespace API;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserUseCase _usecase;
    public UserController(IUserUseCase userUseCase)
    {
        _usecase = userUseCase;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        try
        {
            var result = await _usecase.CreateUser(request);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new
            {
                Error = e.Message
            });
        }
    }
    [HttpGet("me")]
    public async Task<IActionResult> Read(Guid ID)
    {
        try
        {
            var result = await _usecase.ReadUser(ID);
            return Ok(new
            {
                Data = result
            });
        } catch(Exception e)
        {
            return BadRequest(new
            {
                Error = e.Message
            });
        }
    }
    [HttpPut("me")]
    public async Task<IActionResult> Update(Guid ID, UpdateUserRequest request)
    {
        try
        {
            var result = await _usecase.UpdateUser(ID, request);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new
            {
                Error = e.Message
            });
        }
    }
    [HttpDelete("me")]
    public async Task<IActionResult> Delete(Guid ID)
    {
        try
        {
            var result = await _usecase.DeleteUser(ID);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new
            {
                Error = e.Message
            });
        }
    }
}
