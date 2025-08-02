using UnityEngine;
using System.Collections;

public class FireSpinProj : ProjectileTemp
{

    /* projectile lifespan in seconds */
    public Vector3 targetScale = Vector3.zero;
    public float duration = 2f;// this var is shrink duration
    private float timeBeforeShrink = .5f;
    private void OnEnable()
    {
        lifespan = duration + timeBeforeShrink;
        StartShrink();
        Destroy(gameObject, lifespan);

    }

    protected override void DamageEnemy(Collider2D collider)
    {
        base.DamageEnemy(collider);



        Debug.Log("FireSpinProj hit something");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided");
        if (collider.gameObject.CompareTag("Enemy"))
        {
            DamageEnemy(collider);
        }
    }


    public void StartShrink()
    {
        StartCoroutine(ShrinkCoroutine());
    }

    private IEnumerator ShrinkCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeShrink);
        Vector3 initialScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            yield return null;
        }

    }
}
