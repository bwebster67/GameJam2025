using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    private Animator animator;
    public GameObject icon;
    public UnityEngine.UI.Image iconImage;

    void Awake()
    {
        animator = GetComponent<Animator>();
        iconImage = icon.GetComponent<UnityEngine.UI.Image>();
        iconImage.color = Color.clear;
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
