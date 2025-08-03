using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{

    public GameObject Option1Card;
    public GameObject Option2Card;
    public GameObject Option3Card;
    private MusicAction Option1Ability;
    private MusicAction Option2Ability;
    private MusicAction Option3Ability;
    public UnityEngine.UI.Image Option1CardIcon;
    public UnityEngine.UI.Image Option2CardIcon;
    public UnityEngine.UI.Image Option3CardIcon;
    public TMP_Text Option1NotesText;
    public TMP_Text Option2NotesText;
    public TMP_Text Option3NotesText;
    private int Option1Notes;
    private int Option2Notes;
    private int Option3Notes;
    public List<MusicAction> musicActions;
    public GameObject DragDropPrefab;
    public Canvas canvas;
    public GameObject SaveButton;
    public static event Action OnFinishLevelUpSequence;

    void Start()
    {
        //if (canvas == null)
        //    canvas = GameObject.FindGameObjectWithTag("Canvas");


        int randomIndex;

        // Option 1
        randomIndex = UnityEngine.Random.Range(0, musicActions.Count);
        Option1Ability = musicActions[randomIndex];
        musicActions.RemoveAt(randomIndex);
        Option1CardIcon.sprite = Option1Ability.actionIcon;
        Option1Notes = UnityEngine.Random.Range(2,5);
        Option1NotesText.text = $"{Option1Notes}";

        // Option 2
        randomIndex = UnityEngine.Random.Range(0, musicActions.Count);
        Option2Ability = musicActions[randomIndex];
        musicActions.RemoveAt(randomIndex);
        Option2CardIcon.sprite = Option2Ability.actionIcon;
        Option2Notes = UnityEngine.Random.Range(2,5);
        Option2NotesText.text = $"{Option2Notes}";
        
        // Option 3
        randomIndex = UnityEngine.Random.Range(0, musicActions.Count);
        Option3Ability = musicActions[randomIndex];
        musicActions.RemoveAt(randomIndex);
        Option3CardIcon.sprite = Option3Ability.actionIcon;
        Option3Notes = UnityEngine.Random.Range(2,5);
        Option3NotesText.text = $"{Option3Notes}";
        
    }

    public void ChooseOption1()
    {
        Debug.Log("Player Chose Option 1");
        OptionSelected();
        Destroy(Option2Card);
        Destroy(Option3Card);
        for (int i = 0; i < Option1Notes; i++)
        {
            GameObject dragDropPrefab = Instantiate(DragDropPrefab, canvas.transform);
            DragDrop dragDrop = dragDropPrefab.GetComponent<DragDrop>();
            dragDrop.canvas = canvas.GetComponent<Canvas>();
            dragDrop.musicAction = Option1Ability;
            UnityEngine.UI.Image dragDropPrefabImage = dragDropPrefab.GetComponent<UnityEngine.UI.Image>();
            dragDropPrefabImage.sprite = Option1CardIcon.sprite;
            RectTransform rect = dragDropPrefab.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; // Center in parent canvas
            } 
        }
    }
    
    public void ChooseOption2()
    {
        Debug.Log("Player Chose Option 2");
        OptionSelected();
        Destroy(Option1Card);
        Destroy(Option3Card);
        for (int i = 0; i < Option2Notes; i++)
        {
            GameObject dragDropPrefab = Instantiate(DragDropPrefab, canvas.transform);
            DragDrop dragDrop = dragDropPrefab.GetComponent<DragDrop>();
            dragDrop.canvas = canvas.GetComponent<Canvas>();
            dragDrop.musicAction = Option2Ability;
            UnityEngine.UI.Image dragDropPrefabImage = dragDropPrefab.GetComponent<UnityEngine.UI.Image>();
            dragDropPrefabImage.sprite = Option2CardIcon.sprite;
            RectTransform rect = dragDropPrefab.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; // Center in parent canvas
            } 
        }
    }
    
    public void ChooseOption3()
    {
        Debug.Log("Player Chose Option 3");
        OptionSelected();
        Destroy(Option1Card);
        Destroy(Option2Card);
        for (int i = 0; i < Option3Notes; i++)
        {
            GameObject dragDropPrefab = Instantiate(DragDropPrefab, canvas.transform);
            DragDrop dragDrop = dragDropPrefab.GetComponent<DragDrop>();
            dragDrop.canvas = canvas.GetComponent<Canvas>();
            dragDrop.musicAction = Option3Ability;
            UnityEngine.UI.Image dragDropPrefabImage = dragDropPrefab.GetComponent<UnityEngine.UI.Image>();
            dragDropPrefabImage.sprite = Option3CardIcon.sprite;
            RectTransform rect = dragDropPrefab.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; // Center in parent canvas
            } 
        }
    }

    public void OptionSelected()
    {
        if (Option1Card is not null)
            Destroy(Option1Card);
        if (Option2Card is not null)
            Destroy(Option2Card);
        if (Option3Card is not null)
            Destroy(Option3Card);
    }
    public void FinishLevelUpSequence()
    {
        Destroy(SaveButton);
        OnFinishLevelUpSequence.Invoke();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
