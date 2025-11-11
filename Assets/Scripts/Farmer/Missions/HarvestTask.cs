using System;
using UnityEngine;

public class HarvestTask : IFarmTask<HarvestTaskData>
{
    private HarvestTaskData data;
    public Vector3 position => data.position;
    public bool IsStarted { get; private set; }
    public bool IsCompleted { get; private set; }

    public Action OnStart { get; set; }
    public Action OnComplete { get; set; }

    public string NameTask => "HarvestTask";

    public void Setup(HarvestTaskData data)
    {
        this.data = data;
    }

    public void Start()
    {
        IsStarted = true;
        IsCompleted = false;
        OnStart?.Invoke();
        Debug.Log($"🌾 Bắt đầu thu hoạch {data.entityToHarvest} Task:{NameTask}");
    }

    public void Complete(object result = null)
    {
        data.entityToHarvest.Harvest();
        IsCompleted = true;
        OnComplete?.Invoke();
        Debug.Log($"✅ Đã thu hoạch xong {data.entityToHarvest}");
    }

    public void DisplayInfo()
    {
        Debug.Log($"HarvestTask → Started={IsStarted}, Completed={IsCompleted}");
    }

    public void Update()
    {
        if (data.entityToHarvest.IsHarvestable() == false && IsCompleted)
            Complete();
    }
}
