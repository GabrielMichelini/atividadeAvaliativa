using UnityEngine;
using UnityEngine.InputSystem; 

public class CustomShooter : MonoBehaviour
{
    public bool canShoot = true; 
    public bool gastaMunicao = false; 
    public int currentAmmo = 30;
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float bulletSpeed = 50f; 
    public Animator blasterAnimator;
    public AudioSource somDeTiro; 

    void Update()
    {
        // Se pausado, não atira
        if (Time.timeScale == 0f) return;

        bool temBala = !gastaMunicao || currentAmmo > 0;

        if (canShoot && temBala && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (gastaMunicao) currentAmmo--;

        if (somDeTiro != null) somDeTiro.Play();

        if (blasterAnimator != null)
        {
            blasterAnimator.ResetTrigger("Fire"); 
            blasterAnimator.SetTrigger("Fire"); 
        }

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = firePoint.forward * bulletSpeed;
        }
    }
}