namespace MamiyaTool
{
    public interface IBTData
    {

    }

    public interface IBTData<T> : IBTData
    {
        T Data { get; set; }
    }
}