

using NotificationMS.Domain.Entities;

namespace ProductsMs.Core.Repository
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
      //  Task DeleteAsync(ProductId id);
      //  Task<ProductEntity?> UpdateAsync(ProductEntity product);
    }
}
