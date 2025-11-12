using System;
using UnityEngine;

public class SowSeedTaskData
{
    public SowSeedTaskData(Dirt dirt, GameObject seed)
    {
        this.dirt = dirt;
        this.seed = seed;
    }
    public DateTime startTime = DateTime.Now;
    public Dirt dirt;
    public GameObject seed;
}
