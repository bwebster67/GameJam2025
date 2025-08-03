using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerMusicController : MonoBehaviour
{

    [Header("Timing Settings")]
    public int beatsPerCycle = 16;
    public double bpm = 120;

    [Header("Audio")]
    public AudioSource audioTemplate;

    public List<MusicAction> beatActions;

    private double secondsPerBeat;
    private double nextBeatDspTime;
    public int currentBeatIndex = 0;
    public List<MusicAction> allMusicActions;

    [Header("Music Actions")]
    public MakeCircleAction MakeCircleAction;
    public SnareAction SnareAction;
    public MakeFireBallAction MakeFireBallAction;
    public MakeVortexAction MakeVortexAction;
    public MakeFireSpinAction MakeFireSpinAction;
    public NullAction NullAction;

    // Events
    public UnityEvent NewBeat;
    public UnityEvent<int> NewNoteAdded;
    public static event Action LevelUpEvent;

    public GameObject NoteBarGO;
    private NoteBar noteBar;
    public GameObject DragDropPrefab;
    public GameObject canvas;

    // Experience
    public float playerExperience = 0f;
    public float levelUpThreshold = 7;


    public UIManagerScript uiMngr;
    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
        UIManagerScript.OnPauseEvent += StopMusic;
        UIManagerScript.OnUnPauseEvent += StartMusic;
        Note.OnNoteDropped += AddDroppedNode;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
        UIManagerScript.OnPauseEvent -= StopMusic;
        UIManagerScript.OnUnPauseEvent -= StartMusic;
        Note.OnNoteDropped -= AddDroppedNode;
    }

    private void Awake()
    {
        if (NewBeat == null) NewBeat = new UnityEvent();
        if (NewNoteAdded == null) NewNoteAdded = new UnityEvent<int>();
    }

    private void Start()
    {
        beatActions = new List<MusicAction>();
        noteBar = NoteBarGO.GetComponent<NoteBar>();
        secondsPerBeat = 60.0 / bpm;

        // Fill list with NullAction
        while (beatActions.Count < beatsPerCycle)
            beatActions.Add(null);

        for (int i = 0; i < beatActions.Count; i++) {
            AddNonDroppedNode(i, NullAction);
        }

        // temp add red circle actions
        for (int i = 6; i < beatsPerCycle; i += 7)
            AddNonDroppedNode(i, MakeCircleAction);
        // temp add snare actions
        for (int i = 4; i < beatsPerCycle; i += 7)
            AddNonDroppedNode(i, SnareAction);

        StartMusic();
    }
    public void StartMusic()
    {
        double now = AudioSettings.dspTime;
        nextBeatDspTime = Mathf.Ceil((float)(now / secondsPerBeat)) * secondsPerBeat;

        StartCoroutine(BeatLoop());
    }
    public void StopMusic()
    {
        StopAllCoroutines();
    }
    public void AddNote(int index, MusicAction action)
    {
        beatActions[index] = action;
        NewNoteAdded.Invoke(index);
    }
    public void AddNonDroppedNode(int index, MusicAction action)
    {
        GameObject dragDropGO = Instantiate(DragDropPrefab, canvas.transform);
        dragDropGO.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        DragDrop dragDrop = dragDropGO.GetComponent<DragDrop>();
        dragDrop.canvas = canvas.GetComponent<Canvas>();
        dragDrop.musicAction = action;
        UnityEngine.UI.Image dragDropPrefabImage = dragDropGO.GetComponent<UnityEngine.UI.Image>();
        dragDropPrefabImage.sprite = action.actionIcon;
        RectTransform rect = dragDropGO.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero; // Center in parent canvas
        } 

        dragDropGO.transform.SetParent(noteBar.Notes[index].transform);
        dragDropGO.transform.position = noteBar.Notes[index].transform.position + new Vector3(0, 3, 0);
        
        AddNote(index, action);
    }
    public void AddDroppedNode(int index, MusicAction action)
    {
        AddNote(index, action);
    }
    public void ChangeToCircleAttack(int index)
    {
        AddNote(index, MakeCircleAction);
    }
    private IEnumerator BeatLoop()
    {
        while (true)
        {
            NewBeat.Invoke();
            // a) Pick the upcoming action index
            int upcomingIndex = (currentBeatIndex + 1) % beatsPerCycle;
            MusicAction upcoming = beatActions[upcomingIndex];
            var clip = upcoming?.clip;
            double clipLen = clip?.length ?? 0;

            // get and reserve audio source
            AudioSource src = GetFreeAudioSource();
            double reserveUntil = nextBeatDspTime + clipLen + 0.05;

            foreach (var ps in _audioPool)
            {
                if (ps.src == src)
                {
                    ps.nextFreeDspTime = reserveUntil;
                    break;
                }
            }
            src.clip = clip;
            src.PlayScheduled(nextBeatDspTime);
            if (clip != null)
            {
                src.SetScheduledEndTime(nextBeatDspTime + clipLen + 0.05);
            }

            yield return new WaitUntil(() => AudioSettings.dspTime >= nextBeatDspTime - 0.1); // makes animation start 0.1s earlier
            noteBar.BumpNote(upcomingIndex);
            yield return new WaitUntil(() => AudioSettings.dspTime >= nextBeatDspTime);

            double drift = AudioSettings.dspTime - nextBeatDspTime;
            Debug.Log($"Drift on beat {upcomingIndex}: {drift * 1000:F1} ms");
            currentBeatIndex = upcomingIndex;
            yield return StartCoroutine(upcoming.Execute(this));

            // advance to next beat
            nextBeatDspTime += secondsPerBeat;

        }
    }


    class PooledSource
    {
        public AudioSource src;
        public double nextFreeDspTime;
    }

    private List<PooledSource> _audioPool = new List<PooledSource>();

    private AudioSource GetFreeAudioSource()
    {
        double nowDsp = AudioSettings.dspTime;

        // 1) See if any source’s nextFreeDspTime has already passed
        foreach (var ps in _audioPool)
        {
            if (nowDsp >= ps.nextFreeDspTime)
                return ps.src;
        }

        // 2) Otherwise clone a fresh one
        var dup = Instantiate(audioTemplate, transform);
        dup.playOnAwake = false;
        var newPs = new PooledSource
        {
            src = dup,
            nextFreeDspTime = nowDsp  // available immediately
        };
        _audioPool.Add(newPs);
        return dup;
    }
    public void changeMusicVolume(float volume) /// 0-1
    {
        audioTemplate.volume = volume;
    }

    public void HandleEnemyDeath(Enemy enemy)
    {
        GainExp(enemy.expValue);
    }

    public void GainExp(float exp)
    {
        playerExperience += exp;
        if (playerExperience >= levelUpThreshold)
        {
            LevelUp();
            playerExperience -= levelUpThreshold;
            levelUpThreshold *= 1.3f;
            levelUpThreshold += 3;
        }
    }
    public void LevelUp()
    {
        LevelUpEvent.Invoke();
    }

}