using System.Collections.Generic;
using UnityEngine;


public class FarmManager : MonoBehaviour
{
    public GameObject plotPrefab;
    [SerializeField] private Transform point;
    public int rows = 1;
    public int columns = 3;
    public float plotSize = 1;

    [SerializeField] private List<GameObject> farmPlots = new();
    [ContextMenu("Initialize Farm Plots")]
    private void InitializeFarmPlots()
    {
        Vector3 startPosition = new Vector3(point.position.x, 0, point.position.z);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 plotPosition = startPosition + new Vector3(col * plotSize, 0, row * plotSize);
                var plot = Instantiate(plotPrefab, plotPosition, Quaternion.identity, point);
                farmPlots.Add(plot);
            }
        }
    }
    [ContextMenu("Clear Farm Plots")]
    private void ClearFarmPlots()
    {
        foreach (var plot in farmPlots)
        {
            DestroyImmediate(plot);
        }
        farmPlots.Clear();
    }
}
