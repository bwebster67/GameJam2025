using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "MusicActions/MakeCircleAction")]
public class MakeCircleAction : MusicAction 
{
    public GameObject prefab;
    public Quaternion rotation = Quaternion.identity;

    public override IEnumerator Execute(MonoBehaviour runner)
    {
        Debug.Log("MakeCircleAction");
        Transform parent = runner.transform;

        Vector3 spawnPos = parent != null ? parent.position : runner.transform.position;
        UnityEngine.Object.Instantiate(prefab, spawnPos, rotation);
        yield break;
    }
}
