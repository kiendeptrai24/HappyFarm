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
    public int itemAmountExisted;
    public int itemAmount;
    public int itemPrice;
    [Header("Item Component")]
    public Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemHas;
    public GameObject itemPrefab;
    public GameObject ghostItemPrefab;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;

    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        if (itemAmountExisted > 0)
            itemHas.text = $"Exists: {itemAmountExisted}$";
        else
            itemHas.text = $"{itemPrice}$/x{itemAmount}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        Popup.Show_Static(itemData.name, itemData.input, itemData.validCharacters, itemData.characterLimit,
         delegate(){}
         , (value,log) =>
        {
            if (itemAmount * itemData.price < inventory.coins)
            {
                int itemAmount = int.Parse(value);
                Shop.Instance.Buy(itemData, itemAmount);
                log.text = "";
                Popup.HideStatic();
            }
            else
            {
                log.text = "You Dont have enough!";
            }

        });
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = startPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // có thể để trống
    }

    public bool CanPurchase(long coins, int quatily)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchase(int quality)
    {
        throw new System.NotImplementedException();
    }
}
