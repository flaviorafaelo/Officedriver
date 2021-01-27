using System;
using System.Collections.Generic;
using System.Linq;
using Altima.Broker.Business.Types;

namespace Altima.Broker.Extensions
{
    public static class TypeExtension
    {
        public static bool Implements(this Type self, Type type)
        {
            var types = self.Assembly.GetExportedTypes().Where(x =>
                type.IsAssignableFrom(x) && /*!x.IsAbstract &&*/ x.Name.Equals(self.Name)).ToList();
            return types.Count() > 0; 
        }

        public static string ToStringType(this Type type)
        {
            if (Implements(type, typeof(IBlobType)))
                return "blob";

            if (Implements(type, typeof(IImageType)))
                return "image";

            if (Implements(type, typeof(IList<>)) && Implements(type, typeof(IObjectType)))
                return "record";

            if (Implements(type, typeof(IObjectType)))
                return "object";

            if (type.IsEnum)
                return "list-fixed";

            if (Implements(type,typeof(IStringType))) 
                return "string";

            if (Implements(type, typeof(IIntegerType)))
                return "integer";

            if (Implements(type, typeof(IDateType)))
                return "date";

            if (Implements(type, typeof(IDateTimeType)))
                return "datetime";

            if (Implements(type, typeof(IBooleanType)))
                return "boolean";

           

            return "unknown";
        }
    }
}
