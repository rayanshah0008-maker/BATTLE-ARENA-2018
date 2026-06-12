using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPun
{
    public float maxHealth = 100f;
    public float currentHealth;
    
    public float maxArmor = 100f;
    public float currentArmor;
    
    private int killCount = 0;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentArmor = 0;
    }

    [PunRPC]
    public void TakeDamage(float damage, string damageSource = "Unknown")
    {
        if (isDead) return;
        if (!photonView.IsMine) return;

        float actualDamage = damage;
        float armorDamage = 0;

        // Armor reduces damage by 50%
        if (currentArmor > 0)
        {
            armorDamage = damage * 0.5f;
            if (armorDamage > currentArmor)
            {
                armorDamage = currentArmor;
                actualDamage = damage - armorDamage;
                currentArmor = 0;
            }
            else
            {
                currentArmor -= armorDamage;
                actualDamage = damage * 0.5f;
            }
        }

        currentHealth -= actualDamage;

        Debug.Log($"[DAMAGE] {damageSource} dealt {actualDamage} damage. Health: {currentHealth}, Armor: {currentArmor}");

        if (currentHealth <= 0)
        {
            Die(damageSource);
        }
    }

    void Die(string killedBy = "Unknown")
    {
        if (isDead) return;
        isDead = true;
        currentHealth = 0;

        Debug.Log($"[DEATH] {photonView.Owner.NickName} was killed by {killedBy}");

        // Notify other players
        photonView.RPC("ShowDeathScreen", RpcTarget.All, killedBy);
        
        // Disable player
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        
        // Respawn after delay or go to lobby
        Invoke("ReturnToLobby", 5f);
    }

    [PunRPC]
    void ShowDeathScreen(string killedBy)
    {
        if (photonView.IsMine)
        {
            Debug.Log($"You were killed by {killedBy}");
            // TODO: Show death screen UI
        }
    }

    void ReturnToLobby()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        
        Debug.Log($"[HEAL] Healed {amount}. Current Health: {currentHealth}");
    }

    public void AddArmor(float amount)
    {
        if (isDead) return;
        currentArmor += amount;
        if (currentArmor > maxArmor)
            currentArmor = maxArmor;
        
        Debug.Log($"[ARMOR] Added {amount} armor. Current Armor: {currentArmor}");
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public float GetArmorPercent()
    {
        return currentArmor / maxArmor;
    }

    public void AddKill()
    {
        killCount++;
        Debug.Log($"[KILL] Kill count: {killCount}");
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
