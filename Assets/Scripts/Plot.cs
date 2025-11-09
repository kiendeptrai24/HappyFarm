using UnityEngine;

// Plot that can host crops, animals or any placeable item implementing PlotPlaceableBase
public class Plot : MonoBehaviour
{
    private bool isFilled = false;
    private GameObject currentObj;
    private IPlaceable placeableItem;
    public void FillPlot(GameObject obj)
    {
        if (isFilled)
        {
            Debug.Log("Plot is already filled.");
            return;
        }
        placeableItem = obj.GetComponent<IPlaceable>();
        if (placeableItem == null)
        {
            Debug.Log("Object does not implement IPlaceable interface.");
            return;
        }
        currentObj = Instantiate(obj, transform.position, Quaternion.identity, transform);
        placeableItem.OnPlaced(this);
        isFilled = true;
    }
    public bool Isfilled() => isFilled;
    public void ClearPlot()
    {
        if (!isFilled)
        {
            Debug.Log("Plot is already empty.");
            return;
        }
        if(placeableItem != null)
            placeableItem.OnRemoved();
        currentObj = null;
        placeableItem = null;
        isFilled = false;
    } 

}
