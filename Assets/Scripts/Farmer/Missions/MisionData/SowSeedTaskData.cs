using UnityEngine;

public class SowSeedTaskData
{
    public SowSeedTaskData(Dirt dirt, GameObject seed)
    {
        this.dirt = dirt;
        this.seed = seed;
    }
    public Dirt dirt;
    public GameObject seed;
}
