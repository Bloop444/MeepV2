using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VotingSystem : MonoBehaviour
{
    [Header("Script can be used for voting like yes or no or")]
    [Header("Script made my huhmonke but edited by beatboxvr")]
    [Header("For you webhook make sure u put ur discord webhook if u dont know how serach a yt vid")]
    [Header("Like which map people want")]
    [Space(2)]
    [Header("Webhook Link")]
    public string WebhookLink;
    [Header("Ur HandTag")]
    public string HandTag = "HandTag";
    private bool CanSend = true;
    [Header("What the message is gonna say")]
    public string WhatToVoteFor;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(HandTag) && CanSend
            )
        {
            string message = PhotonNetwork.NickName + " Voted for: " + WhatToVoteFor;
            StartCoroutine(sendWebhook(WebhookLink, message));
            CanSend = false;

            IEnumerator sendWebhook(string link, string message)
            {
                WWWForm form = new WWWForm();
                form.AddField("content", message);
                using (UnityWebRequest www = UnityWebRequest.Post(link, form))
                {
                    yield return www.SendWebRequest();
                }
                CanSend = false;
            }
        }
    }
}
