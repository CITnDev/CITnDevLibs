using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace CitnDev.System
{
    public static class DumpObject
    {
        public const string NullRepresentation = "<null>";

        public static void ConsoleDump<T>(string name, T value, string indentString = "\t")
        {
            Console.WriteLine(Dump(name, value, 0, indentString));
        }

        public static string Dump<T>(string name, T value, int indentCount, String indentString)
        {
            return InternalDump(name, typeof(T), value, indentCount, indentString);
        }

        private static bool CanContinueDumping(int indentCount)
        {
            return indentCount < 10;
        }

        private static string InternalDump(string name, Type valueType, object value, int indentCount, string indentString)
        {
            if (!CanContinueDumping(indentCount))
                return "...";

            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            if (IsValueType(valueType))
                return indent + "- " + name + " = " + DumpValueType(value);

            if (CanDirectDumpClass(valueType))
                return indent + "- " + name + " = " + DumpDirectClass(value);

            if (IsCollectionType(valueType))
                return indent + "- " + name + " = " + DumpCollection(value, indentCount, indentString);

            return DumpObjectInstance(name, value, indentCount, indentString);
        }

        private static string DumpCollection(object value, int indentCount, string indentString)
        {
            if (value == null)
                return NullRepresentation;

            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            string dumpText;

            if (value.GetType().GetInterface("IDictionary") != null)
            {
                var dict = value as IDictionary;
                dumpText = "{" + dict.Count + " items}";
                foreach (var key in dict.Keys)
                {
                    dumpText += Environment.NewLine + InternalDump(key.ToString(), dict[key].GetType(), dict[key], indentCount + 1, indentString);
                }

                return dumpText;
            }

            if (value.GetType().GetInterface("ICollection") != null)
            {
                var col = value as ICollection;
                dumpText = "{" + col.Count + " items}";
                var enumerator = col.GetEnumerator();
                enumerator.Reset();
                for (var i = 0; i < col.Count; i++)
                {
                    enumerator.MoveNext();
                    var curValue = enumerator.Current;
                    dumpText += Environment.NewLine + InternalDump("[" + i + "]", curValue.GetType(), curValue, indentCount + 1, indentString);
                }

                return dumpText;
            }

            throw new NotImplementedException();
        }

        private static string DumpDirectClass(object value)
        {
            if (value == null)
                return NullRepresentation;

            if (value.GetType() == typeof(CultureInfo))
                // ReSharper disable PossibleNullReferenceException
                return (value as CultureInfo).Name;
            // ReSharper restore PossibleNullReferenceException

            throw new NotImplementedException();
        }

        private static string DumpValueType<T>(T value)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if ((typeof(T).IsClass || typeof(T).IsGenericType) && value == null)
                // ReSharper restore CompareNonConstrainedGenericWithNull
                return NullRepresentation;

            return value.ToString();
        }

        private static string DumpObjectInstance(string name, object value, int indentCount, string indentString)
        {
            var indent = string.Empty;
            for (var i = 0; i < indentCount; i++)
                indent += indentString;

            if (value == null)
                return indent + "+ " + name + " = " + NullRepresentation;

            string dumpText = indent + "+ " + name + " = " + value.GetType();
            foreach (var propertyInfo in value.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                    dumpText += Environment.NewLine + DumpProperty(propertyInfo, value, indentCount + 1, indentString);
            }

            return dumpText;
        }

        private static string DumpProperty(PropertyInfo property, object instance, int indentCount, string indentString)
        {
            var value = property.GetGetMethod().Invoke(instance, null);

            return InternalDump(property.Name, property.PropertyType, value, indentCount, indentString);
        }

        private static bool IsValueType(Type type)
        {
            return type == typeof(string) || type.IsValueType;
        }

        private static bool IsCollectionType(Type type)
        {
            return type.GetInterface("IDictionary") != null ||
                   type.GetInterface("ICollection") != null;
        }

        private static bool CanDirectDumpClass(Type type)
        {
            return type == typeof(CultureInfo);
        }

    }
}
