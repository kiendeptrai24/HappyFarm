


using UnityEngine;

public class HarvestTaskData
{
    public HarvestTaskData(IEntity entityToHarvest,Vector3 position)
    {
        this.entityToHarvest = entityToHarvest;
        this.position = position;
    }
    public IEntity entityToHarvest;
    public Vector3 position;
}
