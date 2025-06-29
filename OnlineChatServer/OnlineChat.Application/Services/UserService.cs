using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.Application.Services;

public class UserService : BaseService<User, UserDto, IUserRepository>, IUserService
{
	private PasswordHasher<object> _passwordHasher;

	public UserService(IMapper mapper, IUserRepository repository, PasswordHasher<object> passwordHasher) : base(mapper,
		repository)
	{
		_passwordHasher = passwordHasher;
	}

	public override async Task<Guid> CreateAsync(UserDto item)
	{
		var user = await _repository.GetByEmail(item.Email);
		if (user != null) return Guid.Empty;
		
		item.Password = _passwordHasher.HashPassword(null, item.Password);
		return await base.CreateAsync(item);
	}

	public async Task<UserDto?> Login(string email, string password)
	{
		var user = await _repository.GetByEmail(email);
		if (user == null) return null;

		return _passwordHasher.VerifyHashedPassword(null, user.Password, password) 
		       != PasswordVerificationResult.Success ? 
		null :
		_mapper.Map<User, UserDto>(user);
	}

	public async Task<UserDto?> GetByEmail(string email)
	{
		var res = await _repository.GetByEmail(email);

		return res != null ? _mapper.Map<User, UserDto>(res) : null;
	}
}