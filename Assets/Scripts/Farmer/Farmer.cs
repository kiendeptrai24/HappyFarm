

using System;
using UnityEngine;
using UnityEngine.AI;
public class Farmer : MonoBehaviour
{
    private FarmerMovement movement;
    [SerializeField] private Transform startPoint;
    private IFarmTaskBase curMission;
    [SerializeField] private float timeToCompletedMision;
    private float startTimeMission;
    private bool isIdle = true;
    public Action isIdleChanged { get; set; }

    private void Start()
    {
        movement = GetComponent<FarmerMovement>();
        FarmerManager.Instance.AddFarmer(this);
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
        startTimeMission = Time.time;
        isIdleChanged?.Invoke();
        curMission = mission;
        curMission.Start();
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
            if (Time.time - startTimeMission >= timeToCompletedMision)
            {
                curMission.Complete();
            }
        }
    }

}