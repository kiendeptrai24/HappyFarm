

using System;
using UnityEngine;

public interface IEntity
{
    Action OnDead { get; set; }
    Action OnCanHasvest { get; set; }
    Action OnHavested { get; set; }
    void Plant(Dirt dirt);
    void SetPlantData(EntityData data);
    EntityData Harvest();
    bool IsHarvestable();
    void Died();
    void ShowData();
}