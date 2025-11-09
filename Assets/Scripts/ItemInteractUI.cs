using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInteractUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public Image itemIcon;
    public GameObject itemPrefab;
    public GameObject ghostItemPrefab;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
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
        DragManager.Instance.EndDrag(this);
        rectTransform.position = startPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // có thể để trống
    }
}
