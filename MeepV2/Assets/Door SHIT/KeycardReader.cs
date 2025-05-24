using UnityEngine;
using Photon.Pun;
using System.Collections;

public class KeycardReader : MonoBehaviourPun
{
    public KeycardColor acceptedColor;
    public GameObject assignedKeycard;
    public SecureDoor targetDoor;

    public AudioSource audioSource;
    public AudioClip acceptSound;
    public AudioClip denySound;

    private bool canTap = true;
    public float cooldownTime = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == assignedKeycard)
        {
            Keycard card = other.GetComponent<Keycard>();
            if (card != null)
            {
                if (card.cardColor == acceptedColor)
                {
                    if (canTap)
                    {
                        Debug.Log("Access granted: correct keycard and color");
                        PlayOneShot(acceptSound);

                        if (targetDoor.photonView.IsMine)
                            targetDoor.photonView.RPC("ToggleDoor", RpcTarget.All);

                        StartCoroutine(ResetTapCooldown());
                    }
                }
                else
                {
                    Debug.Log("Access denied: correct card, wrong color");
                    PlayOneShot(denySound);
                    targetDoor.PlayWarning();
                }
            }
        }
        else if (other.CompareTag("Keycard"))
        {
            Debug.Log("Access denied: wrong keycard object");
            PlayOneShot(denySound);
            targetDoor.PlayWarning();
        }
    }

    private IEnumerator ResetTapCooldown()
    {
        canTap = false;
        yield return new WaitForSeconds(cooldownTime);
        canTap = true;
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.loop = false;
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(clip);
        }
    }
}
