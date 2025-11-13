using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Toggle shopToggle;
    [SerializeField] private Toggle inventoryToggle;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private List<GameObject> panels;
    [Header("Shop Panels Info")]
    [SerializeField] private Button DirtBtn;
    [SerializeField] private Button TreeBtn;
    [SerializeField] private Button animalBtn;
    [SerializeField] private Button updgrateBtn;
    [SerializeField] private Button farmerBtn;
    [SerializeField] private GameObject dirtPanel;
    [SerializeField] private GameObject treePanel;
    [SerializeField] private GameObject animalPanel;
    [SerializeField] private GameObject updgratePanel;
    [SerializeField] private GameObject farmerPanel;

    [Header("Inventory Panels Info")]
    [SerializeField] private Button gameDataBtn;
    [SerializeField] private Button taskinfoBtn;
    [SerializeField] private GameObject gameDataPanel;
    [SerializeField] private GameObject taskinfoPanel;
    [SerializeField] private TextMeshProUGUI taskInfo;


    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI farmer;
    [SerializeField] private TextMeshProUGUI unusedseeds;
    [SerializeField] private TextMeshProUGUI land;
    [SerializeField] private TextMeshProUGUI product;


    [SerializeField] private Inventory inventoryData;
    [SerializeField] private FarmManager farmManagerData;
    [SerializeField] private TaskManager taskManagerData;
    [SerializeField] private FarmerManager farmerManager;
    [SerializeField] private DetectTask detectTask;

    [SerializeField] private RectTransform contentRectTransform;
    private void Awake()
    {
        inventoryData.OnProductDataUpdate += OnProductDataChanged;
        inventoryData.OnSeedDataUpdate += OnSeedDataChanged;
        inventoryData.OnCoinsAndLevelUpdate += OnCointsAndLevelUpdate;
        taskManagerData.OnTaskChanged += OnTaskChanged;
        farmerManager.OnFarmerChanged += OnFarmerChanged;
        detectTask.OnRefresh += OnFarmChanged;
        shopToggle.onValueChanged.AddListener(OnShopToggleChanged);
        inventoryToggle.onValueChanged.AddListener(OnInventoryToggleChanged);
        DirtBtn.onClick.AddListener(ShowDirtPanel);
        TreeBtn.onClick.AddListener(ShowTreePanel);
        animalBtn.onClick.AddListener(ShowAnimalPanel);
        updgrateBtn.onClick.AddListener(ShowUpdgratePanel);
        farmerBtn.onClick.AddListener(ShowFarmerPanel);
        gameDataBtn.onClick.AddListener(ShowGameDataPanel);
        taskinfoBtn.onClick.AddListener(ShowTaskInfoPanel);

        ShowDirtPanel();
        ShowGameDataPanel();
        shopToggle.isOn = true;
        inventoryToggle.isOn = false;
    }
    private void ShowTaskInfoPanel()
    {
        taskinfoPanel.SetActive(true);
        gameDataPanel.SetActive(false);

    }

    private void ShowGameDataPanel()
    {
        gameDataPanel.SetActive(true);
        taskinfoPanel.SetActive(false);
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);

    }

    private void OnCointsAndLevelUpdate(long coin, int level)
    {
        coinTxt.text = $"Coins: {coin}";
        levelTxt.text = $"Level: {level}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }

    private void OnSeedDataChanged(List<SeedData> list)
    {
        string unusedseedsData = "Seed in Store: \n";
        foreach (var seed in list)
        {
            unusedseedsData += $"Seed: {seed.name}/x{seed.quantity} \n";
        }
        unusedseeds.text = unusedseedsData;
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }
    public void OnProductDataChanged(List<FarmProductData> data)
    {
        string inventoryData = "Product in Inventory: \n";
        foreach (var product in data)
        {
            inventoryData += $"-{product.type.ToString()} x{product.quantity}\n";
        }
        product.text = inventoryData;
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }
    public void OnFarmerChanged(int idle, int working)
    {

        farmer.text = $"Farmer in farm: \n -Farmer working: {working}\n-Farmer idle: {idle}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }
    public void OnFarmChanged(List<IFillOnAble> vacantplots, List<IFillOnAble> landsHaveUse)
    {
        land.text = $"Land in farm: \n-land in use: {landsHaveUse.Count}\n-land vacant: {vacantplots.Count}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }
    public void OnTaskChanged(List<IFarmTaskBase> inProgressMissions, List<IFarmTaskBase> completedMissions)
    {
        taskInfo.text = "";
        string task = "Task in progress: \n";
        int count = 0;
        foreach (var inProgressMission in inProgressMissions)
        {
            count++;
            task += count.ToString() + ". " + inProgressMission.DisplayInfo() + "\n\n";
        }
        task += "Task completed: \n";
        count = 0;
        foreach (var completedMission in completedMissions)
        {
            count++;
            task += count.ToString() + ". " + completedMission.DisplayInfo() + "\n\n\n";
        }
        taskInfo.text = task;
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }

    private void OnShopToggleChanged(bool active)
    {
        Debug.Log("dsadasdasda");
        ShowPanel(shopPanel, active);
    }
    private void OnInventoryToggleChanged(bool active)
    {
        ShowPanel(inventoryPanel, active);
    }
    public void ShowPanel(GameObject panel, bool show)
    {
        panel.SetActive(show);
    }

    private void ShowAnimalPanel()
    {
        animalPanel.SetActive(true);
        treePanel.SetActive(false);
        updgratePanel.SetActive(false);
        dirtPanel.SetActive(false);
        farmerPanel.SetActive(false);
    }
    public void ShowDirtPanel()
    {
        dirtPanel.SetActive(true);
        treePanel.SetActive(false);
        updgratePanel.SetActive(false);
        animalPanel.SetActive(false);
        farmerPanel.SetActive(false);
    }
    public void ShowTreePanel()
    {
        dirtPanel.SetActive(false);
        treePanel.SetActive(true);
        updgratePanel.SetActive(false);
        animalPanel.SetActive(false);
        farmerPanel.SetActive(false);
    }
    public void ShowUpdgratePanel()
    {
        dirtPanel.SetActive(false);
        treePanel.SetActive(false);
        updgratePanel.SetActive(true);
        animalPanel.SetActive(false);
        farmerPanel.SetActive(false);
    }
    public void ShowFarmerPanel()
    {
        farmerPanel.SetActive(true);
        dirtPanel.SetActive(false);
        treePanel.SetActive(false);
        updgratePanel.SetActive(false);
        animalPanel.SetActive(false);
    }

}
