using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPun
{
    public float speed = 5f;
    [SerializeField] private TextMeshProUGUI nicknameText;  // Asignar en Inspector

    void Start()
    {
        // Solo cambiar el texto en el dueño del objeto
        if (nicknameText != null)
        {
            nicknameText.text = photonView.Owner.NickName;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Solo controlamos nuestro propio jugador

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
    }
}


