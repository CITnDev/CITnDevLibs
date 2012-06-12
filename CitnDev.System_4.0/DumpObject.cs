using System;
using System.Reflection;

namespace CitnDev.System
{
    public static class DumpObject
    {
        public const string NullRepresentation = "<null>";

        public static void ConsoleDump<T>(T value, string indentString = "\t")
        {
            Console.WriteLine(Dump(value,0, indentString));
        }

        public static string Dump<T>(T value, int indentCount, String indentString)
        {
            if (indentCount > 100)
                return string.Empty;

            string dumpText;
            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            var type = typeof(T);
            if (IsValueType(type))
            {
                dumpText = indent + "- " + DumpValueType(value);
            }
            else if (type.IsClass)
            {
                dumpText = indent + "+ " + value;
                foreach (var propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.CanRead)
                        dumpText += Environment.NewLine + DumpProperty(propertyInfo, value, indentCount + 1, indentString);
                }
            }
            else
                throw new NotImplementedException("Can't dump type '" + type + "'.");
            
            return dumpText;
        }

        private static string DumpValueType<T>(T value)
        {
            if ((typeof(T).IsClass || typeof(T).IsGenericType) && value == null)
                return NullRepresentation;

            return value.ToString();
        }

        private static string DumpProperty(PropertyInfo property, object instance, int indentCount, string indentString)
        {
            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            var value = property.GetGetMethod().Invoke(instance, null);

            if (IsValueType(property.PropertyType))
                return indent + "- " + property.Name + " = " + DumpValueType(value);

            return DumpObjectInstance(property, value, indentCount, indentString);
        }

        private static string DumpObjectInstance(PropertyInfo property, object value, int indentCount, string indentString)
        {
            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            if (value == null)
                return indent + "+ " + property.Name + " = " + NullRepresentation;

            string dumpText = indent + "+ " + property.Name + " = " + property.PropertyType;
            foreach (var propertyInfo in property.PropertyType.GetProperties())
            {
                if (propertyInfo.CanRead)
                    dumpText += Environment.NewLine + DumpProperty(propertyInfo, value, indentCount + 1, indentString);
            }

            return dumpText;
        }

        private static bool IsValueType(Type type)
        {
            return type == typeof (string) || type.IsValueType;
        }
    }
}
