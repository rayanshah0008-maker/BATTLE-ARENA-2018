using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// Main Game Manager - Handles core game logic, player management, and game flow
/// Free Fire Style Battle Royale
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Game Settings")]
    public int maxPlayers = 50;
    public float gameStartDelay = 5f;
    public float safeZoneShrinkInterval = 60f;
    public float safeZoneDamage = 5f;

    [Header("References")]
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public SafeZoneManager safeZoneManager;
    public UIManager uiManager;
    public AudioManager audioManager;

    private static GameManager instance;
    private int playersAlive;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private float gameTimer = 0f;
    private int winnerPhotonID = -1;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        Debug.Log("[GameManager] Game initialized");
        playersAlive = PhotonNetwork.PlayerList.Length;
        
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("StartGame", gameStartDelay);
        }
    }

    private void Update()
    {
        if (!gameStarted) return;

        gameTimer += Time.deltaTime;
    }

    public void StartGame()
    {
        gameStarted = true;
        photonView.RPC("RPC_StartGame", RpcTarget.All);
    }

    [PunRPC]
    void RPC_StartGame()
    {
        Debug.Log("[GameManager] Game Started!");
        gameStarted = true;
        
        if (uiManager != null)
            uiManager.ShowGameHUD();
    }

    public void PlayerEliminated(Player eliminatedPlayer)
    {
        playersAlive--;
        Debug.Log($"[GameManager] Player eliminated: {eliminatedPlayer.NickName}. Players alive: {playersAlive}");

        photonView.RPC("RPC_PlayerEliminated", RpcTarget.All, eliminatedPlayer.ActorNumber);
    }

    [PunRPC]
    void RPC_PlayerEliminated(int playerActorNumber)
    {
        if (uiManager != null)
        {
            uiManager.AddKillNotification("Player Eliminated");
        }
    }

    public void EndGame()
    {
        gameEnded = true;
        winnerPhotonID = PhotonNetwork.LocalPlayer.ActorNumber;

        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            Debug.Log("[GameManager] YOU WON! Victory Royale!");
            if (uiManager != null)
                uiManager.ShowVictoryScreen();
        }

        photonView.RPC("RPC_EndGame", RpcTarget.All, winnerPhotonID);
    }

    [PunRPC]
    void RPC_EndGame(int winner)
    {
        gameEnded = true;
        Debug.Log($"[GameManager] Game Ended! Winner: {winner}");
    }

    public int GetPlayersAlive()
    {
        return playersAlive;
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public float GetGameTime()
    {
        return gameTimer;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[GameManager] Player left: {otherPlayer.NickName}");
        playersAlive--;
    }
}