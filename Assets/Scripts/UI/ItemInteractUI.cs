using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Shop;

public class ItemInteractUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler
{
    private Inventory inventory;
    [Header("Item Info")]
    public ShopItemData itemData;
    public ShopItemSaleData itemSaleData;
    public ShopItemType shopItemType;
    [Header("Item Component")]
    public Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemHas;
    public GameObject itemPrefab;
    public GameObject ghostItemPrefab;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private bool isEndDrag = false;


    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        inventory.OnSeedDataUpdate += seeds =>
        {
            UpdateItemUI(seeds);
        };
        inventory.OnProductDataUpdate += products =>
        {
            UpdateDataSale(products);
        };
        UpdateItemUI(inventory.seedDatas);
        UpdateDataSale(inventory.farmProductDatas);
    }

    private void UpdateDataSale(List<FarmProductData> products)
    {
        foreach (var product in products)
        {
            if (itemSaleData.type == product.type)
            {
                itemSaleData.quality = product.quantity;
                itemSaleData.price = product.price;
            }
        }
    }

    private void UpdateItemUI(List<SeedData> seeds)
    {
        foreach (var seed in seeds)
        {
            if (itemData.product == seed.type)
            {
                itemData.quality = seed.quantity;
                if (itemData.quality > 0)
                {
                    itemHas.text = $"x{itemData.quality}";
                }
                else
                {
                    itemHas.text = $"1/x{itemData.price}";

                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        isEndDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        DragManager.Instance.BeginDrag(itemPrefab, ghostItemPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        isEndDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = startPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEndDrag) return;

        Popup.Show_Static(itemData.name, itemData.input, itemData.validCharacters, itemData.characterLimit,
         delegate () { }
         , (value, log) =>
        {
            int itemAmount = 1;
            try
            {
                itemAmount = int.Parse(value);
            }
            catch (System.Exception)
            {
            }

            if (shopItemType == ShopItemType.EquipmentUpgrade)
            {
                inventory.UpgradeFarm(itemAmount);
                return;
            }
            if (Popup.instance.IsSale())
                LogicSale(log, itemAmount);
            else
                LogicBuy(log, itemAmount);
        });
    }

    private void LogicBuy(TextMeshProUGUI log, int itemAmount)
    {
        if (itemAmount * itemData.price < inventory.coins)
        {
            Shop.Instance.Buy(itemData, itemAmount);
            log.text = "";
            Popup.HideStatic();
        }
        else
        {
            log.text = "You Dont have enough!";
        }
    }

    private void LogicSale(TextMeshProUGUI log, int itemAmount)
    {
        Debug.Log(itemAmount + " " + itemSaleData.quality);
        if (itemAmount < itemSaleData.quality)
        {
            Shop.Instance.Sell(itemSaleData, itemAmount);
            log.text = "";
            Popup.HideStatic();
        }
        else
        {
            log.text = "You Dont have enough product to buy!";
        }
    }
}
