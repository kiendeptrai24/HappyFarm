
using UnityEngine;
public interface IPlaceable
{
    PlaceableType Type { get; }
    string Name { get; }
    Vector2 Size { get; }
    void OnPlaced(Plot tile);
    void OnRemoved();
}
