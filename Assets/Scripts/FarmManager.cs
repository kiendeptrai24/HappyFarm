using System;
using System.Collections.Generic;
using UnityEngine;


public class FarmManager : MonoBehaviour, ISaveLoadData
{
    public PlotData[,] plotDatas = new PlotData[10, 10];
    public Plot plotPrefab;
    [SerializeField] private Transform point;
    [SerializeField] private GameObject dirtPrefab;

    public int rows = 1;
    public int columns = 3;
    public float plotSize = 1;

    [Header("Farm Plots")]
    [SerializeField] private Plot[,] farmPlots = new Plot[10, 10];
    [SerializeField] private List<Plot> plots = new();
    [SerializeField] private List<PlotData> plotsToSave = new();
    [SerializeField] private List<GameObject> entites;
    [SerializeField] private List<FarmEntity> baseFarmEntities;

    [Header("Dirt info")]
    [SerializeField] public List<IFillOnAble> landsHaveUse = new();
    [SerializeField] public List<IFillOnAble> vacantplots = new();

    [Header("Planted info")]
    [SerializeField] public List<FarmEntity> plantedEntities = new();

    public Action<List<IFillOnAble>, List<IFillOnAble>> OnFarmChanged;

    private void Awake()
    {
        SaveLoadManager.Instance.RegisterSaveLoadData(this);
        InitializeFarmPlots();
    }
    private void Start()
    {

    }
    [ContextMenu("Initialize Farm Plots")]
    private void InitializeFarmPlots()
    {
        ClearFarmPlots();
        Vector3 startPosition = new Vector3(point.position.x, 0, point.position.z);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 plotPosition = startPosition + new Vector3(col * plotSize, 0, row * plotSize);
                var plot = Instantiate(plotPrefab, plotPosition, Quaternion.identity, point);

                plot.Init(row, col, false);
                plots.Add(plot);

                farmPlots[row, col] = plot;
                plotDatas[row, col] = plot.plotData;

                ApplyPlotCallback(row, col, plot);
            }
        }
        OnFarmChanged?.Invoke(vacantplots, landsHaveUse);
    }

    private void ApplyPlotCallback(int row, int col, Plot plot)
    {
        farmPlots[row, col].OnFillOnUnable += (land) =>
        {
            plotsToSave.Add(plot.plotData);

            var dirt = land.GetComponent<IFillOnAble>();
            if (dirt == null) return;
            ApplyFillOnCallBack(dirt);
        };
    }

    private void ApplyFillOnCallBack(IFillOnAble dirt)
    {
        dirt.OnFillOnUnable += (source) =>
        {
            var entity = source.GetComponent<FarmEntity>();

            if (entity != null)
                plantedEntities.Add(entity);

            landsHaveUse.Add(dirt);

            if (vacantplots.Contains(dirt))
                vacantplots.Remove(dirt);


            OnFarmChanged?.Invoke(vacantplots, landsHaveUse);
        };

        dirt.OnFillOnAnble += (source) =>
        {
            var entity = source.GetComponent<FarmEntity>();

            if (entity != null && plantedEntities.Contains(entity))
                plantedEntities.Remove(entity);

            vacantplots.Add(dirt);

            if (landsHaveUse.Contains(dirt))
                landsHaveUse.Remove(dirt);

            OnFarmChanged?.Invoke(vacantplots, landsHaveUse);
        };
    }

    [ContextMenu("Clear Farm Plots")]
    private void ClearFarmPlots()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                DestroyImmediate(farmPlots[row, col]);
                farmPlots[row, col] = null;
                plotDatas[row, col] = null;
            }
        }
        plots.Clear();
    }
    [ContextMenu("Show Data")]
    private void ShowData()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (farmPlots[row, col] == null) continue;
                Debug.Log(col + " " + row);
            }
        }
    }
    public void Save(GameData gameData)
    {
        gameData.plotDatas.Clear();
        gameData.plotDatas.AddRange(plotsToSave);
        plotsToSave.Clear();
    }
    public void Load(GameData gameData)
    {
        LoadFarmEntities();
        Debug.Log(gameData.plotDatas.Count);
        try
        {
            foreach (var plotData in gameData.plotDatas)
            {
                // Kiểm tra index hợp lệ
                if (plotData.row < 0 || plotData.row >= farmPlots.GetLength(0) ||
                    plotData.col < 0 || plotData.col >= farmPlots.GetLength(1))
                {
                    Debug.LogWarning($"Plot index out of range: {plotData.row}, {plotData.col}");
                    continue;
                }

                // create dirt object
                if (farmPlots[plotData.row, plotData.col] == null)
                    return;
                farmPlots[plotData.row, plotData.col].OnFill(dirtPrefab);

                var dirtObj = farmPlots[plotData.row, plotData.col].currentObj.GetComponent<Dirt>();
                // add plot data to plotsToSave
                // create entity object
                if (plotData.dirtData.hasEntity == true)
                {
                    GameObject entityLoad = null;
                    foreach (var entity in baseFarmEntities)
                    {
                        if (entity.data != null && entity.data.type == plotData.dirtData.type)
                        {
                            entityLoad = entity.gameObject;
                            break;
                        }
                    }
                    if (entityLoad != null)
                    {

                        dirtObj.OnFill(entityLoad);
                        dirtObj.currentEntity.SetPlantData(plotData.dirtData.entityData);
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    private void LoadFarmEntities()
    {
        foreach (var entity in entites)
        {
            baseFarmEntities.Add(entity.GetComponent<FarmEntity>());
        }
    }
}
