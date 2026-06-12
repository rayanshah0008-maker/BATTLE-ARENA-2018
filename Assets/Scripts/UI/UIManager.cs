using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI Manager - Handles all UI elements and screens
/// Free Fire Style UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Main Menu")]
    public Canvas mainMenuCanvas;
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Lobby")]
    public Canvas lobbyCanvas;
    public Button soloButton;
    public Button duoButton;
    public Button squadButton;
    public TextMeshProUGUI playerCountText;

    [Header("Game HUD")]
    public Canvas gameHUD;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI playersAliveText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI weaponNameText;

    [Header("End Game Screens")]
    public Canvas victoryCanvas;
    public Canvas defeatCanvas;
    public TextMeshProUGUI finalStatsText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClick);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClick);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClick);

        ShowMainMenu();
    }

    private void Update()
    {
        if (gameManager == null || !gameManager.IsGameStarted()) return;
        UpdateGameHUD();
    }

    public void ShowMainMenu()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(true);
        if (lobbyCanvas != null) lobbyCanvas.gameObject.SetActive(false);
        if (gameHUD != null) gameHUD.gameObject.SetActive(false);
    }

    public void ShowLobby()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(false);
        if (lobbyCanvas != null) lobbyCanvas.gameObject.SetActive(true);
    }

    public void ShowGameHUD()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(false);
        if (lobbyCanvas != null) lobbyCanvas.gameObject.SetActive(false);
        if (gameHUD != null) gameHUD.gameObject.SetActive(true);
    }

    private void UpdateGameHUD()
    {
        PlayerController localPlayer = FindObjectOfType<PlayerController>();
        if (localPlayer == null) return;

        PlayerStats stats = localPlayer.GetPlayerStats();
        WeaponSystem weapons = localPlayer.GetWeaponSystem();

        if (healthText != null && stats != null)
            healthText.text = $"❤️ {stats.GetHealth()}/{stats.GetMaxHealth()}";

        if (armorText != null && stats != null)
            armorText.text = $"🛡️ {stats.GetArmor()}/{stats.GetMaxArmor()}";

        if (ammoText != null && weapons != null)
        {
            Gun gun = weapons.GetCurrentWeapon();
            if (gun != null)
                ammoText.text = $"💥 {gun.GetAmmo()}";
        }

        if (weaponNameText != null && weapons != null)
        {
            Gun gun = weapons.GetCurrentWeapon();
            if (gun != null)
                weaponNameText.text = gun.GetWeaponName();
        }

        if (playersAliveText != null && gameManager != null)
            playersAliveText.text = $"Players: {gameManager.GetPlayersAlive()}";

        if (gameTimerText != null && gameManager != null)
        {
            float time = gameManager.GetGameTime();
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;
            gameTimerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void AddKillNotification(string killedPlayerName)
    {
        Debug.Log($"[UIManager] Kill: {killedPlayerName}");
    }

    public void ShowVictoryScreen()
    {
        if (gameHUD != null) gameHUD.gameObject.SetActive(false);
        if (victoryCanvas != null) victoryCanvas.gameObject.SetActive(true);
    }

    public void ShowDefeatScreen()
    {
        if (gameHUD != null) gameHUD.gameObject.SetActive(false);
        if (defeatCanvas != null) defeatCanvas.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        ShowLobby();
        Debug.Log("[UIManager] Play button clicked");
    }

    private void OnSettingsClick()
    {
        Debug.Log("[UIManager] Settings clicked");
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}