using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
	public void Configure(EntityTypeBuilder<Message> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.AuthorId).IsRequired();
		builder.Property(x => x.ChatId).IsRequired();
		builder.Property(x => x.Content).IsRequired();
		builder.Property(x => x.SentAt).IsRequired();
		
		builder
			.HasOne(x => x.Author)
			.WithMany(x => x.Messages)
			.HasForeignKey(x => x.AuthorId)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder
			.HasOne(x => x.Chat)
			.WithMany(x => x.Messages)
			.HasForeignKey(x => x.ChatId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}