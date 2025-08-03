using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class Note : MonoBehaviour, IDropHandler
{
    private Animator animator;
    public GameObject icon;
    public UnityEngine.UI.Image iconImage;
    public RectTransform rectTransform;
    public static event Action<int, MusicAction> OnNoteDropped;
    public int NoteIndex;
    public GameObject DroppedIcon = null;
    public bool HasAction = false;
    public NullAction nullAction;
    private PlayerMusicController playerMusicController;
    private GameObject playerGO;

    void Update()
    {
        bool hasDragDrop = GetComponentInChildren<DragDrop>() != null;
        HasAction = hasDragDrop;

        if (!HasAction)
        {
            playerMusicController.AddNonDroppedNode(NoteIndex, nullAction);
        }
        
        DragDrop childDragDrop = GetComponentInChildren<DragDrop>();
        if (childDragDrop != null)
        {
            DroppedIcon = childDragDrop.gameObject;
        }

    }
    public void OnDrop(PointerEventData eventData)
    {
        // If has action, don't drop
        // IN THE FUTURE MAKE IT SWITCH PLACES 

        // 
        if (eventData.pointerDrag != null)
        {
            GameObject IncomingIcon = eventData.pointerDrag;
            DragDrop dragDrop = DroppedIcon.GetComponent<DragDrop>();
            if (dragDrop.musicAction is NullAction)
            {
                IncomingIcon.transform.SetParent(rectTransform.transform);
                Destroy(DroppedIcon);
            }
            
            IncomingIcon.transform.position = rectTransform.position + new Vector3(0, 3, 0);
            HasAction = true;
            DragDrop incomingDragDrop = IncomingIcon.GetComponent<DragDrop>();
            OnNoteDropped.Invoke(NoteIndex, incomingDragDrop.musicAction);
        }
    }

    void Awake()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerMusicController = playerGO.GetComponent<PlayerMusicController>();
        }
        animator = GetComponent<Animator>();
        iconImage = icon.GetComponent<UnityEngine.UI.Image>();
        iconImage.color = Color.clear;
    }
    void Start()
    {
    }
    public void NoteRise()
    {
        animator.SetTrigger("IsActive");
        animator.SetTrigger("IsInactive");
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        iconImage.color = Color.white;
    }

}
