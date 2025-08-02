using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NoteBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Note> Notes;
    public GameObject Player;
    private PlayerMusicController playerMusicController;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMusicController = Player.GetComponent<PlayerMusicController>();
        // playerMusicController.NewNoteAdded.AddListener(UpdateNodeIcon);
    }
    private void OnEnable()
    {
        PlayerMusicController.LevelUpEvent += HandleLevelUp;
        LevelUpUI.OnFinishLevelUpSequence += HandleFinishLevelUp;
    }

    private void OnDisable()
    {
        PlayerMusicController.LevelUpEvent += HandleLevelUp;
        LevelUpUI.OnFinishLevelUpSequence -= HandleFinishLevelUp;
    }
    public void HandleLevelUp()
    {
        animator.SetTrigger("Grow");
    }
    public void HandleFinishLevelUp()
    {
        animator.ResetTrigger("Grow");
        animator.SetTrigger("Shrink");
    }
    void UpdateNodeIcon(int index)
    {
        Notes[index].SetIcon(playerMusicController.beatActions[index].actionIcon);
    }

    public void BumpNote(int index)
    {
        Notes[index].NoteRise();
    }

}
