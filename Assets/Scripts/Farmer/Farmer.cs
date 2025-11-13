

using System;
using UnityEngine;
using UnityEngine.AI;
public class Farmer : MonoBehaviour, IDragItemInteract
{
    private FarmerMovement movement;
    [SerializeField] private Transform startPoint;
    private IFarmTaskBase curMission;
    [SerializeField] public float timeToCompletedMision;
    public float startTimeTask;
    private bool isIdle = true;
    public Action isIdleChanged { get; set; }
    public ShopItemType Type { get; set; } = ShopItemType.Farmer;
    public bool CanSpawnAnyWhere { get; set; } = true;
    private FarmerManager farmerManager;
    private void Awake()
    {
        farmerManager = FarmerManager.Instance;
        movement = GetComponent<FarmerMovement>();
        farmerManager.AddFarmer(this);
        farmerManager.SetUpPoint(this);
    }
    private void Start()
    {
        if (startPoint != null)
            return;
        movement.MoveTo(startPoint.position);
    }
    public void SetUp(Transform startPoint)
    {
        this.startPoint = startPoint;
    }
    public bool IsIdle() => isIdle;
    public void SetTask(IFarmTaskBase mission)
    {
        movement.MoveTo(mission.position);
        isIdle = false;
        startTimeTask = Time.time;
        isIdleChanged?.Invoke();
        curMission = mission;
        curMission.Start();
        if (curMission == null)
        {
            Debug.Log("--------------------------------");
        }
        curMission.OnComplete += () =>
        {
            curMission = null;
            isIdle = true;
            isIdleChanged?.Invoke();
            movement.MoveTo(startPoint.position);
        };
    }
    private void Update()
    {
        if (curMission != null && curMission.IsCompleted == false)
        {
            curMission.Update();
            if (Time.time - startTimeTask >= timeToCompletedMision)
            {
                curMission.Complete();
            }
        }
    }

}