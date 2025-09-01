using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class MainMenuLauncher : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button connectionButton;
    [SerializeField] TextMeshProUGUI statusText;   // opcional

    [Header("Flow")]
    [SerializeField] string gameSceneName = "Game";

    [Header("Nickname Rules")]
    [SerializeField, Range(1, 20)] int minChars = 3;
    [SerializeField, Range(3, 20)] int maxChars = 12;

    const string NickKey = "playerNickname";
    string nickname = "";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        connectionButton.onClick.AddListener(TryConnect);
        inputField.onValueChanged.AddListener(OnNicknameChanged);
        inputField.onSubmit.AddListener(_ => TryConnect());

        inputField.characterLimit = maxChars;
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;

        nickname = PlayerPrefs.GetString(NickKey, "");
        inputField.text = Format(nickname);
        Evaluate();
        SetStatus("Ingresá tu nickname");
    }

    string Format(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Trim().Replace(" ", "_").ToUpperInvariant();

        var sb = new StringBuilder(s.Length);
        foreach (char c in s)
        {
            if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_')
                sb.Append(c);
        }
        return sb.ToString();
    }

    void OnNicknameChanged(string _)
    {
        var caret = inputField.caretPosition;
        inputField.text = Format(inputField.text);
        inputField.caretPosition = Mathf.Min(caret, inputField.text.Length);

        nickname = inputField.text;
        Evaluate();
    }

    void Evaluate()
    {
        bool ok = nickname.Length >= minChars;
        connectionButton.interactable = ok;
        SetStatus(ok ? "Listo para conectar" : $"Mínimo {minChars} caracteres");
    }

    void TryConnect()
    {
        if (!connectionButton.interactable) return;

        PhotonNetwork.NickName = nickname;
        PlayerPrefs.SetString(NickKey, nickname);
        PlayerPrefs.Save();

        connectionButton.interactable = false;
        SetStatus("Conectando a Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // ==== Callbacks PUN ====
    public override void OnConnectedToMaster()
    {
        SetStatus("Conectado al Master. Entrando al lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SetStatus("En lobby. Creando/uniendo sala...");
        var opts = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.JoinOrCreateRoom("Sala1", opts, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"[PHOTON] Nick: {PhotonNetwork.NickName} | ID: {PhotonNetwork.LocalPlayer.ActorNumber}");
        SetStatus("Cargando escena de juego...");
        PhotonNetwork.LoadLevel(gameSceneName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetStatus($"Desconectado: {cause}");
        connectionButton.interactable = true;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetStatus($"Falló JoinRoom: {message}");
        connectionButton.interactable = true;
    }

    void SetStatus(string msg)
    {
        if (statusText) statusText.text = msg;
        Debug.Log(msg);
    }
}
