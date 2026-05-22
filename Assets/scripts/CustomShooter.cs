using UnityEngine;
using UnityEngine.InputSystem; 

public class CustomShooter : MonoBehaviour
{
    [Header("Controle de Estado")]
    public bool canShoot = true; 
    public bool gastaMunicao = false; // Começa desligado (munição infinita)
    public int currentAmmo = 30;

    [Header("Configurações do Projétil")]
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float bulletSpeed = 50f; 

    [Header("Referências")]
    public Animator blasterAnimator;
    public AudioSource somDeTiro; 

    void Update()
    {
        // Só deixa atirar se não estiver gastando bala, OU se ainda tiver bala no pente
        bool temBala = !gastaMunicao || currentAmmo > 0;

        if (canShoot && temBala && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Se estiver no minigame, consome 1 bala
        if (gastaMunicao)
        {
            currentAmmo--;
        }

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