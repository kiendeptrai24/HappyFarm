
using UnityEngine;

public class Crop : MonoBehaviour, IPlaceable
{
    public PlaceableType Type => PlaceableType.Crop;

    public string Name => "Crop";

    public Vector2 Size => new Vector2(1, 1);

    public Plot plot;

    public void OnPlaced(Plot tile)
    {
        plot = tile;
    }

    public void OnRemoved()
    {
        plot = null;
    }
}