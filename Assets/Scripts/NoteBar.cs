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
    void Awake()
    {
        playerMusicController = Player.GetComponent<PlayerMusicController>();
        playerMusicController.NewNoteAdded.AddListener(UpdateNodeIcon);
    }
    void Start()
    {
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
