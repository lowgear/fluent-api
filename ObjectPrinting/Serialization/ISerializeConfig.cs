namespace ObjectPrinting.Serialization
{
    public interface ISerializeConfig<TOwner>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}