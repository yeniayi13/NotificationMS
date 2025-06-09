using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Domain.Entities.ValueObjects
{
    public class NotificationId
    {
        private NotificationId(Guid value) => Value = value;

        public static NotificationId Create()
        {
            return new NotificationId(Guid.NewGuid());
        }
        public static NotificationId? Create(Guid value)
        {
            // if (value == Guid.Empty) throw new NullAttributeException("Product id is required");

            return new NotificationId(value);
        }

        public static NotificationId Create(object value)
        {
            throw new NotImplementedException();
        }

        public Guid Value { get; init; }
    }
}
