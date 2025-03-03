using UnityEngine;

public class CannonballExplosion : MonoBehaviour
{
    public GameObject explosionVFX;
    public AudioClip explosionSFX;
    public float explosionRadius = 5f;
    public float explosionForce = 500f;
    public float upliftModifier = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector3 explosionDirection = (rb.transform.position - transform.position).normalized;
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upliftModifier, ForceMode.Impulse);
            }
        }

        if (explosionSFX != null)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
        }

        if (explosionVFX != null)
        {
            GameObject explosionEffect = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            ParticleSystem ps = explosionEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(explosionEffect, ps.main.duration);
            }
        }

        Destroy(gameObject);
    }
}
