

using System;
using UnityEngine;

public interface IFarmTaskBase
{
    string NameTask { get; }
    Vector3 position { get; }
    Action OnStart { get; set; }
    Action OnComplete { get; set; }
    bool IsStarted { get; }
    bool IsCompleted { get; }

    void Start();
    void Update();
    void Complete(object result = null);
    string DisplayInfo();
}
