using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class NullAction : MusicAction 
{
    public override void Execute()
    {
        Debug.Log("null action");
    }
}
