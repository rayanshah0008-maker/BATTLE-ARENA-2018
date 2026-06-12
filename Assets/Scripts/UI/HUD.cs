using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HUD : MonoBehaviour
{
    [Header("Health & Armor")]
    public Text healthText;
    public Text armorText;
    public Image healthBar;
    public Image armorBar;

    [Header("Ammo & Weapons")]
    public Text ammoText;
    public Text weaponNameText;

    [Header("Game Info")]
    public Text killsText;
    public Text playersAliveText;
    public Text timerText;
    public Text modeText;

    [Header("Mini Map")]
    public RawImage miniMap;
    public Image playerMarkerOnMap;

    private PlayerHealth playerHealth;
    private WeaponSystem weaponSystem;
    private GameManager gameManager;
    private PhotonView photonView;
    private int kills = 0;

    void Start()
    {
        photonView = FindObjectOfType<PhotonView>();
        
        playerHealth = FindObjectOfType<PlayerHealth>();
        weaponSystem = FindObjectOfType<WeaponSystem>();
        gameManager = GameManager.instance;

        if (gameManager != null)
            modeText.text = "BATTLE ARENA 2018";
    }

    void Update()
    {
        if (playerHealth != null)
            UpdateHealthUI();

        if (weaponSystem != null)
            UpdateWeaponUI();

        if (gameManager != null)
            UpdateGameUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = $"HP: {playerHealth.currentHealth:F0}/{playerHealth.maxHealth:F0}";
        armorText.text = $"Armor: {playerHealth.currentArmor:F0}/{playerHealth.maxArmor:F0}";
        
        healthBar.fillAmount = playerHealth.GetHealthPercent();
        armorBar.fillAmount = playerHealth.GetArmorPercent();
    }

    void UpdateWeaponUI()
    {
        var currentWeapon = weaponSystem.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            weaponNameText.text = currentWeapon.name;
            ammoText.text = $"{currentWeapon.ammo}/{currentWeapon.maxAmmo}";
        }
        else
        {
            weaponNameText.text = "No Weapon";
            ammoText.text = "0/0";
        }
    }

    void UpdateGameUI()
    {
        if (playerHealth != null)
            killsText.text = $"Kills: {playerHealth.GetKillCount()}";
        
        playersAliveText.text = $"Players: {gameManager.GetPlayersAlive()}/{gameManager.GetTotalPlayers()}";
        
        float timeRemaining = gameManager.GetMatchTimeRemaining();
        int minutes = (int)timeRemaining / 60;
        int seconds = (int)timeRemaining % 60;
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }
}
