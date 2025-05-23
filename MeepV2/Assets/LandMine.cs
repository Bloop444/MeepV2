using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LandMine : MonoBehaviourPun
{
    public float beepInterval = 10f;
    public float flashDuration = 0.5f;
    public Material beepMaterial;
    public Material originalMaterial;
    public Renderer mineRenderer;

    public GameObject explosionPrefab; // Must be in Resources folder
    public float explosionLifeTime = 5f;

    public Transform[] teleportTargets;
    public float teleportDelay = 2f;
    public float reactivationDelay = 3f;

    public Transform playerToTeleport; // 👈 Drag your Gorilla Rig root here
    public Light activationLight;
    public AudioSource audioSource;
    public AudioClip beepClip;
    public AudioClip triggerClip;
    public AudioClip explosionClip;

    private bool isCoolingDown = false;
    private PhotonView pv;

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
            StartCoroutine(FlashBeepMaterial());

        if (audioSource != null && beepClip != null)
            audioSource.PlayOneShot(beepClip);
    }

    IEnumerator FlashBeepMaterial()
    {
        mineRenderer.material = beepMaterial;
        yield return new WaitForSeconds(flashDuration);
        mineRenderer.material = originalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient || isCoolingDown)
            return;

        PhotonView hitView = other.GetComponentInParent<PhotonView>();
        PhotonView playerView = playerToTeleport?.GetComponent<PhotonView>();

        if (hitView != null && playerView != null && hitView.ViewID == playerView.ViewID)
        {
            isCoolingDown = true;
            int randomIndex = Random.Range(0, teleportTargets.Length);
            Vector3 targetPos = teleportTargets[randomIndex].position;

            pv.RPC("ActivateMine", RpcTarget.All);
            playerView.RPC("TeleportTo", playerView.Owner, targetPos);
        }
    }

    [PunRPC]
    void ActivateMine()
    {
        if (activationLight != null)
        {
            activationLight.enabled = true;
            StartCoroutine(DisableLight());
        }

        if (audioSource != null && triggerClip != null)
            audioSource.PlayOneShot(triggerClip);

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject explosion = PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity);
            StartCoroutine(DestroyExplosionAfterTime(explosion, explosionLifeTime));
        }

        StartCoroutine(RearmMine());
    }

    IEnumerator DisableLight()
    {
        yield return new WaitForSeconds(0.3f);
        activationLight.enabled = false;
    }

    IEnumerator RearmMine()
    {
        yield return new WaitForSeconds(teleportDelay + reactivationDelay);
        isCoolingDown = false;
    }

    [PunRPC]
    void TeleportTo(Vector3 position)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(TeleportRoutine(position));
        }
    }

    IEnumerator TeleportRoutine(Vector3 targetPosition)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
            c.enabled = false;

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.1f);

        foreach (var c in colliders)
            c.enabled = true;
    }

    IEnumerator DestroyExplosionAfterTime(GameObject explosion, float time)
    {
        yield return new WaitForSeconds(time);
        if (explosion != null)
            PhotonNetwork.Destroy(explosion);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
