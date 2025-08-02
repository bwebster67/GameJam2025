using UnityEngine;

public class explosionFireBall : MonoBehaviour
{
   
    void OnEnable()
    {
        Destroy(gameObject, .5f);
        Debug.Log("epxlosion fireball hello!");
    }

    
}
