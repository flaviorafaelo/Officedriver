using System;
using System.Collections.Generic;
using System.Reflection;
using Altima.Broker.Business;
using Altima.Broker.Business.Types;
using Altima.Broker.Extensions;
using Altima.Broker.System;

namespace Altima.Broker.Metadata.Generator
{

    public static class ModelGenerator
    {
        public static Model Create(Type type)
        {
            IProperty property = null;
            IList<IProperty> properties = new List<IProperty>();

            PropertyInfo[] props = type.GetProperties();
            foreach (var propInfo in props)
            {
                DataType dataType = propInfo.PropertyType.ToDataType();
                bool required = false;
                int size = 0;
                int maxLen = 0;
                int minLen = 0;
                string mask = "";
                string validation = "";

                object[] typeAttrs = propInfo.PropertyType.GetCustomAttributes(true);
                foreach (object attr in typeAttrs)
                {
                    if (attr is TypeAttribute typeAttr)
                    {
                        size = typeAttr.Size;
                        maxLen = typeAttr.MaxLen;
                        minLen = typeAttr.MinLen;
                        mask = typeAttr.Mask;
                        validation = typeAttr.Validation;
                    }
                }

                object[] modelAttrs = propInfo.GetCustomAttributes(true);
                foreach (object attr in modelAttrs)
                {
                    if (attr is ModelAttribute modelAttr)
                    {
                        required = modelAttr.Required;
                    }
                }

                switch (dataType)
                {
                    case DataType.String:
                        property = new StringProperty(propInfo.Name, size, required);
                        break;
                    case DataType.Integer:
                        property = new IntegerProperty(propInfo.Name, required);
                        break;
                    case DataType.Date:
                        property = new DateProperty(propInfo.Name, required);
                        break;
                    case DataType.DateTime:
                        property = new DateTimeProperty(propInfo.Name, required);
                        break;
                    case DataType.Boolean:
                        property = new BooleanProperty(propInfo.Name, required);
                        break;
                    case DataType.Numeric:
                        property = new NumericProperty(propInfo.Name, 2, required);
                        break;
                    case DataType.Image:
                        break;
                    case DataType.Record:
                        property = new RecordProperty(propInfo.Name, required, ModelGenerator.Create(propInfo.PropertyType).Properties);
                        break;
                    case DataType.Blob:
                        break;
                    case DataType.Object:
                        property = new ObjectProperty(propInfo.Name, required, ModelGenerator.Create(propInfo.PropertyType).Properties);
                        break;
                    case DataType.List:
                        IList<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
                        foreach (int i in propInfo.PropertyType.GetEnumValues())
                            values.Add(new KeyValuePair<string, string>(i.ToString(), Enum.GetName(propInfo.PropertyType, i)));
                        property = new ListProperty(propInfo.Name, required, values);
                        break;
                    default:
                        property = new UnknownProperty(propInfo.Name, required);
                        break;
                }
                if (property != null)
                    properties.Add(property);
            }
            return new Model(type.FullName, type.Name, properties);
        }
    }
}