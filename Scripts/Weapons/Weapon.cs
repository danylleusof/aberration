using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform Camera;

    float timeSinceLastShot;

    public ParticleSystem muzzleFlash;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        // Start with full magazine
        weaponData.currentAmmo = weaponData.magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(Camera.position, Camera.forward * weaponData.maxDistance);
    }

    void OnDisable() => weaponData.reloading = false;

    public void StartReload()
    {
        // Reload
        if (!weaponData.reloading && this.gameObject.activeSelf && weaponData.currentAmmo < weaponData.magazineSize)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        weaponData.reloading = true;
        AudioManager.instance.PlaySFX("Reload");
        playerController.WeaponAnimation();
        yield return new WaitForSeconds(weaponData.reloadTime);
        weaponData.currentAmmo = weaponData.magazineSize;
        weaponData.reloading = false;
    }

    bool CanShoot() => !weaponData.reloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && Time.timeScale == 1;

    void Shoot()
    {
        if (weaponData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                playerController.WeaponAnimation();
                AudioManager.instance.PlaySFX("Fire");
                muzzleFlash.Play();

                if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(weaponData.damage);
                }

                weaponData.currentAmmo--;
                timeSinceLastShot = 0;
            }
        }
        // Play Out of Ammo sound
        else if (weaponData.currentAmmo == 0)
        {
            if (CanShoot())
            {
                playerController.WeaponAnimation();
                AudioManager.instance.PlaySFX("Out of Ammo");
                timeSinceLastShot = 0;
            }
        }
    }
}
