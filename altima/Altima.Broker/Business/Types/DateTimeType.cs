using System;

namespace Altima.Broker.Business.Types
{
    public class DateTimeType : BaseType<DateTime>
    {
        public DateTimeType(DateTime value) => Value = value;
        public static implicit operator DateTimeType(DateTime value) => new DateTimeType(value);
    }
}
