using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.VR;

public class PMJoinRoomLeaveRoomButton : MonoBehaviour
{
    public CodeManager manager;
    public string HandTag = "HandTag";
    private Color oldColor;
    private string RoomCodee;
    public bool joinroom;
    public bool leaveroom;

    private void Start()
    {
        oldColor = gameObject.GetComponent<Renderer>().material.color;
    }
    private IEnumerator color()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<Renderer>().material.color = oldColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == HandTag)
        {
            StartCoroutine(color());
            if (leaveroom)
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
            if (joinroom)
            {
                RoomCodee = manager.NewRoomThingy;
                
                int maxPlayers = 10;
                PhotonVRManager.JoinPrivateRoom(RoomCodee, maxPlayers);
            }
        }
    }
}
