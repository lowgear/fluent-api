using System;
using System.Globalization;

namespace ObjectPrinting.Serialization
{
    public static class SerializationExtensions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(this TypeSerializeConfig<TOwner, double> config,
            CultureInfo culture)
        {
            config.PrintingConfig.NumericCultures[typeof(double)] = culture;
            return ((ISerializeConfig<TOwner>) config).PrintingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this TypeSerializeConfig<TOwner, int> config,
            CultureInfo culture)
        {
            config.PrintingConfig.NumericCultures[typeof(int)] = culture;
            return ((ISerializeConfig<TOwner>) config).PrintingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this TypeSerializeConfig<TOwner, float> config,
            CultureInfo culture)
        {
            config.PrintingConfig.NumericCultures[typeof(float)] = culture;
            return ((ISerializeConfig<TOwner>) config).PrintingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this TypeSerializeConfig<TOwner, long> config,
            CultureInfo culture)
        {
            config.PrintingConfig.NumericCultures[typeof(long)] = culture;
            return ((ISerializeConfig<TOwner>) config).PrintingConfig;
        }

        public static PrintingConfig<TOwner> Take<TOwner>(this PropertySerializeConfig<TOwner, string> config,
            int length)
        {
            string Cut(string s) => s.Substring(0, length);
            ((ISerializeConfig<TOwner>) config).PrintingConfig.PropertySerialization[config.PropertyPath] =
                (Func<string, string>) Cut;
            return ((ISerializeConfig<TOwner>)config).PrintingConfig;
        }

        public static string Serialize<TType>(this TType obj, Func<PrintingConfig<TType>, PrintingConfig<TType>> configuer = null)
        {
            var printer = ObjectPrinter.For<TType>();
            return (configuer != null ? configuer(printer) : printer).PrintToString(obj);
        }
    }
}