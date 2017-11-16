using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner> 
    {
        public PrintingConfig()
        {
            this.excludedTypes = new HashSet<Type>();
        }

        private PrintingConfig<TOwner> AddCulture(Type type, CultureInfo culture)
        {
            return this;
        }

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintToString(propertyInfo.GetValue(obj),
                              nestingLevel + 1));
            }
            return sb.ToString();
        }

        public SerializeConfig<TOwner, TType> Printing<TType>()
        {
            return new SerializeConfig<TOwner, TType>(this);
        }

        public SerializeConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> propertySelector)
        {
            return new SerializeConfig<TOwner, TPropType>(this);
        }

        private readonly HashSet<Type> excludedTypes;
        
        public PrintingConfig<TOwner> ExcludeType<TType>()
        {
            excludedTypes.Add(typeof(TType));
            return this;
        }

    }

}