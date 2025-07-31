using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMusicController : MonoBehaviour
{
    // [Header("Timing Settings")]
    // public int beatsPerCycle = 20;
    // public double bpm = 120;
    // private double nextBeatTime;
    // public AudioSource audioSource;
    // public List<MusicAction> beatActions = new List<MusicAction>();
    // public string currentBeatAction = "none";
    // private bool isPlaying = false;
    // private int currentActionIndex = 0;
    // public MakeCircleAction MakeCircleAction;

    // private IEnumerator Instrumental()
    // {
    //     /* The loop that is activated when the instrumental is playing */
    //     while (true)
    //     {
    //         // beatActions[currentActionIndex].Execute();
    //         yield return StartCoroutine(beatActions[currentActionIndex].Execute(this));
    //         currentActionIndex = (currentActionIndex + 1) % beatsPerCycle;
    //         Debug.Log($"currentActionIndex: {currentActionIndex}");
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    // void StartInstrumental()
    // {
    //     Debug.Log("Starting Instrumental");
    //     isPlaying = true;
    //     StartCoroutine(Instrumental());
    // }

    // void ScheduleNextSound()
    // {
    //     // schedule the click sound on the DSP clock
    //     audioSource.PlayScheduled(nextBeatTime);

    //     // queue up your game-logic callback exactly at that DSP time
    //     StartCoroutine(InvokeOnBeat(nextBeatTime));

    //     // compute and schedule the following click
    //     nextBeatTime += (60.0 / bpm);
    //     Invoke(nameof(ScheduleNextSound), (float)((60.0 / bpm) - 0.01));
    //     // small lead-in so it queues before reaching dspTime
    // }

    // private IEnumerator InvokeOnBeat(double dspBeatTime)
    // {
    //     // wait until the DSP clock reaches the beat
    //     while (AudioSettings.dspTime < dspBeatTime)
    //         yield return null;
    //     // DO your action on-beat:
    // }

    // void Start()
    // {
    //     for (int i = 0; i < beatsPerCycle; i++)
    //     {
    //         beatActions.Add(ScriptableObject.CreateInstance<NullAction>());
    //     }

    //     beatActions[5] = MakeCircleAction;
    //     beatActions[10] = MakeCircleAction;
    //     beatActions[15] = MakeCircleAction;

    //     // schedule the first sound one beat from now
    //     nextBeatTime = AudioSettings.dspTime + (60.0 / bpm);
    //     ScheduleNextSound(); /* uses nextBeatTime */

    //     StartInstrumental();
    // }

    [Header("Timing Settings")]
    public int beatsPerCycle = 16;
    public double bpm = 120;

    [Header("Audio")]
    [Tooltip("A template AudioSource. We'll reuse it to schedule any action's clip.")]
    public AudioSource audioTemplate;

    public List<MusicAction> beatActions = new List<MusicAction>();

    private double secondsPerBeat;
    private double nextBeatDspTime;
    private int currentBeatIndex = -1;
    public MakeCircleAction MakeCircleAction;
    public NullAction NullAction;

    private void Start()
    {
        secondsPerBeat = 60.0 / bpm;

        // ensure list is long enough
        while (beatActions.Count < beatsPerCycle)
            beatActions.Add(NullAction);

        // example: every 7th
        for (int i = 6; i < beatsPerCycle; i += 7)
            beatActions[i] = MakeCircleAction;

        double now = AudioSettings.dspTime;
        nextBeatDspTime = AudioSettings.dspTime + secondsPerBeat;
        nextBeatDspTime = Mathf.Ceil((float)(now / secondsPerBeat)) * secondsPerBeat;
        // ScheduleNextBeat();
        StartCoroutine(BeatLoop());
    }

    private IEnumerator BeatLoop()
    {
        while (true)
        {
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

            yield return new WaitUntil(() => AudioSettings.dspTime >= nextBeatDspTime);

            double drift = AudioSettings.dspTime - nextBeatDspTime;
            Debug.Log($"Drift on beat {upcomingIndex}: {drift*1000:F1} ms");
            currentBeatIndex = upcomingIndex;
            yield return StartCoroutine(upcoming.Execute(this));

            // 5) advance to next beat
            nextBeatDspTime += secondsPerBeat;

        }
    }

    private void DebugDrift(double scheduledDspTime)
    {
        double now = AudioSettings.dspTime;
        double drift = now - scheduledDspTime;
        Debug.Log($"Drift: {drift * 1000:F2} ms");
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