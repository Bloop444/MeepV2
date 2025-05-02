using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class Option1 : MonoBehaviourPunCallbacks
{
    [Header("credit primal monke or else")]
    public string HandTag = "HandTag";
    public float delay = 1f;
    public int playersPerRoom = 10;
    private Color oldColor;

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
        if (other.CompareTag(HandTag))
        {
            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(color());
                if (!PhotonNetwork.InRoom)
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                else if (PhotonNetwork.CurrentRoom.PlayerCount < playersPerRoom)
                {
                    StartCoroutine(LeaveAndRejoinRoom());
                }
            }
        }
    }

    private IEnumerator LeaveAndRejoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        yield return new WaitForSeconds(delay);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        
        Debug.Log("Joined a room successfully.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = GenerateRandomRoomName();
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = (byte)playersPerRoom };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       
        Debug.LogError("Room creation failed: " + message);
    }

    private string GenerateRandomRoomName()
    {
        const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int roomNameLength = 4;
        string roomName = string.Empty;

        for (int i = 0; i < roomNameLength; i++)
        {
            roomName += characters[Random.Range(0, characters.Length)];
        }

        return roomName;
    }
}
