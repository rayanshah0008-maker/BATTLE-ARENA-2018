using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonSetup : MonoBehaviourPun
{
    public static PhotonSetup instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Photon setup will be done from MainMenu
        Debug.Log("[PHOTON] Setup ready");
    }

    public void CreateRoom(string roomName, byte maxPlayers)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log($"[PHOTON] Creating room: {roomName} (Max: {maxPlayers} players)");
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("[PHOTON] Joining random room...");
    }
}
