
//using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private DetectTask detectTask;
    public float timeToUpdateTask = 3f;
    private float timeElapsed = 0f;

    private Inventory inventory;
    private  Queue<IFarmTaskBase> missions = new();
    private  List<IFarmTaskBase> inProgressMissions = new();
    private List<IFarmTaskBase> completedMissions = new();
    public List<GameObject> entities = new();
    public HashSet<IEntity> entitiesSet = new();
    public int inProgressMissionsCount;
    public int completedMissionCount;
    public int taskInQueue;
    public System.Action<List<IFarmTaskBase>, List<IFarmTaskBase>> OnTaskChanged;
    private void Awake()
    {

        detectTask = GetComponent<DetectTask>();
        inventory = FindAnyObjectByType<Inventory>();
        detectTask.OnRefresh += () =>
        {
            UpdateMission();
        };

    }
    private void Update() {
        
        if (Time.time - timeElapsed > timeToUpdateTask)
            UpdateMission();
    }
    public void UpdateMission()
    {
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
            Dirt place = dirt as Dirt;
            IEntity entity = place.currentEntity;
            if (entitiesSet.Contains(entity)) continue;
            entitiesSet.Add(entity);

            var harvestTask = new HarvestTask();
            harvestTask.Setup(new HarvestTaskData(entity, place.position));

            dirt.OnFillOnAnble += (_) =>
            {
                ClearEntityWhenDies(harvestTask);
                dirt.OnFillOnAnble -= (_) =>
                {
                    ClearEntityWhenDies(harvestTask);
                };

            };
            entity.OnCanHasvest += () =>
            {
                Debug.Log($"Create Harvest Task with entity" );
                AddTask(harvestTask);
                OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
            };
            entity.OnHavested += () =>
            {
                if (missions.Contains(harvestTask))
                {
                    var newQueue = new Queue<IFarmTaskBase>();

                    foreach (var m in missions)
                        if (m != harvestTask)
                            newQueue.Enqueue(m);

                    missions = newQueue;
                }
                else if (inProgressMissions.Contains(harvestTask))
                {

                    harvestTask.Complete(false);
                    inProgressMissions.Remove(harvestTask);
                    completedMissions.Add(harvestTask);
                    
                }
            };

        }
    }

    private void ClearEntityWhenDies(HarvestTask harvestTask)
    {
        if (missions.Contains(harvestTask))
        {
            var newQueue = new Queue<IFarmTaskBase>();

            foreach (var m in missions)
                if (m != harvestTask)
                    newQueue.Enqueue(m);

            missions = newQueue;
        }
        else if (inProgressMissions.Contains(harvestTask))
        {
            if (inProgressMissions.Contains(harvestTask))
            {
                inProgressMissions.Remove(harvestTask);
                completedMissions.Add(harvestTask);
            }
        }
    }

    private void SeedTask()
    {
        foreach (var dirt in detectTask.fillAbles)
        {
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
            Debug.Log($"Create sow Task with entity" );
            AddTask(sowTask);
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
        mission.OnComplete += () =>
        {
            inProgressMissions.Remove(mission);
            completedMissions.Add(mission);
        };
        
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
