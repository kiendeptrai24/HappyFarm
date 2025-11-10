using System;
using UnityEngine;

// Plot that can host crops, animals or any placeable item implementing PlotPlaceableBase
public class Plot : MonoBehaviour, IFillOnAble
{
    private GameObject currentObj;
    private IPlaceable placeableItem;
    public Vector3 position => transform.position;

    public Action OnFillOnAnble { get; set; }
    public Action OnFillOnUnable { get; set; }

    private void Start() {
        OnFillOnAnble?.Invoke();
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
        placeableItem.OnPlaced(this);
    }
    public void OnEmpty()
    {
         if (!Isfilled())
        {
            Debug.Log("Plot is already empty.");
            return;
        }
        if(placeableItem != null)
            placeableItem.OnRemoved();
        currentObj = null;
        placeableItem = null;
        OnFillOnAnble?.Invoke();
    }
}
