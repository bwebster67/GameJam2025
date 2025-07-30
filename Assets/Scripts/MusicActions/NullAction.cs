using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class NullAction : MusicAction 
{
    public override IEnumerator Execute(MonoBehaviour runner)
    {
        Debug.Log("null action");
        yield break;
    }
}
