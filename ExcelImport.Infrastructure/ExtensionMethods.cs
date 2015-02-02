using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExcelImport.Infrastructure
{
    public static class ExtensionMethods
    {
        public static List<PropertyInfo> GetSearchFields(this Type type)
        {
            var props = type
                .GetProperties();

            return props.Where(
                    prop => ((Attribute[])prop.GetCustomAttributes(typeof(Attribute), true))
                        .Any()).ToList();
        }
    }
}
