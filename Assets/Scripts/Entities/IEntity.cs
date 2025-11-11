

using System;
using UnityEngine;

public interface IEntity
{
    Action OnCanHasvest { get; set; }
    Action OnHavested { get; set; }
    void Plant(Dirt dirt);
    EntityData Harvest();
    bool IsHarvestable();
    void OnDead();
    void ShowData();
}