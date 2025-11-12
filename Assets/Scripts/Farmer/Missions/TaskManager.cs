
//using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private DetectTask detectTask;
    private  Queue<IFarmTaskBase> missions = new();
    private  List<IFarmTaskBase> inProgressMissions = new();
    private List<IFarmTaskBase> completedMissions = new();
    public List<GameObject> entities = new();
    public HashSet<IEntity> entitiesSet = new();
    public int inProgressMissionsCount;
    public int completedMissionCount;
    public System.Action<List<IFarmTaskBase>, List<IFarmTaskBase>> OnTaskChanged;
    private void Awake() {
        detectTask = GetComponent<DetectTask>();
        detectTask.OnRefresh += () =>
        {
            UpdateMission();
        };
        
    }
    public void UpdateMission()
    {
        foreach (var dirt in detectTask.fillAbles)
        {
            int random = Random.Range(0, entities.Count);
            GameObject seedEntity = entities[random];

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
                    if(inProgressMissions.Contains(sowTask))
                    {
                        inProgressMissions.Remove(sowTask);
                        completedMissions.Add(sowTask);
                    }
                }
            };
            AddTask(sowTask);
            OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
        }
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
            };
            entity.OnCanHasvest += () =>
            {
                AddTask(harvestTask);
                entity.OnCanHasvest -= () =>
                {
                    AddTask(harvestTask);
                    OnTaskChanged?.Invoke(inProgressMissions, completedMissions);
                };
            };
        }
        inProgressMissionsCount = inProgressMissions.Count;
        completedMissionCount = completedMissions.Count;
    }
    public void AddTask(IFarmTaskBase task)
    {
        missions.Enqueue(task);
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
