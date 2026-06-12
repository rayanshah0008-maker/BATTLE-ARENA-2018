using UnityEngine;
using Photon.Pun;

/// <summary>
/// Item Pickup - Handles item pickup and collection
/// </summary>
public class ItemPickup : MonoBehaviourPunCallbacks
{
    [Header("Pickup Settings")]
    public Item item;
    public float pickupRadius = 2f;
    public float pickupAnimationDuration = 0.5f;

    [Header("Visual")]
    public Animator animator;
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;

    private bool isPickedUp = false;
    private SphereCollider pickupCollider;

    private void Start()
    {
        if (item == null)
            item = GetComponent<Item>();

        // Create pickup collider if not exists
        pickupCollider = GetComponent<SphereCollider>();
        if (pickupCollider == null)
        {
            pickupCollider = gameObject.AddComponent<SphereCollider>();
        }

        pickupCollider.radius = pickupRadius;
        pickupCollider.isTrigger = true;

        Debug.Log($"[ItemPickup] {item.GetItemName()} ready for pickup");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            PickupItem(player);
        }
    }

    /// <summary>
    /// Pickup item (take it into inventory)
    /// </summary>
    public void PickupItem(PlayerController player)
    {
        if (isPickedUp) return;

        isPickedUp = true;

        // Network sync
        if (photonView != null)
            photonView.RPC("RPC_PickupItem", RpcTarget.All, player.GetPlayer().ActorNumber);
        else
            PerformPickup(player);
    }

    [PunRPC]
    void RPC_PickupItem(int playerActorNumber)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && player.GetPlayer().ActorNumber == playerActorNumber)
        {
            PerformPickup(player);
        }
    }

    /// <summary>
    /// Actually perform the pickup
    /// </summary>
    private void PerformPickup(PlayerController player)
    {
        // Add to inventory
        InventorySystem inventory = player.GetInventorySystem();
        if (inventory != null)
        {
            inventory.AddItem(item);
            Debug.Log($"[ItemPickup] {player.GetPlayer().NickName} picked up {item.GetItemName()}");
        }

        // Play pickup effect
        PlayPickupEffect();

        // Destroy item
        Destroy(gameObject, 0.1f);
    }

    /// <summary>
    /// Play pickup effect and animation
    /// </summary>
    private void PlayPickupEffect()
    {
        if (pickupEffect != null)
        {
            pickupEffect.Play();
        }

        if (animator != null)
        {
            animator.SetTrigger("Pickup");
        }

        if (pickupSound != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
                audio.PlayOneShot(pickupSound);
        }
    }

    /// <summary>
    /// Visualize pickup radius in editor
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
