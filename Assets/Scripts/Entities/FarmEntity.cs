

using UnityEngine;

public abstract class FarmEntity : MonoBehaviour, IEntity
{
    public EntityData data;
    private Dirt dirt;
    public float startTime;
    public float witherStartTime;
    [Header("Mesh Filter")]
    public Mesh matureTree;
    private MeshFilter meshFilter;
    private bool isMature = false;
    private bool startWither = false;
    protected virtual void Start()
    {
        startTime = Time.time;
        meshFilter = GetComponent<MeshFilter>();
        witherStartTime = float.MaxValue;
    }
    public void Plant(Dirt dirt)
    {
        this.dirt = dirt;
    }
    public EntityData Harvest()
    {
        if (IsHarvestable())
        {
            data.lifeCycles--;
            startTime = Time.time;
            startWither = false;
            witherStartTime = float.MaxValue;
            if (data.lifeCycles <= 0)
            {
                OnDead();
            }
            return data;
        }
        return null;
    }
    protected virtual void Update()
    {
        if (IsHarvestable() && startWither == false)
        {
            witherStartTime = Time.time;
            if (isMature == false)
            {
                isMature = true;
                if (meshFilter != null && meshFilter.mesh != matureTree)
                {
                    meshFilter.mesh = matureTree;
                }
            }
            startWither = true;
        }

        if (IsHarvestable() && startWither == true && data.witherDelay - Time.time - witherStartTime <= 0)
        {
            OnDead();
        }
    }


    public virtual bool IsHarvestable()
    {
        return Time.time - startTime >= data.timeToHarvest;
    }

    public void OnDead()
    {
        if (dirt != null)
            dirt.OnEmpty();
        Destroy(gameObject);
    }
    protected virtual void ResetTime()
    {
        startTime = Time.time;
    }
    public void ShowData()
    {
        float timeElapsed = Time.time - startTime;
        float timeRemaining = Mathf.Max(data.timeToHarvest - timeElapsed, 0f);

        Debug.Log(
            $"Crop: {data.name}, Life Cycles Left: {data.lifeCycles}, " +
            $"Time to Harvest Remaining: {timeRemaining:F1}s"
        );

    }
}