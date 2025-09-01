using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkGame : MonoBehaviourPunCallbacks
{
    public string playerPrefabName = "Player";
    public Transform[] spawnPoints;

    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            pos = sp.position; rot = sp.rotation;
        }

        PhotonNetwork.Instantiate(playerPrefabName, pos, rot);
    }
}

