

using System;
using UnityEngine;

public abstract class FarmEntity : MonoBehaviour, IEntity
{
    private Inventory inventory;
    public EntityData data;
    private Dirt dirt;
    public float startTime;
    public float witherStartTime;
    [Header("Mesh Filter")]
    public Mesh matureTree;
    public MeshFilter meshFilter;
    private bool startWither = false;
    public Action OnCanHasvest { get; set; }
    public Action OnHavested { get; set; }
    public Action OnDead { get; set; }

    public bool isDead = false;

    protected virtual void Start()
    {
        startTime = Time.time;
        inventory = FindAnyObjectByType<Inventory>();
        witherStartTime = float.MaxValue;
        data.ApplyUpgrade(inventory.farmUpgradeData);
    }
    public void Plant(Dirt dirt)
    {
        this.dirt = dirt;
        this.dirt.dirtData.entityData = data;
    }
    public EntityData Harvest()
    {
        if (IsHarvestable())
        {
            data.lifeCycles--;
            startTime = Time.time;
            startWither = false;
            witherStartTime = float.MaxValue;
            inventory.AddFarmProduct(data);
            OnHavested?.Invoke();
            if (data.lifeCycles <= 0)
            {
                Died();
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
            if (data.isMature == false)
                TreeIsMatrue();
            startWither = true;
            OnCanHasvest?.Invoke();
        }

        if (IsHarvestable() && startWither == true && Time.time - witherStartTime > data.witherDelay)
        {
            Died();
        }
    }
    private void TreeIsMatrue()
    {
        Debug.Log("Tree is mature");
        data.isMature = true;
        if (meshFilter != null && meshFilter.mesh != matureTree)
        {
            meshFilter.mesh = matureTree;
        }

    }


    public virtual bool IsHarvestable()
    {
        if (data == null) return false;
        return Time.time - startTime >= data.timeToHarvest;
    }

    public void Died()
    {
        if (isDead) return;
        isDead = true;

        OnDead?.Invoke();

        data = null;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
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

    public void SetPlantData(EntityData data)
    {
        this.data = data;
        this.dirt.dirtData.entityData = this.data;
        if (data.isMature)
            TreeIsMatrue();
    }
}