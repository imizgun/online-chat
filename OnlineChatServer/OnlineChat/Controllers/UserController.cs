using Microsoft.AspNetCore.Mvc;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.Bodies;
using OnlineChat.Responses;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
	private IUserService _userService;
	
	public  UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	public async Task<ActionResult<List<UserDto>>> GetAll(int? take, int? skip)
	{
		var notNullTake = take ?? 5;
		var notNullSkip = skip ?? 1;
		return await _userService.GetAllAsync(notNullSkip, notNullTake);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<UserResponse?>> Get(Guid id)
	{
		var user = await _userService.GetAsync(id);
		
		if (user == null) return NotFound();
		return new UserResponse {Name = user.Name, Email = user.Email, Id = user.Id};
	}

	[HttpPost]
	public async Task<ActionResult> Post([FromBody] UserBody user)
	{
		UserDto userDto = new (){
			Name = user.Name,
			Email = user.Email,
			Password = user.Password
		};
		
		var res = await _userService.CreateAsync(userDto);
		
		if (res == Guid.Empty) return Conflict(new {message = "User with this email already exists."});
		
		return Ok(new {message = "User signed up successfully"});
	}

	[HttpPost("auth")]
	public async Task<ActionResult> Login([FromBody] LoginBody user)
	{
		var log = await _userService.Login(user.Email, user.Password);
		
		if (log == null) return Unauthorized();
		
		return Ok(new UserResponse {Name = log.Name, Email = log.Email, Id = log.Id});
	}
}