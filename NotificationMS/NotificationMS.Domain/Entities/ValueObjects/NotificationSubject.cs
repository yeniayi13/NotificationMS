using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Domain.Entities.ValueObjects
{
    public partial class NotificationSubject
    {
        private const string Pattern = @"^[a-zA-Z]+$";
        private NotificationSubject(string value) => Value = value;

        public static NotificationSubject Create(string value)
        {
            try
            {
                // if (string.IsNullOrEmpty(value)) throw new NullAttributeException("Product Name is required");
                //if (!NameRegex().IsMatch(value)) throw new InvalidAttributeException("Product Name is invalid");

                return new NotificationSubject(value);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string Value { get; init; } //*init no permite setear desde afuera, solo desde el constructor


    }
}
