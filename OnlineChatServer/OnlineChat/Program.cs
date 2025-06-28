using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnlineChat;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.Application.Services;
using OnlineChat.DatabaseAccess;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;
using OnlineChat.DatabaseAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDbContext"));
});

builder.Services.AddSingleton(new PasswordHasher<object>());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IBaseRepository<Chat>, ChatRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBaseService<ChatDto>, BaseService<Chat, ChatDto, IBaseRepository<Chat>>>();

builder.Services.AddAutoMapper(typeof(AutoMapperConf));
builder.Services.AddControllers();
builder.Services.AddCors(policy =>
{
	policy.AddDefaultPolicy(x => 
		x.WithOrigins("http://localhost:4200")
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
	dbContext.Database.CanConnect();
	dbContext.Database.Migrate();
}


app.MapControllers();
app.UseCors();
app.UseHttpsRedirection();

app.Run();