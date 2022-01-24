using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomMgr : MonoBehaviourPunCallbacks
{
    public Transform panelRooms, panelPlayers,  canvasLobby, canvasRoom;
    public GameObject prefabRoom, prefabPlayer, botonStart;
    public UIRoom selectedRoom;
    public TextMeshProUGUI roomName;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(!PhotonNetwork.InRoom)
        {
            foreach (UIRoom room in panelRooms.GetComponentsInChildren<UIRoom>())
                Destroy(room.gameObject);
            foreach (RoomInfo room in roomList)
                Instantiate(prefabRoom, panelRooms).GetComponent<UIRoom>().InitData(room.Name, room.PlayerCount);
        }
    }

    public void CrearRoom()
    {
        canvasLobby.gameObject.SetActive(false);
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("Room de " + PhotonNetwork.NickName, options);
    }

    public void SelectedRoom(UIRoom room)
    {
        selectedRoom?.Deselected();
        selectedRoom = room;
    }

    public void OnJoinClicked()
    {
        canvasLobby.gameObject.SetActive(false);
        if (selectedRoom)
            PhotonNetwork.JoinRoom(selectedRoom.roomName);
        else
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        canvasLobby.gameObject.SetActive(true);
    }

    public void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        canvasRoom.gameObject.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        canvasRoom.gameObject.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        PlayerListing();
        if (PhotonNetwork.IsMasterClient)
            botonStart.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerListing();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerListing();
    }

    void PlayerListing()
    {
        Player[] currPlayers = PhotonNetwork.PlayerList;
        foreach(TMPro.TextMeshProUGUI c in panelPlayers.GetComponentsInChildren<TextMeshProUGUI>())
            Destroy(c.gameObject);

        foreach (Player p in currPlayers)
            Instantiate(prefabPlayer, panelPlayers).GetComponent<TextMeshProUGUI>().text = p.NickName;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Mapa");
    }

    public void BtnSalir()
    {
        PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
