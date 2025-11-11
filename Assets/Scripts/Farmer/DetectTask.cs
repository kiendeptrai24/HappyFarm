
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectTask : Singleton<DetectTask>
{
    public List<IFillOnAble> fillOns = new();
    public List<IFillOnAble> fillAbles = new();
    public List<IFillOnAble> fillUnables = new();
    public int fillOnCount;
    public int fillAbleCount;
    public int fillUnablesCount;
    public Action OnRefresh;
    private void Start()
    {
        fillOnCount = fillOns.Count;
        fillAbleCount = fillAbles.Count;
        fillUnablesCount = fillUnables.Count;
    }
    public void AddFillOn(IFillOnAble fillOn)
    {
        fillOn.OnFillOnAnble += () =>
        {
            Resfesh();
        };
        fillOn.OnFillOnUnable += () =>
        {
            Resfesh();
        };
        fillOns.Add(fillOn);
        Resfesh();
    }
    public void RemoveFillOn(IFillOnAble fillOn)
    {
        fillOns.Remove(fillOn);
    }
    public void Resfesh()
    {
        fillAbles.Clear();
        fillUnables.Clear();

        foreach (var fillOn in fillOns)
        {
            if (fillOn == null) continue;

            if (!fillOn.Isfilled())
            {
                Debug.Log("Dirt Is Empty");
                fillAbles.Add(fillOn);
            }
            else
            {
                Debug.Log("Dirt Is Filled");    
                fillUnables.Add(fillOn);
            }
        }
        fillOnCount = fillOns.Count;
        fillAbleCount = fillAbles.Count;
        fillUnablesCount = fillUnables.Count;
        OnRefresh?.Invoke();
    }
}