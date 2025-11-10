
using NUnit.Framework;
using UnityEngine;

public class Dirt : MonoBehaviour, IPlaceable, IFillOnAble
{
    [Header("Dirt Info")]
    public PlaceableType Type => PlaceableType.Dirt;
    public string Name => "Dirt";
    public Vector2 Size => new Vector2(1, 1);
    public Vector3 position => new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
    public Plot plot;

    [Header("Crop Info")]
    public IEntity currentEntity;
    public GameObject cropObj;
    public void OnPlaced(Plot tile)
    {
        plot = tile;
    }
    public void OnRemoved()
    {
        plot = null;
    }

    public void OnFill(GameObject source)
    {
        if (Isfilled())
        {
            Debug.Log("Dirt already has a crop planted.");
            return;
        }
        IEntity entity = source.GetComponent<IEntity>();
        if (entity == null)
        {
            Debug.Log("Object does not implement ICrop interface.");
            return;
        }
        cropObj = Instantiate(source, position, Quaternion.identity, transform);
        entity = cropObj.GetComponent<IEntity>();
        entity.Plant(this);
        currentEntity = entity;
    }

    public void OnEmpty()
    {
        if (currentEntity == null)
        {
            Debug.Log("No crop to remove.");
            return;
        }
        currentEntity = null;
    }

    public bool Isfilled() => currentEntity != null;
    
}