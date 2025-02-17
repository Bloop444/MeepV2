using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RankDisplaySync : MonoBehaviourPun, IPunObservable
{
    public TMP_Text rankText;
    private XP_Manager xpManager;
    private string currentRank;

    private void Start()
    {
        if (photonView.IsMine)
        {
            xpManager = FindObjectOfType<XP_Manager>();
            if (xpManager != null)
            {
                currentRank = xpManager.curRank.ToString();
                UpdateRankDisplay();
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine && xpManager != null)
        {
            string newRank = xpManager.curRank.ToString();
            if (currentRank != newRank)
            {
                currentRank = newRank;
                UpdateRankDisplay();
            }
        }
    }

    private void UpdateRankDisplay()
    {
        rankText.text = currentRank;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentRank);
        }
        else
        {
            currentRank = (string)stream.ReceiveNext();
            UpdateRankDisplay();
        }
    }
}
