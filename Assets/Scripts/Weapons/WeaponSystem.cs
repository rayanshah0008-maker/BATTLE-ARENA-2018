using UnityEngine;
using Photon.Pun;

public class WeaponSystem : MonoBehaviourPun
{
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public float damage;
        public float fireRate;
        public int ammo;
        public int maxAmmo;
        public float range;
        public string type; // AR, Sniper, Shotgun, Pistol, SMG, Grenade
    }

    public Weapon[] weapons = new Weapon[6];
    private int currentWeaponIndex = -1;
    private float lastFireTime = 0f;
    private bool canShoot = true;

    void Start()
    {
        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        // Weapon 0: AR - M4
        weapons[0] = new Weapon 
        { 
            name = "M4", 
            damage = 25, 
            fireRate = 0.1f, 
            ammo = 0, 
            maxAmmo = 300, 
            range = 500f,
            type = "AR"
        };

        // Weapon 1: Sniper - AWM
        weapons[1] = new Weapon 
        { 
            name = "AWM", 
            damage = 86, 
            fireRate = 1.5f, 
            ammo = 0, 
            maxAmmo = 50, 
            range = 1000f,
            type = "Sniper"
        };

        // Weapon 2: Shotgun
        weapons[2] = new Weapon 
        { 
            name = "Combat Shotgun", 
            damage = 65, 
            fireRate = 0.8f, 
            ammo = 0, 
            maxAmmo = 32, 
            range = 30f,
            type = "Shotgun"
        };

        // Weapon 3: Pistol - Glock
        weapons[3] = new Weapon 
        { 
            name = "Glock", 
            damage = 15, 
            fireRate = 0.15f, 
            ammo = 30, 
            maxAmmo = 120, 
            range = 200f,
            type = "Pistol"
        };

        // Weapon 4: SMG - MP5
        weapons[4] = new Weapon 
        { 
            name = "MP5", 
            damage = 18, 
            fireRate = 0.05f, 
            ammo = 0, 
            maxAmmo = 225, 
            range = 100f,
            type = "SMG"
        };

        // Weapon 5: Grenade
        weapons[5] = new Weapon 
        { 
            name = "Grenade", 
            damage = 50, 
            fireRate = 2f, 
            ammo = 0, 
            maxAmmo = 15, 
            range = 300f,
            type = "Grenade"
        };
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Weapon Switch
        if (Input.GetKeyDown(KeyCode.E)) SelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.R)) SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.T)) SelectWeapon(2);
        if (Input.GetKeyDown(KeyCode.F)) SelectWeapon(3);
        if (Input.GetKeyDown(KeyCode.G)) SelectWeapon(4);
        if (Input.GetKeyDown(KeyCode.H)) SelectWeapon(5);

        // Shoot
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (currentWeaponIndex == -1)
        {
            Debug.Log("No weapon selected!");
            return;
        }

        if (!canShoot || Time.time - lastFireTime < weapons[currentWeaponIndex].fireRate)
            return;

        if (weapons[currentWeaponIndex].ammo <= 0)
        {
            Debug.Log("No ammo!");
            return;
        }

        lastFireTime = Time.time;
        weapons[currentWeaponIndex].ammo--;

        // Raycast from camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, weapons[currentWeaponIndex].range))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PhotonView targetPhoton = hit.collider.GetComponent<PhotonView>();
                    if (targetPhoton != null && targetPhoton != photonView)
                    {
                        targetPhoton.RPC("TakeDamage", RpcTarget.All, 
                            weapons[currentWeaponIndex].damage, photonView.Owner.NickName);
                        
                        // Add kill
                        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.AddKill();
                        }
                    }
                }

                Debug.Log($"[HIT] {weapons[currentWeaponIndex].name} hit {hit.collider.name} for {weapons[currentWeaponIndex].damage} damage");
            }
        }
    }

    void Reload()
    {
        if (currentWeaponIndex == -1) return;
        
        int ammoNeeded = weapons[currentWeaponIndex].maxAmmo - weapons[currentWeaponIndex].ammo;
        weapons[currentWeaponIndex].ammo = weapons[currentWeaponIndex].maxAmmo;
        
        Debug.Log($"[RELOAD] {weapons[currentWeaponIndex].name} reloaded. Ammo: {weapons[currentWeaponIndex].ammo}/{weapons[currentWeaponIndex].maxAmmo}");
    }

    void SelectWeapon(int index)
    {
        if (index >= weapons.Length)
        {
            Debug.Log("Invalid weapon index!");
            return;
        }

        currentWeaponIndex = index;
        Debug.Log($"[SELECT] Selected: {weapons[currentWeaponIndex].name}");
    }

    public Weapon GetCurrentWeapon()
    {
        if (currentWeaponIndex == -1) return null;
        return weapons[currentWeaponIndex];
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }

    public void AddAmmo(string weaponType, int amount)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.type == weaponType)
            {
                weapon.ammo += amount;
                if (weapon.ammo > weapon.maxAmmo)
                    weapon.ammo = weapon.maxAmmo;
                
                Debug.Log($"[AMMO] Added {amount} ammo to {weapon.name}. Total: {weapon.ammo}");
                break;
            }
        }
    }
}
