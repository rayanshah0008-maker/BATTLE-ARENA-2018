using UnityEngine;
using Photon.Pun;

/// <summary>
/// Player Stats - Tracks health, armor, kills, and other player statistics
/// </summary>
public class PlayerStats : MonoBehaviourPunCallbacks
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Armor Settings")]
    public float maxArmor = 100f;
    private float currentArmor = 0f;
    public float armorDamageReduction = 0.4f;

    [Header("Statistics")]
    private int kills = 0;
    private int deaths = 0;
    private float survivalTime = 0f;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isDead)
            survivalTime += Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float armorDamage = damage * armorDamageReduction;
        float healthDamage = damage - armorDamage;

        if (currentArmor > 0)
        {
            currentArmor -= armorDamage;
            if (currentArmor < 0)
            {
                healthDamage += Mathf.Abs(currentArmor);
                currentArmor = 0;
            }
        }
        else
        {
            healthDamage = damage;
        }

        currentHealth -= healthDamage;

        Debug.Log($"[PlayerStats] Took {damage} damage. Health: {currentHealth}, Armor: {currentArmor}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            deaths++;
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void AddArmor(float amount)
    {
        currentArmor = Mathf.Min(currentArmor + amount, maxArmor);
    }

    public void AddKill()
    {
        kills++;
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetArmor() => currentArmor;
    public float GetMaxArmor() => maxArmor;
    public int GetKills() => kills;
    public int GetDeaths() => deaths;
    public float GetSurvivalTime() => survivalTime;
    public bool IsDead() => isDead;
}