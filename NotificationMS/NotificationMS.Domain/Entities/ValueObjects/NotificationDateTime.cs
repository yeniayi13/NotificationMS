using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Domain.Entities.ValueObjects
{
    public partial class NotificationDateTime
    {
        private const string Pattern = @"^[a-zA-Z]+$";
        private NotificationDateTime(DateTime value) => Value = value;

        public static NotificationDateTime Create()
        {
            return new NotificationDateTime(new DateTime());
        }
        public static NotificationDateTime Create(DateTime value)
        {
            try
            {
                // if (string.IsNullOrEmpty(value)) throw new NullAttributeException("Product Name is required");
                //if (!NameRegex().IsMatch(value)) throw new InvalidAttributeException("Product Name is invalid");

                return new NotificationDateTime(value);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public DateTime Value { get; init; } //*init no permite setear desde afuera, solo desde el constructor


    }
}
