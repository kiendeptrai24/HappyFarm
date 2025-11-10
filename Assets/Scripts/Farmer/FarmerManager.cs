

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FarmerManager : Singleton<FarmerManager>
{
    private TaskManager taskManager;
    [SerializeField] private List<Farmer> farmers = new();
    [SerializeField] private List<Farmer> idlefarmers = new();
    [SerializeField] private List<Farmer> workingfarmers = new();
    [SerializeField] private float timeToNextTask = 1;
    private float startTimeNextTask;
    private void Awake() {
        taskManager = FindAnyObjectByType<TaskManager>();
    }
    public void AddFarmer(Farmer farmer) {
        farmer.isIdleChanged += () =>
        {
            RefeshFarmer();
        };
        farmers.Add(farmer);
        RefeshFarmer();
    }
    public void RemoveFarmer(Farmer farmer)
    {
        farmers.Remove(farmer);
    }
    private void Update() {
        if (timeToNextTask <= Time.time - startTimeNextTask)
        {
            AssignTaskToFarmer();
            startTimeNextTask = Time.time;
        }
        
    }
    public void AssignTaskToFarmer()
    {
        var task = taskManager.GetMission();

        if (task == null || idlefarmers.Count == 0) return;

        int random = Random.Range(0, idlefarmers.Count);
        idlefarmers[random].SetTask(task);
    }
    public void RefeshFarmer()
    {
        idlefarmers.Clear();
        workingfarmers.Clear();
        foreach (var farmer in farmers)
        {
            if (farmer.IsIdle())
                idlefarmers.Add(farmer);
            else
                workingfarmers.Add(farmer);
        }
    }
}