using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "MusicActions/MakeFireSpinAction")]
public class MakeFireSpinAction : MusicAction
{
    public GameObject fireSpin;
    public Quaternion rotation = new Quaternion(0f, 0f, 0f, -30f);

    public float damage;

    public override IEnumerator Execute(MonoBehaviour runner)
    {
        // Debug.Log("MakeCircleAction");
        Transform parent = runner.transform;

        Vector3 spawnPos = parent != null ? parent.position : runner.transform.position;
        GameObject fireSpinning = UnityEngine.Object.Instantiate(fireSpin, spawnPos, rotation);

        if (fireSpinning.TryGetComponent<FireSpinProj>(out var fireSpinProj))
        {
            fireSpinProj.damage = damage;
        }

        yield break;
    }
}
