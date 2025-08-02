using System.Collections;
using UnityEngine;

/* base template class for a music action */
public abstract class MusicAction : ScriptableObject
{
    public AudioClip clip;
    

    public virtual void ScheduleSound(AudioSource templateSource, double dspTime)
    {
        if (clip == null || templateSource == null)
            return;

        templateSource.clip = clip;
        templateSource.PlayScheduled(dspTime);
    }

    /* function that executes the action */
    public abstract IEnumerator Execute(MonoBehaviour runner);
}
