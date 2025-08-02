using UnityEngine;
using System.Collections;

public class ParticleSystemDisableScript : MonoBehaviour
{
    [SerializeField] private float disableDelay = 1f; // Time before disabling (match particle lifetime)

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}