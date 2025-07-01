using AutoMapper;
using OnlineChat.Application.DTOs;
using OnlineChat.DatabaseAccess.Entities;
using OnlineChat.Responses;

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
			.MaxDepth(3)
			.ReverseMap();

		CreateMap<UserDto, UserResponse>()
			.PreserveReferences()
			.MaxDepth(1);

		CreateMap<ChatDto, ChatResponse>()
			.PreserveReferences()
			.MaxDepth(1);

		CreateMap<MessageDto, MessageResponse>()
			.PreserveReferences()
			.MaxDepth(1);
	}
}