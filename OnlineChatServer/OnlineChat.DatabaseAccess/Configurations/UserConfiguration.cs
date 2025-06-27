using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Email).IsRequired();
		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.Password).IsRequired();
		
		builder.HasMany(x => x.Chats)
			.WithMany(x => x.Members);
	}
}