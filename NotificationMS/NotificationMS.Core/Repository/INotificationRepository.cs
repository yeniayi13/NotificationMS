

using NotificationMS.Domain.Entities;

namespace ProductsMs.Core.Repository
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
    }
}
