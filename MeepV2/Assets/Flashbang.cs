using UnityEngine;
using System.Collections;
using Photon.Pun;
using easyInputs;

public class Flashbang : MonoBehaviourPunCallbacks
{
    public Rigidbody flashbangRigidbody;
    public Rigidbody secondaryRigidbody;
    public AudioSource audioSource;
    public AudioClip triggerClip;
    public AudioClip explosionClip;
    public GameObject flashEffect;
    public float flashDelay = 5f;
    public float flashDuration = 2f;
    public float activationRange = 5f;
    public bool testMode = false;

    private bool hasActivated = false;
    private bool isHeld = false;
    private bool lastTestModeState = false;

    void Start()
    {
        if (flashEffect != null)
            flashEffect.SetActive(false);
    }

    void Update()
    {
        if (!hasActivated && testMode && !lastTestModeState)
        {
            if (!photonView.IsMine)
                photonView.RequestOwnership();
            photonView.RPC("ActivateFlashbang", RpcTarget.All);
        }

        lastTestModeState = testMode;

        if (!hasActivated)
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand) || EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                if (!photonView.IsMine)
                    photonView.RequestOwnership();
                isHeld = true;
            }

            if (isHeld && (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) || EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand)))
            {
                if (!photonView.IsMine)
                    photonView.RequestOwnership();
                photonView.RPC("ActivateFlashbang", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void ActivateFlashbang()
    {
        if (hasActivated)
            return;

        hasActivated = true;
        StartCoroutine(HandleFlashbang());
    }

    IEnumerator HandleFlashbang()
    {
        if (flashbangRigidbody != null)
            flashbangRigidbody.isKinematic = false;

        if (secondaryRigidbody != null)
            secondaryRigidbody.isKinematic = false;

        if (audioSource != null && triggerClip != null)
            audioSource.PlayOneShot(triggerClip);

        yield return new WaitForSeconds(flashDelay);

        if (audioSource != null && explosionClip != null)
            audioSource.PlayOneShot(explosionClip);

        if (flashEffect != null)
            flashEffect.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        if (flashEffect != null)
            flashEffect.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}
