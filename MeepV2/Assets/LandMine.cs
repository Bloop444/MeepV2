using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LandMine : MonoBehaviourPun
{
    public float beepInterval = 10f;
    public float flashDuration = 0.5f;
    public Material beepMaterial;
    public Material originalMaterial;
    public Renderer mineRenderer;
    public GameObject explosionPrefab;
    public float explosionLifeTime = 5f;
    public float explosionRadius = 5f;
    public float triggerDelay = 2f;
    public float teleportDelay = 1f;
    public float reactivationDelay = 3f;
    public float colliderDisableTime = 0.1f;

    public Light activationLight;
    public AudioSource audioSource;
    public AudioClip beepClip;
    public AudioClip triggerClip;
    public AudioClip explosionClip;

    public List<Transform> playersToTeleport = new List<Transform>();
    public List<Transform> teleportTargets = new List<Transform>();
    public List<Collider> teleportColliders = new List<Collider>();

    private PhotonView pv;
    private bool isCoolingDown = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        StartCoroutine(BeepLoop());
    }

    IEnumerator BeepLoop()
    {
        while (true)
        {
            FlashBeep();
            yield return new WaitForSeconds(beepInterval);
        }
    }

    void FlashBeep()
    {
        if (mineRenderer != null)
            StartCoroutine(FlashMaterial());

        if (audioSource != null && beepClip != null)
            audioSource.PlayOneShot(beepClip);
    }

    IEnumerator FlashMaterial()
    {
        mineRenderer.material = beepMaterial;
        yield return new WaitForSeconds(flashDuration);
        mineRenderer.material = originalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient || isCoolingDown)
            return;

        if (other.CompareTag("Body") || other.CompareTag("HandTag"))
        {
            isCoolingDown = true;
            pv.RPC("TriggerMine", RpcTarget.All);
        }
    }

    [PunRPC]
    void TriggerMine()
    {
        if (activationLight != null)
            activationLight.enabled = true;

        if (audioSource != null && triggerClip != null)
            audioSource.PlayOneShot(triggerClip);

        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        yield return new WaitForSeconds(triggerDelay);

        if (audioSource != null && explosionClip != null)
            audioSource.PlayOneShot(explosionClip);

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject explosion = PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity);
            StartCoroutine(DestroyExplosion(explosion));
        }

        yield return new WaitForSeconds(teleportDelay);

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playersToTeleport.Count && i < teleportTargets.Count; i++)
            {
                Transform player = playersToTeleport[i];
                Transform target = teleportTargets[i];
                PhotonView targetPV = player.GetComponent<PhotonView>();
                if (targetPV != null)
                    pv.RPC("TeleportPlayer", RpcTarget.All, targetPV.ViewID, target.position);
            }
        }

        if (activationLight != null)
            activationLight.enabled = false;

        yield return new WaitForSeconds(reactivationDelay);
        isCoolingDown = false;
    }

    [PunRPC]
    void TeleportPlayer(int viewID, Vector3 destination)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view != null)
            StartCoroutine(HandleTeleport(view.transform, destination));
    }

    IEnumerator HandleTeleport(Transform player, Vector3 destination)
    {
        foreach (var col in teleportColliders)
            if (col != null) col.enabled = false;

        player.position = destination;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(colliderDisableTime);

        foreach (var col in teleportColliders)
            if (col != null) col.enabled = true;
    }

    IEnumerator DestroyExplosion(GameObject obj)
    {
        yield return new WaitForSeconds(explosionLifeTime);
        if (obj != null)
            PhotonNetwork.Destroy(obj);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
