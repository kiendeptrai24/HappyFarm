

public interface IFarmTask<TData> : IFarmTaskBase
{
    void Setup(TData data);
}
