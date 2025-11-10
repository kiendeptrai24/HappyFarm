

public interface IEntity
{
    void Plant(Dirt dirt);
    EntityData Harvest();
    bool IsHarvestable();
    void OnDead();
    void ShowData();
}