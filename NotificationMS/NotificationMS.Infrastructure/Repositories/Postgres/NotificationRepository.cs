using AutoMapper;
using NotificationMS.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationMS.Domain.Entities;
using ProductsMs.Core.Repository;

namespace NotificationMS.Infrastructure.Repositories.Postgres
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationRepository(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddAsync(Notification notification)
        {
           
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveEfContextChanges("");
        }
    }
}
