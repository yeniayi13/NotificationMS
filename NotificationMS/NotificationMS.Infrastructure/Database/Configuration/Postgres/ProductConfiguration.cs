
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using NotificationMS.Domain.Entities;
using NotificationMS.Domain.Entities.ValueObjects;

namespace NotificationMS.Infrastructure.Database.Configuration.Postgres
{

    [ExcludeFromCodeCoverage]
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(n => n.NotificationId);

            builder.Property(n => n.NotificationId)
                .HasConversion(notificationId => notificationId.Value, value => NotificationId.Create(value)!)
                .IsRequired();

            builder.Property(n => n.NotificationMessage)
                .HasConversion(notificationMessage => notificationMessage.Value, value => NotificationMessage.Create(value)!)
                .IsRequired();

            builder.Property(n => n.NotificationSubject)
                .HasConversion(notificationSubject => notificationSubject.Value, value => NotificationSubject.Create(value)!)
                .IsRequired();

            builder.Property(n => n.NotificationUserId)
                .HasConversion(notificationUserId => notificationUserId.Value, value => NotificationUserId.Create(value)!)
                .IsRequired();

            builder.Property(n => n.NotificationDateTime)
                .HasConversion(notificationDateTime => notificationDateTime.Value, value => NotificationDateTime.Create(value)!)
                .IsRequired();

            builder.Property(n => n.NotificationStatus)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}