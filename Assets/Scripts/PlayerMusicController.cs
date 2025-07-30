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
    public MakeCircleAction MakeCircleAction;

    private IEnumerator Instrumental()
    {
        /* The loop that is activated when the instrumental is playing */
        while (true)
        {
            // beatActions[currentActionIndex].Execute();
            yield return StartCoroutine(beatActions[currentActionIndex].Execute(this));
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
            beatActions.Add(ScriptableObject.CreateInstance<NullAction>());
        }

        beatActions[5] = MakeCircleAction;
        beatActions[10] = MakeCircleAction;
        beatActions[15] = MakeCircleAction;
        // beatActions[10] = ScriptableObject.CreateInstance<TestPrintAction>();
        // beatActions[15] = ScriptableObject.CreateInstance<TestPrintAction>();

        StartInstrumental();
    }

    void Update()
    {
        
    }
}
