using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationMS.Core.Database;
using NotificationMS.Domain.Entities;


namespace NotificationMS.Core.Database
{
    public interface IApplicationDbContext
    {
        DbContext DbContext { get; }
       DbSet<Notification> Notifications { get; set; }
     
       

        IDbContextTransactionProxy BeginTransaction();

        void ChangeEntityState<TEntity>(TEntity entity, EntityState state);

        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
    }
}