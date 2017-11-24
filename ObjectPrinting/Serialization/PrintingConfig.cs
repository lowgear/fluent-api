using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectPrinting.Serialization
{
    public class PrintingConfig<TOwner>
    {
        public PrintingConfig()
        {
            NumericCultures = new Dictionary<Type, CultureInfo>();
            excludedTypes = new HashSet<Type>();
            TypeSerialization = new Dictionary<Type, Delegate>();
            PropertySerialization = new Dictionary<string, Delegate>();
            excludedProperties = new HashSet<string>();
        }

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel, string path = "")
        {
            // ReSharper disable once InlineOutVariableDeclaration
            string result;
            if (TrySerializeNull(obj, out result))
                return result;

            if (TrySerializeCustomProperty(obj, path, out result))
                return result;

            if (TrySerializeCustomType(obj, out result))
                return result;

            if (TrySerializeNumericWithCulture(obj, out result))
                return result;

            if (TrySerializeFinalType(obj, out result))
                return result;

            return SerializeObject(obj, nestingLevel, path);
        }

        private string SerializeObject(object obj, int nestingLevel, string path)
        {
            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in SelectProperties(path, type))
            {
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintToString(propertyInfo.GetValue(obj),
                              nestingLevel + 1,
                              path + "." + propertyInfo.Name));
            }
            return sb.ToString();
        }

        private IEnumerable<PropertyInfo> SelectProperties(string path, Type type)
        {
            return type.GetProperties()
                .Where(prop => !excludedTypes.Contains(prop.PropertyType))
                .Where(prop => !excludedProperties.Contains(path + "." + prop.Name))
                .OrderBy(prop => prop.Name, StringComparer.Ordinal);
        }

        private static bool TrySerializeFinalType(object obj, out string result)
        {
            if (GenericIndependentStatics.FinalTypes.Contains(obj.GetType()))
            {
                result = obj + Environment.NewLine;
                return true;
            }
            result = null;
            return false;
        }

        private bool TrySerializeNumericWithCulture(object obj, out string result)
        {
            if (NumericCultures.TryGetValue(obj.GetType(), out var culture))
            {
                result = SerializeNumericWithCulturte(obj, culture) + Environment.NewLine;
                return true;
            }
            result = null;
            return false;
        }

        private bool TrySerializeCustomType(object obj, out string result)
        {
            if (TypeSerialization.TryGetValue(obj.GetType(), out var typePrinter))
            {
                result = (string) typePrinter.DynamicInvoke(obj) + Environment.NewLine;
                return true;
            }
            result = null;
            return false;
        }

        private bool TrySerializeCustomProperty(object obj, string path, out string result)
        {
            if (PropertySerialization.TryGetValue(path, out var propertyPrinter))
            {
                result = (string) propertyPrinter.DynamicInvoke(obj) + Environment.NewLine;
                return true;
            }
            result = null;
            return false;
        }

        private static bool TrySerializeNull(object obj, out string result)
        {
            if (obj == null)
            {
                result = "null" + Environment.NewLine;
                return true;
            }
            result = null;
            return false;
        }

        private static string SerializeNumericWithCulturte(object o, IFormatProvider culture)
        {
            switch (o)
            {
                case int i:
                    return i.ToString(culture);
                case float f:
                    return f.ToString(culture);
                case double d:
                    return d.ToString(culture);
                case long l:
                    return l.ToString(culture);
                default:
                    throw new ArgumentException();
            }
        }

        public TypeSerializeConfig<TOwner, TType> Printing<TType>()
        {
            return new TypeSerializeConfig<TOwner, TType>(this);
        }

        public PropertySerializeConfig<TOwner, TPropType> Printing<TPropType>(
            Expression<Func<TOwner, TPropType>> propertySelector)
        {
            return new PropertySerializeConfig<TOwner, TPropType>(this,
                GetPath(propertySelector));
        }

        private static string RemoveContext(string propertyPath)
        {
            return propertyPath.Substring(propertyPath.IndexOf(".", StringComparison.Ordinal));
        }

        private readonly HashSet<Type> excludedTypes;
        private readonly HashSet<string> excludedProperties;
        internal readonly Dictionary<Type, Delegate> TypeSerialization;
        internal readonly Dictionary<Type, CultureInfo> NumericCultures;
        internal readonly Dictionary<string, Delegate> PropertySerialization;

        public PrintingConfig<TOwner> ExcludeType<TType>()
        {
            excludedTypes.Add(typeof(TType));
            return this;
        }

        public PrintingConfig<TOwner> ExcludeProperty<TPropertyType>(
            Expression<Func<TOwner, TPropertyType>> propertySelector)
        {
            excludedProperties.Add(GetPath(propertySelector));
            return this;
        }

        private static string GetPath<TPropertyType>(Expression<Func<TOwner, TPropertyType>> propertySelector)
        {
            return RemoveContext(propertySelector.Body.ToString());
        }
    }

    internal static class GenericIndependentStatics
    {
        public static readonly IReadOnlyCollection<Type> FinalTypes = new[]
        {
            typeof(int), typeof(double), typeof(float), typeof(string),
            typeof(DateTime), typeof(TimeSpan)
        };
    }
}
