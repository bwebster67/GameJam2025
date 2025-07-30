using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TestPrintAction : MusicAction 
{
    public override void Execute()
    {
        Debug.Log("TestPrintAction Activated!");
    }
}
