using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
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
    }
    public void NoteRise()
    {
        animator.SetTrigger("IsActive");
        // animator.ResetTrigger("IsActive");
        animator.SetTrigger("IsInactive");
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }

}
