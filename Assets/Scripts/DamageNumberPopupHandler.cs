using UnityEngine;

public class FloatingDamageTextHandler : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.75f);
    }

}
