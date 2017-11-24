using System;

namespace ObjectPrinting.Serialization
{
    public class PropertySerializeConfig<TOwner, TPropertyType> : ISerializeConfig<TOwner>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        PrintingConfig<TOwner> ISerializeConfig<TOwner>.PrintingConfig => printingConfig;
        internal readonly string PropertyPath;

        public PropertySerializeConfig(PrintingConfig<TOwner> printingConfig, string propertyPath)
        {
            this.printingConfig = printingConfig;
            PropertyPath = propertyPath;
        }

        public PrintingConfig<TOwner> Using(Func<TPropertyType, string> serializator)
        {
            printingConfig.PropertySerialization.Add(PropertyPath, serializator);
            return printingConfig;
        }
    }
}
