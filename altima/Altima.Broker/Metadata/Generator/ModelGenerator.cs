using System;
using System.Collections.Generic;
using System.Reflection;
using Altima.Broker.Extensions;
using Altima.Broker.Business;

namespace Altima.Broker.Metadata.Generator
{
    public static class ModelGenerator
    {
        public static Model Create(Type type)
        {
            IProperty property;
            IList<IProperty> properties = new List<IProperty>();

            PropertyInfo[] props = type.GetProperties();
            foreach (var propInfo in props)
            {
                string typeName = propInfo.PropertyType.ToStringType();

                if (typeName.Equals("object"))//criar um enum Talvez criar um Extension para PropertyType e retornar um property???????
                {
                    Model record = ModelGenerator.Create(propInfo.PropertyType);
                    property = new PropertyObject(propInfo.Name, typeName, propInfo.Name, false, "Geral", record.Properties);//Criar um GeneradorPropery
                }
                else
                if (typeName.Equals("record"))
                {
                    Model record = ModelGenerator.Create(propInfo.PropertyType);
                    property = new PropertyObject(propInfo.Name, typeName, propInfo.Name, false, "Geral", record.Properties);//Criar um GeneradorPropery
                }
                else
                if (typeName.Equals("list-fixed"))
                {
                    IList<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
                    foreach (int i in propInfo.PropertyType.GetEnumValues())
                    {
                        values.Add(new KeyValuePair<string, string>(i.ToString(), Enum.GetName(propInfo.PropertyType, i)));
                    }
                    property = new PropertyListFixed(propInfo.Name, typeName, propInfo.Name, false, "Geral", values);
                }
                else
                {
                    property = new Property(propInfo.Name, typeName, propInfo.Name, "Geral", false);
                }

                object[] attrs = propInfo.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    /*if (attr is TypeAttribute modelAttr)
                    {
                        //property.Size
                        //MaxLen
                        //MinLen
                        //Mask
                        //Validation
    }
                    if (attr is ModelAttribute modelAttr)
                    {
                        //property.Caption = modelAttr.Caption;
                        //property.Required = modelAttr.Required;
                        //property.Group = modelAttr.Group;
                        //property.Order = modelAttr.Order;
                    }*/
                }
                properties.Add(property);
            }
            return new Model(type.Name, properties);
        }
    }
}
