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

        public static DataType ToDataType(this Type type)
        {
            if (Implements(type, typeof(IBlobType)))
                return DataType.Blob;

            if (Implements(type, typeof(IImageType)))
                return DataType.Image;

            if (Implements(type, typeof(IList<>)) && Implements(type, typeof(IObjectType)))
                return DataType.Record;

            if (Implements(type, typeof(IObjectType)))
                return DataType.Object;

            if (type.IsEnum)
                return DataType.List;

            if (Implements(type,typeof(IStringType))) 
                return DataType.String;

            if (Implements(type, typeof(IIntegerType)))
                return DataType.Integer;

            if (Implements(type, typeof(IDateType)))
                return DataType.Date;

            if (Implements(type, typeof(IDateTimeType)))
                return DataType.DateTime;

            if (Implements(type, typeof(IBooleanType)))
                return DataType.Boolean;         

            return DataType.Unknown;
        }
    }
}
