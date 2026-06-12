using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public InputField playerNameInput;
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Text versionText;
    public Text connectionStatusText;
    public GameObject loadingPanel;

    void Start()
    {
        versionText.text = "BATTLE ARENA 2018 v1.0";
        connectionStatusText.text = "Offline";
        
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void PlayGame()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
            playerName = "Player_" + Random.Range(1000, 9999);

        PhotonNetwork.NickName = playerName;
        
        Debug.Log($"[MENU] Connecting to Photon as {playerName}...");
        
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
        
        PhotonNetwork.ConnectUsingSettings();
    }

    void OpenSettings()
    {
        Debug.Log("[MENU] Opening Settings...");
        // TODO: Show settings panel
    }

    void QuitGame()
    {
        Debug.Log("[MENU] Quitting game...");
        Application.Quit();
    }

    public override void OnConnected()
    {
        Debug.Log("[PHOTON] Connected to Photon!");
        connectionStatusText.text = "Connected";
    }

    public override void OnConnectedToPhoton()
    {
        Debug.Log("[PHOTON] Connected to Photon server!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"[PHOTON] Disconnected: {cause}");
        connectionStatusText.text = "Offline";
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[PHOTON] Joined Lobby!");
        connectionStatusText.text = "In Lobby";
        
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        
        // Go to Lobby scene
        SceneManager.LoadScene("Lobby");
    }
}
