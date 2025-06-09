using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Domain.Entities.ValueObjects
{
    public class NotificationUserId
    {
        private NotificationUserId(Guid value) => Value = value;

        public static NotificationUserId Create()
        {
            return new NotificationUserId(Guid.NewGuid());
        }
        public static NotificationUserId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new NotificationUserId(value);
        }

        public static NotificationUserId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
