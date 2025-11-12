
using System;
using NUnit.Framework;
using UnityEngine;

public class Dirt : MonoBehaviour, IPlaceable, IFillOnAble
{
    [Header("Dirt Info")]
    public PlaceableType Type => PlaceableType.Dirt;
    public string Name => "Dirt";
    public Vector2 Size => new Vector2(1, 1);
    public Vector3 position => new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);

    public Action<GameObject> OnFillOnAnble { get; set; }
    public Action<GameObject> OnFillOnUnable { get; set; }

    public Plot plot;
    public DirtData dirtData;

    [Header("Crop Info")]
    public IEntity currentEntity;
    public GameObject cropObj;
    private void Start()
    {
        DetectTask.Instance.AddFillOn(this);
    }
    public void OnPlaced(Plot tile)
    {
        plot = tile;
        dirtData = plot.plotData.dirtData;
        OnFillOnAnble?.Invoke(cropObj);
    }
    public void OnRemoved() => Destroy(gameObject);
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
        if (transform.gameObject.scene.rootCount == 0)
        {
            Debug.Log(gameObject.name);
            return;
        }

        cropObj = Instantiate(source, position, Quaternion.identity, transform);
        entity = cropObj.GetComponent<IEntity>();
        entity.Plant(this);
        currentEntity = entity;
        OnFillOnUnable?.Invoke(cropObj);
        currentEntity.OnDead += OnEmpty;
    }
    public void OnEmpty()
    {
        if (currentEntity == null)
        {
            Debug.Log("No crop to remove.");
            return;
        }
        currentEntity = null;
        OnFillOnAnble?.Invoke(cropObj);
    }
    public bool Isfilled() => currentEntity != null;

}