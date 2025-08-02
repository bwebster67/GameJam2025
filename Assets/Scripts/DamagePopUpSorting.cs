using UnityEngine;

public class DamagePopUpSorting : MonoBehaviour
{
    
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sortingLayerName = "Default";
        mr.sortingOrder = 22;

    }

    
}
