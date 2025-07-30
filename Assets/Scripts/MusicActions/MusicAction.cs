using System.Collections;
using UnityEngine;

/* base template class for a music action */
public abstract class MusicAction : ScriptableObject
{
    /* function that executes the action */
    public abstract IEnumerator Execute(MonoBehaviour runner);
}
