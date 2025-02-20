using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Cannon : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public XRController controller;
    public InputHelpers.Button fireButton = InputHelpers.Button.Trigger;
    public float activationThreshold = 0.1f;
    public float fireRange = 2f;
    public Transform player;
    public bool testFire = false;

    void Update()
    {
        if (testFire)
        {
            FireCannon();
            testFire = false;
        }

        if (controller != null && PlayerInRange() && InputHelpers.IsPressed(controller.inputDevice, fireButton, out bool isPressed, activationThreshold) && isPressed)
        {
            FireCannon();
        }
    }

    bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= fireRange;
    }

    void FireCannon()
    {
        if (cannonballPrefab != null && firePoint != null)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
            }
        }
    }
}

public class Cannonball : MonoBehaviour
{
    public GameObject impactVFX;
    public float explosionRadius = 5f;
    public float explosionForce = 500f;

    void OnCollisionEnter(Collision collision)
    {
        if (impactVFX != null)
        {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
        }

        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
