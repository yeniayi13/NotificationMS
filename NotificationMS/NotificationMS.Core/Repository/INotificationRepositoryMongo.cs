
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationMS.Domain.Entities;


namespace ProductsMS.Core.Repository
{
    public interface INotificationRepositoryMongo
    {
      Task<List<Notification>> GetAllByUserIdAsync(Guid userId);
    }
}
