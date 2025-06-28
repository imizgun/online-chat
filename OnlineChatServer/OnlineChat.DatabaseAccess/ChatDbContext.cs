using Microsoft.EntityFrameworkCore;
using OnlineChat.DatabaseAccess.Configurations;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess;

public class ChatDbContext : DbContext
{
	public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) {}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new ChatConfiguration());
		modelBuilder.ApplyConfiguration(new MessageConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
	}
	
	public DbSet<User> Users { get; set; }
	public DbSet<Chat> Chats { get; set; }
	public DbSet<Message> Messages { get; set; }
}	