using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CodeManager : MonoBehaviour
{
    [Header("credit primal monke/rtx4090 or you arnt sigma")]
    [Space]
    public TextMeshPro InRoomText;
    [Space]
    [Header("dont touch this")]
    public string NewRoomThingy;
    public TextMeshPro NewRoomText;

    public void Update()
    {
        NewRoomText.text = NewRoomThingy;
        if (PhotonNetwork.InRoom)
        {
            InRoomText.text = PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            InRoomText.text = "Not In Room Press Enter To Join Public.";
        }
    }    
}
