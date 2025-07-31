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

    public float damage;

    public override IEnumerator Execute(MonoBehaviour runner)
    {
        // Debug.Log("MakeCircleAction");
        Transform parent = runner.transform;

        Vector3 spawnPos = parent != null ? parent.position : runner.transform.position;
        GameObject circle = UnityEngine.Object.Instantiate(prefab, spawnPos, rotation);

        if (circle.TryGetComponent<CircleProjectile>(out var circleProjectile))
        {
            circleProjectile.damage = damage;
        }

        yield break;
    }
}
