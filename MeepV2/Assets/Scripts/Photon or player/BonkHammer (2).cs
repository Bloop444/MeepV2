using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BonkHammer : MonoBehaviourPun
{
    [Header("This script was made by Keo.cs")]
    [Header("You do not have to give credits")]
    public AudioSource BonkSound;

    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            return;
        }
        else
        {
            photonView.RPC("PlayBonk", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayBonk()
    {
        BonkSound.Play();
    }
}
