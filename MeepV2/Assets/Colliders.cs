using UnityEngine;
using System.Collections.Generic;

public class Colliders : MonoBehaviour
{
    public List<Collider> collidersToIgnore;

    private void Start()
    {
        Collider thisCollider = GetComponent<Collider>();
        if (thisCollider == null) return;

        foreach (Collider other in collidersToIgnore)
        {
            if (other != null)
            {
                Physics.IgnoreCollision(thisCollider, other);
            }
        }
    }
}
