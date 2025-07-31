using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
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
    private int currentBeatIndex = -1;
    public List<MusicAction> allMusicActions; 
    public MakeCircleAction MakeCircleAction;
    public NullAction NullAction;

    private void Start()
    {
        beatActions = new List<MusicAction>();
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
    public void ChangeToCircleAttack(int index)
    {
        beatActions[index] = MakeCircleAction;
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