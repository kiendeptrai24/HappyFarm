

using System;
using UnityEngine;
using UnityEngine.AI;
public class Farmer : MonoBehaviour
{
    private FarmerMovement movement;

    private IFarmTaskBase curMission;
    [SerializeField] private float timeToCompletedMision;
    private float startTimeMission;
    private bool isIdle = true;
    public Action isIdleChanged { get; set; }

    private void Start()
    {
        movement = GetComponent<FarmerMovement>();

        FarmerManager.Instance.AddFarmer(this);
    }
    public bool IsIdle() => isIdle;
    public void SetTask(IFarmTaskBase mission)
    {
        movement?.MoveTo(mission.position);
        isIdle = false;
        startTimeMission = Time.time;
        isIdleChanged?.Invoke();
        curMission = mission;
        curMission.Start();
        curMission.OnComplete += () =>
        {
            Unsubscribe();
            curMission = null;
            isIdle = true;
            isIdleChanged?.Invoke();
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
                curMission = null;
            }
        }
    }
    private void Unsubscribe()
    {
        if (curMission == null) return;
        curMission.OnComplete -= () =>
        {
            curMission = null;
        };
    }

}