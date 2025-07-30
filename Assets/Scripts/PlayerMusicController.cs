using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMusicController : MonoBehaviour
{
    public int beatsPerCycle = 16;
    public List<MusicAction> beatActions = new List<MusicAction>();
    public string currentBeatAction = "none";
    private bool isPlaying = false;
    private int currentActionIndex = 0;

    private IEnumerator Instrumental()
    {
        /* The loop that is activated when the instrumental is playing */
        while (true)
        {
            beatActions[currentActionIndex].Execute();
            currentActionIndex = (currentActionIndex + 1) % beatsPerCycle;
            Debug.Log($"currentActionIndex: {currentActionIndex}");
            yield return new WaitForSeconds(1f);
        }
    }

    void StartInstrumental()
    {
        Debug.Log("Starting Instrumental");
        isPlaying = true;
        StartCoroutine(Instrumental());
    }

    void StopInstrumental()
    {
        Debug.Log("Stopping Instrumental");
        isPlaying = false;
        StopCoroutine(Instrumental());
    }

    /* Boilerplate functions */
    void Start()
    {
        for (int i = 0; i < beatsPerCycle; i++)
        {
            beatActions.Add(new NullAction());
        }

        beatActions[5] = new TestPrintAction();
        beatActions[10] = new TestPrintAction();
        beatActions[15] = new TestPrintAction();

        StartInstrumental();
    }

    void Update()
    {
        
    }
}
