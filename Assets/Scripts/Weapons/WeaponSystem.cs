using UnityEngine;
using Photon.Pun;

/// <summary>
/// Weapon System - Handles weapon firing, reloading, and switching
/// Free Fire Style Weapons
/// </summary>
public class WeaponSystem : MonoBehaviourPunCallbacks
{
    [Header("Weapon Settings")]
    public Gun[] weapons;
    private int currentWeaponIndex = 0;
    private Gun currentWeapon;

    [Header("Firing")]
    public float fireRate = 0.1f;
    private float lastFireTime = 0f;
    public Transform firePoint;

    private void Start()
    {
        if (weapons.Length > 0)
        {
            currentWeapon = weapons[0];
            currentWeapon.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

    public void Fire()
    {
        if (currentWeapon == null) return;
        if (Time.time - lastFireTime < fireRate) return;
        if (currentWeapon.GetAmmo() <= 0) return;

        lastFireTime = Time.time;
        currentWeapon.Fire(firePoint);
        photonView.RPC("RPC_Fire", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_Fire()
    {
        if (currentWeapon != null)
            currentWeapon.PlayFireAnimation();
    }

    public void Reload()
    {
        if (currentWeapon == null) return;
        photonView.RPC("RPC_Reload", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Reload()
    {
        if (currentWeapon != null)
            currentWeapon.Reload();
    }

    public void SwitchWeapon()
    {
        if (weapons.Length <= 1) return;

        currentWeapon.gameObject.SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.gameObject.SetActive(true);

        Debug.Log($"[WeaponSystem] Switched to {currentWeapon.GetWeaponName()}");
    }

    public Gun GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void AddAmmo(int amount)
    {
        if (currentWeapon != null)
            currentWeapon.AddAmmo(amount);
    }
}

[System.Serializable]
public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public string weaponName = "Assault Rifle";
    public float damage = 25f;
    public float fireRate = 0.1f;
    public float reloadTime = 2f;
    public int maxAmmo = 30;
    private int currentAmmo;

    [Header("Firing")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    public ParticleSystem muzzleFlash;

    [Header("Effects")]
    public AudioClip fireSound;
    public AudioClip reloadSound;

    private AudioSource audioSource;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire(Transform shootPoint)
    {
        if (currentAmmo <= 0) return;
        if (isReloading) return;

        currentAmmo--;

        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = shootPoint.forward * bulletSpeed;
        }

        if (muzzleFlash != null)
            muzzleFlash.Play();

        PlayFireAnimation();
    }

    public void PlayFireAnimation()
    {
        transform.localPosition -= new Vector3(0, 0, 0.05f);
        Invoke("ResetRecoil", 0.05f);
    }

    private void ResetRecoil()
    {
        transform.localPosition = Vector3.zero;
    }

    public void Reload()
    {
        if (isReloading) return;
        if (currentAmmo == maxAmmo) return;
        StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log($"[Gun] {weaponName} reloaded!");
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public float GetDamage()
    {
        return damage;
    }
}