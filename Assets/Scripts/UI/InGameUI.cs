using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Toggle shopToggle;
    [SerializeField] private Toggle inventoryToggle;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private List<GameObject> panels;
    [Header("Shop Panels")]
    [SerializeField] private Button DirtBtn;
    [SerializeField] private Button TreeBtn;
    [SerializeField] private Button animalBtn;
    [SerializeField] private Button BuildingBtn;
    [SerializeField] private GameObject dirtPanel;
    [SerializeField] private GameObject treePanel;
    [SerializeField] private GameObject animalPanel;
    [SerializeField] private GameObject buildingPanel;

    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI farmer;
    [SerializeField] private TextMeshProUGUI unusedseeds;
    [SerializeField] private TextMeshProUGUI land;
    [SerializeField] private TextMeshProUGUI product;


    [SerializeField] private Inventory inventoryData;
    [SerializeField] private FarmManager farmManagerData;
    [SerializeField] private TaskManager taskManagerData;
    [SerializeField] private FarmerManager farmerManager;
    private void Awake() {
        inventoryData.OnProductDataUpdate += OnProductDataChanged;
        inventoryData.OnSeedDataUpdate += OnSeedDataChanged;
        farmManagerData.OnFarmChanged += OnFarmChanged;
        taskManagerData.OnTaskChanged += OnTaskChanged;
        farmerManager.OnFarmerChanged += OnFarmerChanged;

        shopToggle.onValueChanged.AddListener(OnShopToggleChanged);
        inventoryToggle.onValueChanged.AddListener(OnInventoryToggleChanged);
        DirtBtn.onClick.AddListener(ShowDirtPanel);
        TreeBtn.onClick.AddListener(ShowTreePanel);
        animalBtn.onClick.AddListener(ShowAnimalPanel);
        BuildingBtn.onClick.AddListener(ShowBuildingPanel);
        ShowDirtPanel();
        shopToggle.isOn = true;
        inventoryToggle.isOn = false;


    }

    private void OnSeedDataChanged(List<SeedData> list)
    {
        string unusedseedsData = "Seed in Store: \n";
        foreach (var seed in list)
        {
            unusedseedsData += $"Seed: {seed.name}/x{seed.yieldAmount} \n";
        }
        unusedseeds.text  = unusedseedsData;
    }
    public void OnProductDataChanged(List<FarmProductData> data)
    {
        string inventoryData = "Product in Inventory: \n";
        foreach (var product in data)
        {
            inventoryData += $"-{product.name} x{product.yieldAmount}\n";
        }
        product.text = inventoryData;
    }
    public void OnFarmerChanged(int idle, int working)
    {

        farmer.text = $"Farmer in farm: \n -Farmer working: {working}\n-Farmer idle: {idle}";
    }
    public void OnFarmChanged(List<IFillOnAble> vacantplots, List<IFillOnAble> landsHaveUse)
    {
        land.text = $"Land in farm: \n-land in use: {landsHaveUse.Count}\n-land vacant: {vacantplots.Count}";
    }
    public void OnTaskChanged(List<IFarmTaskBase> inProgressMissions, List<IFarmTaskBase> completedMissions)
    {

    }
    public void OnGameDataChange()
    {

    }

    private void OnShopToggleChanged(bool active)
    {
        inventoryToggle.isOn = !active;
        ShowPanel(shopPanel, active);
    }
    private void OnInventoryToggleChanged(bool active)
    {
        shopToggle.isOn = !active;
        ShowPanel(inventoryPanel, active);
    }
    public void ShowPanel(GameObject panel, bool show)
    {
        foreach (var item in panels)
        {
            if (item != panel)
                item.SetActive(false);
            else
            {
                panel.SetActive(show);
            }
        }
    }

    private void ShowAnimalPanel()
    {
        animalPanel.SetActive(true);
        treePanel.SetActive(false);
        buildingPanel.SetActive(false);
        dirtPanel.SetActive(false);
    }
    public void ShowDirtPanel()
    {
        dirtPanel.SetActive(true);
        treePanel.SetActive(false);
        buildingPanel.SetActive(false);
        animalPanel.SetActive(false);
    }
    public void ShowTreePanel()
    {
        dirtPanel.SetActive(false);
        treePanel.SetActive(true);
        buildingPanel.SetActive(false);
        animalPanel.SetActive(false);
    }
    public void ShowBuildingPanel()
    {
        dirtPanel.SetActive(false);
        treePanel.SetActive(false);
        buildingPanel.SetActive(true);
        animalPanel.SetActive(false);
    }

}
