using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [Header("Game Settings")]
    public int maxPlayers = 50;
    public float matchDuration = 1800f; // 30 minutes
    public Vector3 mapCenter = Vector3.zero;
    public float initialSafeZoneRadius = 2000f;

    [Header("Match State")]
    private float matchTimer;
    private float currentSafeZoneRadius;
    private int currentPlayers;
    private int playersAlive;
    private bool matchStarted = false;
    private bool matchEnded = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        matchTimer = matchDuration;
        currentSafeZoneRadius = initialSafeZoneRadius;
        currentPlayers = PhotonNetwork.PlayerList.Length;
        playersAlive = currentPlayers;
        
        Debug.Log($"[GAME] Match started with {currentPlayers} players");
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (matchEnded) return;

        matchTimer -= Time.deltaTime;

        // Shrink safe zone
        if (matchTimer > 0)
        {
            float shrinkPercentage = 1f - (matchTimer / matchDuration);
            currentSafeZoneRadius = initialSafeZoneRadius * (1f - (shrinkPercentage * 0.9f));
        }
        else
        {
            EndMatch();
        }

        // Check alive players
        UpdatePlayerCount();
    }

    void UpdatePlayerCount()
    {
        playersAlive = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Count alive players
            playersAlive++;
        }
    }

    void EndMatch()
    {
        if (matchEnded) return;
        matchEnded = true;
        
        Debug.Log("[GAME] Match Ended!");
        photonView.RPC("ShowMatchEnd", RpcTarget.All);
    }

    [PunRPC]
    void ShowMatchEnd()
    {
        Debug.Log("[UI] Showing match end screen");
        // TODO: Show victory/defeat screen
        
        Invoke("ReturnToLobby", 5f);
    }

    void ReturnToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public float GetSafeZoneRadius()
    {
        return currentSafeZoneRadius;
    }

    public Vector3 GetSafeZoneCenter()
    {
        return mapCenter;
    }

    public float GetMatchTimeRemaining()
    {
        return Mathf.Max(0, matchTimer);
    }

    public int GetPlayersAlive()
    {
        return playersAlive;
    }

    public int GetTotalPlayers()
    {
        return currentPlayers;
    }

    public bool IsMatchStarted()
    {
        return matchStarted;
    }

    public bool IsMatchEnded()
    {
        return matchEnded;
    }
}
