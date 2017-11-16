using System.Globalization;

namespace ObjectPrinting
{
    public static class Extnsions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(this SerializeConfig<TOwner, double> config,
            CultureInfo culture)
        {
            return ((ISerializeConfig<TOwner, double>) config).PrintingConfig;
        }
        public static PrintingConfig<TOwner> Using<TOwner>(this SerializeConfig<TOwner, int> config,
            CultureInfo culture)
        {
            return ((ISerializeConfig<TOwner, int>) config).PrintingConfig;
        }
        public static PrintingConfig<TOwner> Using<TOwner>(this SerializeConfig<TOwner, float> config,
            CultureInfo culture)
        {
            return ((ISerializeConfig<TOwner, float>) config).PrintingConfig;
        }
        public static PrintingConfig<TOwner> Using<TOwner>(this SerializeConfig<TOwner, long> config,
            CultureInfo culture)
        {
            return ((ISerializeConfig<TOwner, long>) config).PrintingConfig;
        }
    }
}