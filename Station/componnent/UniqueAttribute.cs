using System.ComponentModel.DataAnnotations;

using System.Linq;
using Station.Data;

namespace Station.componnent
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            int meterNumber = (int)value;
            int subscriberNumber = (int)value;

            bool meterExists = context.Meters.Any(m => m.numberMeter == meterNumber);
            bool subscriberExists = context.Subscribers.Any(s => s.SubscriberNumber == subscriberNumber);

            if (meterExists)
            {
                return new ValidationResult("This meter number already exists.");
            }

            if (subscriberExists)
            {
                return new ValidationResult("This subscriber number already exists.");
            }

            return ValidationResult.Success;
        }
    }
}