using System;
using UnityEngine;

public class SowSeedTask : IFarmTask<SowSeedTaskData>
{
    private SowSeedTaskData data;

    public bool IsStarted { get; private set; }
    public bool IsCompleted { get; private set; }

    public Action OnStart { get; set; }
    public Action OnComplete { get; set; }

    public Vector3 position => data.dirt.position;

    public string NameTask => "SowSeedTask";

    public void Setup(SowSeedTaskData data)
    {
        this.data = data;
        if (data.seed == null || data.dirt == null)
            throw new ArgumentException("SowSeedTaskData thiếu Seed hoặc Ground.");
    }

    public void Start()
    {
        IsStarted = true;
        IsCompleted = false;
        OnStart?.Invoke();
    }
    
    public void Complete(object result = null)
    {
        data.dirt.OnFill(data.seed);
        IsCompleted = true;
        OnComplete?.Invoke();
    }

    public string DisplayInfo()
    {
        return $"Task {NameTask} was created at {data.startTime:HH:mm}: a free plot is ready, go plant your crop!";
    }

    public void Update()
    {
        if (data.dirt.Isfilled() && !IsCompleted)
            Complete();
    }
}
