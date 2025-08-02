using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TestPrintAction : MusicAction 
{
    public override IEnumerator Execute(MonoBehaviour runner)
    {
        Debug.Log("TestPrintAction Activated!");
        yield break;
    }
}
