using System;

namespace ObjectPrinting.Serialization
{
    public class TypeSerializeConfig<TOwner, TSerializble> : ISerializeConfig<TOwner>
    {
        internal readonly PrintingConfig<TOwner> PrintingConfig;
        PrintingConfig<TOwner> ISerializeConfig<TOwner>.PrintingConfig => PrintingConfig;

        public TypeSerializeConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.PrintingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> Using(Func<TSerializble, string> fucc)
        {
            PrintingConfig.TypeSerialization.Add(typeof(TSerializble), fucc);
            return PrintingConfig;
        }
    }
}