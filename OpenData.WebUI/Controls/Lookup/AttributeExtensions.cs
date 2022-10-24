using System;
using System.Linq;
using System.Reflection;

namespace TestApp.Controls.Lookup
{
    public static class TypeExtensions
    {
        public static T GetCustomAttributeByType<T>(
            this Type type)
            where T : Attribute
        {
            var att = type.GetCustomAttributes(
            typeof(T), true).FirstOrDefault() as T;
            return att;
        }

        public static PropertyInfo GetPropertyWithAttribute(
            this Type attributeType, string typeName)
        {
           var prop = (from property
                                    in attributeType.GetProperties()
             from attribute in property.GetCustomAttributesData()
             where attribute.AttributeType.Name ==typeName
             select property).FirstOrDefault();

            return prop;
        }
    }
}