using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden.Extensions
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                if (item.Value is IDictionary<string, object> )
                {
                    var mi = typeof(ObjectExtensions).GetMethod("ToObject");
                    var fooRef = mi.MakeGenericMethod(someObjectType.GetProperty(item.Key).PropertyType);
                    var result = fooRef.Invoke(null, new[] { item.Value });
                    
                    someObjectType
                       .GetProperty(item.Key)
                       .SetValue(someObject, result, null); 
                }
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(IsSimple(source.GetType()) ? source : source.AsDictionary(), null)
            );

        }

        static bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
    }
}
