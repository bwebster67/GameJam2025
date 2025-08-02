using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler 
{
    [SerializeField] public Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public MusicAction musicAction;
    public Vector3 dragStartLocation;
    public bool disabled = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Disable dragging if musicAction is NullAction
        if (musicAction is NullAction)
        {
            enabled = false;
            disabled = true;
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void Update()
    {
        if (musicAction is not NullAction)
        {
            disabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (disabled == false)
        {
            this.transform.SetParent(canvas.transform);
            dragStartLocation = this.transform.position;
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }


}
