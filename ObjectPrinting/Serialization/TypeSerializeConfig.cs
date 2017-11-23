using System;

namespace ObjectPrinting.Serialization
{
    public class TypeSerializeConfig<TOwner, TSerializble> : ISerializeConfig<TOwner>
    {
        internal readonly PrintingConfig<TOwner> PrintingConfig;
        PrintingConfig<TOwner> ISerializeConfig<TOwner>.PrintingConfig => PrintingConfig;

        public TypeSerializeConfig(PrintingConfig<TOwner> printingConfig)
        {
            PrintingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> Using(Func<TSerializble, string> serializator)
        {
            PrintingConfig.TypeSerialization.Add(typeof(TSerializble), serializator);
            return PrintingConfig;
        }
    }
}