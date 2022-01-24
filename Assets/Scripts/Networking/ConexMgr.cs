using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
public class ConexMgr : MonoBehaviourPunCallbacks
{
    public TMP_InputField txtNick;
    public TextMeshProUGUI estado;
    public GameObject panelConex, panelRooms;

    public void Connect2Master()
    {
        PhotonNetwork.NickName = txtNick.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        panelConex.SetActive(false);
        estado.gameObject.SetActive(true);
    }

    private void OnGUI()
    {
        if (estado && estado.gameObject.activeSelf)
            estado.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Desconectado, causa: " + cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Invoke(nameof(LoadRoomPanel),1);
    }

    void LoadRoomPanel()
    {
        panelRooms.SetActive(true);
        estado.transform.parent.gameObject.SetActive(false);
        PhotonNetwork.JoinLobby();
    }
}
