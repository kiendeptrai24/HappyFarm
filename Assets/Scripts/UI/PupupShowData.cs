using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PupupShowData : MonoBehaviour
{
    public static PupupShowData Instance { get; private set; }
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider growthSlider;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private Slider witherDelaySlider;
    [SerializeField] private TextMeshProUGUI witherDelayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI yieldText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button havestButton;
    [SerializeField] private Button closeButton;
    private FarmEntity currentEnity;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        closeButton.onClick.AddListener(Hide);
        havestButton.onClick.AddListener(() =>
        {
            if (currentEnity != null)
            {
                var data = currentEnity.Harvest();
                if (data != null)
                {
                    //InGameUIManager.Instance.AddToInventory(data, data.yieldAmount);
                    Debug.Log($"Harvested {data.yieldAmount} of {data.name}");
                    Hide();
                }
            }
        });
        popupPrefab.SetActive(false);
    }
    public void SetData(FarmEntity entity)
    {
        this.currentEnity = entity;
    }
    private void Update()
    {
        if (popupPrefab.activeSelf)
        {
            Show();
        }
    }
    public void Show()
    {
        if (currentEnity == null) return;

        // Tính phần trăm tăng trưởng
        float growthPercent = Mathf.Min((Time.time - currentEnity.startTime) / currentEnity.data.timeToHarvest, 1f);

        // Tính thời gian còn lại để thu hoạch
        float remainingLifeTime = Mathf.Max(currentEnity.data.timeToHarvest - (Time.time - currentEnity.startTime), 0f);

        // Tính trạng thái héo
        float witherPercent;
        float remainingWitherTime;

        if (currentEnity.witherStartTime != float.MaxValue)
        {
            witherPercent = Mathf.Min((Time.time - currentEnity.witherStartTime) / currentEnity.data.witherDelay, 1f);
            remainingWitherTime = Mathf.Max(currentEnity.data.witherDelay - (Time.time - currentEnity.witherStartTime), 0f);
        }
        else
        {
            witherPercent = 0f;
            remainingWitherTime = 0f;
        }

        // Cập nhật UI
        popupPrefab.SetActive(true);
        nameText.text = currentEnity.data.name;
        growthSlider.value = growthPercent;
        timeText.text = $"{TimeSpan.FromSeconds(remainingLifeTime):m\\:ss}";
        lifeText.text = $"Life Cycles: {currentEnity.data.lifeCycles}";
        yieldText.text = $"Yield: {currentEnity.data.yieldAmount}";
        priceText.text = $"Price: {currentEnity.data.price} coins/1 unit";
        witherDelaySlider.value = witherPercent;
        witherDelayText.text = $"{TimeSpan.FromSeconds(remainingWitherTime):m\\:ss}";
    }

    public void Hide()
    {
        popupPrefab.SetActive(false);
    }
}
