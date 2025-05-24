using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;

    public AudioClip explosionSound;
    public AudioClip pinPullSound;
    public AudioSource assignedAudioSource;

    public GameObject explosionVFXPrefab;
    public float vfxDuration = 2f;

    public Transform teleportTarget;
    public float colliderDisableDuration = 2f;

    public List<Collider> extraCollidersToDisable = new List<Collider>();
    public Rigidbody extraRb1;
    public Rigidbody extraRb2;

    public bool simulateTrigger = false;

    private AudioSource audioSource;
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private bool isArmed = false;
    private bool hasBeenUsed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.activated.AddListener(OnTriggerPulled);
    }

    void Update()
    {
        if (simulateTrigger && !isArmed && !hasBeenUsed)
        {
            simulateTrigger = false;
            OnTriggerPulled(new ActivateEventArgs());
        }
    }

    void OnTriggerPulled(ActivateEventArgs args)
    {
        if (isArmed || hasBeenUsed) return;

        hasBeenUsed = true;

        if (assignedAudioSource != null && pinPullSound != null)
        {
            assignedAudioSource.PlayOneShot(pinPullSound);
        }

        StartCoroutine(ExplodeSequence());
    }

    IEnumerator ExplodeSequence()
    {
        isArmed = true;
        rb.isKinematic = false;

        if (extraRb1 != null)
            extraRb1.isKinematic = false;

        if (extraRb2 != null)
            extraRb2.isKinematic = false;

        if (explosionSound && audioSource)
        {
            audioSource.clip = explosionSound;
            audioSource.Play();
        }

        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (explosionVFXPrefab)
        {
            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            Player player = hit.GetComponentInParent<Player>();
            if (player != null && player.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(TeleportAndDisableColliders(player));
            }
        }

        Destroy(gameObject);
    }

    IEnumerator TeleportAndDisableColliders(Player player)
    {
        if (teleportTarget != null)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.MovePosition(teleportTarget.position);
            }
            else
            {
                player.transform.position = teleportTarget.position;
            }
        }

        if (player.headCollider != null)
            player.headCollider.enabled = false;

        if (player.bodyCollider != null)
            player.bodyCollider.enabled = false;

        foreach (Collider col in extraCollidersToDisable)
        {
            if (col != null)
                col.enabled = false;
        }

        yield return new WaitForSeconds(colliderDisableDuration);

        if (player.headCollider != null)
            player.headCollider.enabled = true;

        if (player.bodyCollider != null)
            player.bodyCollider.enabled = true;

        foreach (Collider col in extraCollidersToDisable)
        {
            if (col != null)
                col.enabled = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
