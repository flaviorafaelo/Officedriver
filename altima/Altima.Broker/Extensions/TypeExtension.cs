using System;
using System.Collections.Generic;
using Altima.Broker.Business.Types;

namespace Altima.Broker.Extensions
{
    public static class TypeExtension
    {
        public static DataType ToDataType(this Type type)
        {

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return DataType.Record;

            if (type.IsSubclassOf(typeof(BlobType)))
                return DataType.Blob;

            if (type.IsSubclassOf(typeof(ImageType)))
                return DataType.Image;

            if (type.IsSubclassOf(typeof(ObjectType)))
                return DataType.Object;

            if (type.IsEnum)
                return DataType.List;

            if (type.IsSubclassOf(typeof(StringType)))
                return DataType.String;

            if (type.IsSubclassOf(typeof(IntegerType)))
                return DataType.Integer;

            if (type.IsSubclassOf(typeof(DateType)))
                return DataType.Date;

            if (type.IsSubclassOf(typeof(DateTimeType)))
                return DataType.DateTime;

            if (type.IsSubclassOf(typeof(BooleanType)))
                return DataType.Boolean;

            return DataType.Unknown;
        }
    }
}
