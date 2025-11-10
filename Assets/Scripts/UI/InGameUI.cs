using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Toggle shop;
    [SerializeField] private GameObject inventory;
    [Header("Shop Panels")]
    [SerializeField] private Button DirtBtn;
    [SerializeField] private Button TreeBtn;
    [SerializeField] private Button animalBtn;
    [SerializeField] private Button BuildingBtn;
    [SerializeField] private GameObject dirtPanel;
    [SerializeField] private GameObject treePanel;
    [SerializeField] private GameObject animalPanel;
    [SerializeField] private GameObject buildingPanel;

    private void Awake() {
        shop.onValueChanged.AddListener(OnShopToggleChanged);
        DirtBtn.onClick.AddListener(ShowDirtPanel);
        TreeBtn.onClick.AddListener(ShowTreePanel);
        animalBtn.onClick.AddListener(ShowAnimalPanel);
        BuildingBtn.onClick.AddListener(ShowBuildingPanel);
        ShowDirtPanel();
    }

    private void OnShopToggleChanged(bool active)
    {
        if (active)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
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
