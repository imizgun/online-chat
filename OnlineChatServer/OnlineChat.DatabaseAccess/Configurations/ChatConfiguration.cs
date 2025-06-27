using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
	public void Configure(EntityTypeBuilder<Chat> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).IsRequired();
		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.IsPrivate).IsRequired();
		
		builder.HasMany(x => x.Messages)
			.WithOne(x => x.Chat)
			.HasForeignKey(x => x.ChatId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}