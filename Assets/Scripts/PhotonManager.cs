using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Conecta a Photon
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon!");
        PhotonNetwork.JoinLobby(); // unete al lobby
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Sala1", new Photon.Realtime.RoomOptions(), default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Jugador conectado a la sala");
        // Spawnear al jugador cuando entras a la sala
        PhotonNetwork.Instantiate("JugadorPrefab", Vector3.zero, Quaternion.identity);
    }
}
