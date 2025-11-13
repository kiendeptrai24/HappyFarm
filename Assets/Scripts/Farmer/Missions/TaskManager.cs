
//using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private DetectTask detectTask;
    public float timeToUpdateTask = 3f;
    private float timeElapsed = 0f;

    private Inventory inventory;
    private Queue<IFarmTaskBase> missions = new();
    private List<IFarmTaskBase> inProgressMissions = new();
    private List<IFarmTaskBase> completedMissions = new();
    public List<GameObject> entities = new();
    public HashSet<IEntity> entitiesSet = new();
    public HashSet<Dirt> sowSeedSet = new();
    public int inProgressMissionsCount;
    public int completedMissionCount;
    public int taskInQueue;
    public System.Action<List<IFarmTaskBase>, List<IFarmTaskBase>> OnTaskChanged;
    private void Awake()
    {

        detectTask = GetComponent<DetectTask>();
        inventory = FindAnyObjectByType<Inventory>();
        detectTask.OnRefresh += (_,_) =>
        {
            UpdateMission();
        };

    }
    private void Update()
    {

        if (Time.time - timeElapsed > timeToUpdateTask)
            UpdateMission();
    }
    public void UpdateMission()
    {
        Debug.Log("UpdateMission");
        SeedTask();
        HavestTask();
        // update mission count
        inProgressMissionsCount = inProgressMissions.Count;
        completedMissionCount = completedMissions.Count;
        taskInQueue = missions.Count;
        timeElapsed = Time.time;
    }

    private void HavestTask()
    {
        foreach (var dirt in detectTask.fillUnables)
        {
            if (dirt == null) continue;

            Dirt place = dirt as Dirt;
            if (place == null) continue;

            IEntity entity = place.currentEntity;
            if (entity == null) continue;

            // tránh trùng entity
            if (entitiesSet.Contains(entity)) continue;
            entitiesSet.Add(entity);

            // Tạo task mới
            var harvestTask = new HarvestTask();
            harvestTask.Setup(new HarvestTaskData(entity, place.position));

            // -------------------------------
            // Callback: Khi ô đất có thể fill lại
            System.Action<GameObject> onFillAble = null;
            onFillAble = (_) =>
            {
                ClearEntityWhenDies(harvestTask, entity);
                dirt.OnFillOnAnble -= onFillAble; // gỡ ngay sau khi chạy
            };
            dirt.OnFillOnAnble += onFillAble;

            // Callback: Khi entity có thể harvest
            System.Action onCanHarvest = null;
            onCanHarvest = () =>
            {
                if (entity == null) return;

                AddTask(harvestTask);
                OnTaskChanged?.Invoke(inProgressMissions, completedMissions);

                entity.OnCanHasvest -= onCanHarvest; // gỡ sau khi chạy
            };
            entity.OnCanHasvest += onCanHarvest;

            // Callback: Khi entity đã harvest
            System.Action onHarvested = null;
            onHarvested = () =>
            {
                //ClearEntityWhenDies(harvestTask, entity);
                entity.OnHavested -= onHarvested; // gỡ sau khi chạy
            };
            entity.OnHavested += onHarvested;

            // Callback: Khi task hoàn thành
            System.Action onComplete = null;
            onComplete = () =>
            {
                harvestTask.OnComplete -= onComplete; // gỡ sau khi chạy
                ClearEntityWhenDies(harvestTask, entity);
            };
            harvestTask.OnComplete += onComplete;
        }
    }

    // -------------------------------
    // Xử lý dọn task và entity an toàn
    private void ClearEntityWhenDies(HarvestTask harvestTask, IEntity entity)
    {
        if (harvestTask == null || entity == null) return;
        Debug.Log("ClearEntityWhenDies");
        // Dọn missions
        if (missions != null && missions.Contains(harvestTask))
        {
            missions = new Queue<IFarmTaskBase>(missions.Where(m => m != harvestTask));
        }
        // Dọn inProgress
        if (inProgressMissions != null && inProgressMissions.Contains(harvestTask))
        {
            inProgressMissions.Remove(harvestTask);
            completedMissions ??= new List<IFarmTaskBase>();
            completedMissions.Add(harvestTask);
        }

        // Dọn entities
        entitiesSet?.Remove(entity);

        // Dọn null trong các list lâu dài
        inProgressMissions?.RemoveAll(t => t == null);
        completedMissions?.RemoveAll(t => t == null);
        entitiesSet?.RemoveWhere(e => e == null);

        // Thông báo thay đổi farm
        OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
    }



    private void SeedTask()
    {
        foreach (var dirt in detectTask.fillAbles)
        {
            if (sowSeedSet.Contains(dirt as Dirt))
                continue;
            var seedEntity = inventory.GetRandomSeed();
            if (seedEntity == null) return;
            var sowTask = new SowSeedTask();
            sowTask.Setup(new SowSeedTaskData(dirt as Dirt, seedEntity));

            dirt.OnFillOnUnable += (_) =>
            {
                if (missions.Contains(sowTask))
                {
                    var newQueue = new Queue<IFarmTaskBase>();

                    foreach (var m in missions)
                        if (m != sowTask)
                            newQueue.Enqueue(m);

                    missions = newQueue;
                }
                else if (inProgressMissions.Contains(sowTask))
                {
                    if (inProgressMissions.Contains(sowTask))
                    {
                        inProgressMissions.Remove(sowTask);
                        completedMissions.Add(sowTask);
                    }
                }
            };
            sowTask.OnComplete += () =>
            {
                sowSeedSet.Remove(dirt as Dirt);
            };
            AddTask(sowTask);
            sowSeedSet.Add(dirt as Dirt);
        }
        OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
    }

    public void AddTask(IFarmTaskBase task)
    {
        missions.Enqueue(task);
        OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
    }

    public IFarmTaskBase GetMission()
    {
        if (missions.Count == 0) return null;
        var mission = missions.Peek();
        if (mission == null) return null;

        inProgressMissions.Add(missions.Dequeue());
        // mission.OnComplete += () =>
        // {
        //     inProgressMissions.Remove(mission);
        //     completedMissions.Add(mission);
        // };

        return mission;
    }

    public void ShowAllMissions()
    {
        foreach (var mission in missions)
        {
            mission.DisplayInfo();
        }
    }
    public void ShowInProgressMissions()
    {
        foreach (var mission in inProgressMissions)
        {
            mission.DisplayInfo();
        }
    }
    public void ShowCompletedMissions()
    {
        foreach (var mission in completedMissions)
        {
            mission.DisplayInfo();
        }
    }
}
