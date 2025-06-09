using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationMS.Common.Primitives;
using NotificationMS.Domain.Entities.Enum;
using NotificationMS.Domain.Entities.ValueObjects;

namespace NotificationMS.Domain.Entities
{
    public sealed class Notification: AggregateRoot
    {
        public NotificationId NotificationId { get; private set; }  
        public NotificationMessage NotificationMessage { get; private set; }
        public NotificationSubject NotificationSubject { get; private set; }
        public NotificationUserId NotificationUserId { get; private set; }
        public NotificationDateTime NotificationDateTime { get; private set; }

        public NotificationStatus NotificationStatus { get; private set; } = NotificationStatus.Pendiente;
        public Notification(NotificationId notificationId,
                            NotificationMessage notificationMessage,
                            NotificationSubject notificationSubject,
                            NotificationUserId notificationUserId,
                            NotificationDateTime notificationDateTime,
                            NotificationStatus notificationStatus)
        {
            NotificationId = notificationId;
            NotificationMessage = notificationMessage;
            NotificationSubject = notificationSubject;
            NotificationUserId = notificationUserId;
            NotificationDateTime = notificationDateTime;
            NotificationStatus = notificationStatus;
        }
    }
}
