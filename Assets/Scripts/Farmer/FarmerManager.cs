

using System.Collections.Generic;
using UnityEngine;

public class FarmerManager : Singleton<FarmerManager> , ISaveLoadData
{
    private TaskManager taskManager;
    [SerializeField] private Farmer farmerPrefab;
    [SerializeField] private Transform point;
    [SerializeField] private List<Farmer> farmers = new();
    [SerializeField] private List<Farmer> idlefarmers = new();
    [SerializeField] private List<Farmer> workingfarmers = new();
    [SerializeField] private float timeToNextTask = 1;
    private float startTimeNextTask;
    public System.Action<int, int> OnFarmerChanged;
    private void Awake() {
        taskManager = FindAnyObjectByType<TaskManager>();
        SaveLoadManager.Instance.RegisterSaveLoadData(this);
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
        OnFarmerChanged?.Invoke(idlefarmers.Count, workingfarmers.Count);
    }

    public void Save(GameData gameData)
    {
        gameData.farmAmount = farmers.Count;
    }

    public void Load(GameData gameData)
    {
        try
        {
            for (int i = 0; i < gameData.farmAmount; i++)
            {
                var farmer = Instantiate(farmerPrefab, point.position, Quaternion.identity);
                farmer.SetUp(point);
            }
            
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error Instantiate Farmer:" + ex.Message);
        }
    }
}