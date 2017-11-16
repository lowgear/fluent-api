using System;
using System.Globalization;
using System.Linq.Expressions;

namespace ObjectPrinting
{
    public class SerializeConfig<TOwner, TSerializble> : PrintingConfig<TOwner>, ISerializeConfig<TOwner, TSerializble>
    {
        private PrintingConfig<TOwner> printingConfig;
        public SerializeConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        } 

        public PrintingConfig<TOwner> Using(Func<TSerializble, string> fucc)
        {
            /*TODO: either add application of fucc to all fields
             or some specific field that was passed to this class through a constructor
              from printing method of PrintigConfig class*/
            return printingConfig;
        }

        PrintingConfig<TOwner> ISerializeConfig<TOwner, TSerializble>.PrintingConfig => printingConfig;
    }

    public interface ISerializeConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}