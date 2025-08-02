using System.Collections;
using System.Collections.Generic;
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
    public MakeCircleAction MakeCircleAction;
    public NullAction NullAction;
    public UnityEvent NewBeat;
    public UnityEvent<int> NewNoteAdded;
    public GameObject NoteBarGO;
    private NoteBar noteBar;
    private void Awake()
    {
        if (NewBeat == null)      NewBeat = new UnityEvent();
        if (NewNoteAdded == null) NewNoteAdded = new UnityEvent<int>();
    }

    private void Start()
    {
        beatActions = new List<MusicAction>();
        noteBar = NoteBarGO.GetComponent<NoteBar>();
        secondsPerBeat = 60.0 / bpm;

        // Fill list with NullAction
        while (beatActions.Count < beatsPerCycle)
            beatActions.Add(NullAction);

        // temp add red circle actions
        for (int i = 6; i < beatsPerCycle; i += 7)
            AddNote(MakeCircleAction, i);

        StartMusic();
    }
    public void StartMusic()
    {
        double now = AudioSettings.dspTime;
        nextBeatDspTime = Mathf.Ceil((float)(now / secondsPerBeat)) * secondsPerBeat;

        StartCoroutine(BeatLoop());
    }
    public void AddNote(MusicAction action, int index)
    {
        beatActions[index] = action;
        NewNoteAdded.Invoke(index);
    }
    public void ChangeToCircleAttack(int index)
    {
        AddNote(MakeCircleAction, index);
    }
    private IEnumerator BeatLoop()
    {
        while (true)
        {
            NewBeat.Invoke();
            // a) Pick the upcoming action index
            int upcomingIndex = (currentBeatIndex + 1) % beatsPerCycle;
            MusicAction upcoming = beatActions[upcomingIndex];
            var clip = upcoming.clip;
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

            // int safety_debug = 0;
            // while (AudioSettings.dspTime < nextBeatDspTime)
            // {
            //     // safety_debug++;
            //     // if (safety_debug > 1000)
            //     // {
            //     //     break;
            //     // } 
            // }
            // I think the drift happens here
            yield return new WaitUntil(() => AudioSettings.dspTime >= nextBeatDspTime);
            // NewBeat.Invoke();

            double drift = AudioSettings.dspTime - nextBeatDspTime;
            Debug.Log($"Drift on beat {upcomingIndex}: {drift*1000:F1} ms");
            noteBar.BumpNote(upcomingIndex);
            currentBeatIndex = upcomingIndex;
            yield return StartCoroutine(upcoming.Execute(this));

            // 5) advance to next beat
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
        var newPs = new PooledSource {
            src = dup,
            nextFreeDspTime = nowDsp  // available immediately
        };
        _audioPool.Add(newPs);
        return dup;
    }

}