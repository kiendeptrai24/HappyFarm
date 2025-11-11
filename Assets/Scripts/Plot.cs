using System;
using System.Collections.Generic;
using UnityEngine;

// Plot that can host crops, animals or any placeable item implementing PlotPlaceableBase
[Serializable]
public class Plot : MonoBehaviour, IFillOnAble
{
    public PlotData plotData;
    public GameObject currentObj;
    private IPlaceable placeableItem;
    public Vector3 position => transform.position;
    public Action OnFillOnAnble { get; set; }
    public Action<GameObject> OnFillOnUnable { get; set; }

    private void Start()
    {
        OnFillOnAnble?.Invoke();
    }
    public void Init(int row, int col, bool hasCrop, DirtData dirtData = null)
    {
        plotData = new PlotData(row, col, hasCrop, dirtData);
    }
    public bool Isfilled() => currentObj != null && placeableItem != null;
    public void OnFill(GameObject source)
    {
        if (Isfilled())
        {
            Debug.Log("Plot is already filled.");
            return;
        }
        placeableItem = source.GetComponent<IPlaceable>();
        if (placeableItem == null)
        {
            Debug.Log("Object does not implement IPlaceable interface.");
            return;
        }
        currentObj = Instantiate(source, transform.position, Quaternion.identity, transform);

        var fillOnDetect = currentObj.GetComponent<IFillOnAble>();
        fillOnDetect.OnFillOnUnable += (source) =>
        {
            var entity = source.GetComponent<FarmEntity>();
            if (entity != null)
            {
                Debug.Log(entity.data.name);
                plotData.dirtData.hasEntity = true;
                plotData.dirtData.nameOfEntiy = entity.data.name;
            }
            else
            {
                Debug.Log(entity.data.name);
                plotData.dirtData.hasEntity = false;
                plotData.dirtData.nameOfEntiy = null;
            }

        };

        placeableItem.OnPlaced(this);
        OnFillOnUnable?.Invoke(currentObj);
        plotData.hasCrop = true;
    }
    public void OnEmpty()
    {
        if (!Isfilled())
        {
            Debug.Log("Plot is already empty.");
            return;
        }
        if (placeableItem != null)
            placeableItem.OnRemoved();
        currentObj = null;
        placeableItem = null;
        plotData.hasCrop = false;
        OnFillOnAnble?.Invoke();
    }
}
