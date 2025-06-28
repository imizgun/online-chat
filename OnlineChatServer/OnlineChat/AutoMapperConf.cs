using AutoMapper;
using OnlineChat.Application.DTOs;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat;

public class AutoMapperConf : Profile
{
	public AutoMapperConf()
	{
		CreateMap<User, UserDto>()
			.PreserveReferences()
			.MaxDepth(1)
			.ReverseMap();
		
		CreateMap<Message, MessageDto>()
			.PreserveReferences()
			.MaxDepth(1)
			.ReverseMap();
		
		CreateMap<Chat, ChatDto>()
			.PreserveReferences()
			.MaxDepth(1)
			.ReverseMap();
	}
}