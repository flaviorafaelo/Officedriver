using System;

namespace Altima.Broker.Business.Types
{
    public class DateType : BaseType<DateTime>
    {
        public DateType(DateTime value) => Value = value;
        public static implicit operator DateType(DateTime value) => new DateType(value);

    }
}
