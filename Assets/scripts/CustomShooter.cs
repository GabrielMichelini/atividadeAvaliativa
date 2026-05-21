using UnityEngine;
using UnityEngine.InputSystem; 

public class CustomShooter : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public GameObject bulletPrefab; // O prefab azul da bala
    public Transform firePoint;     // O cano da arma
    public float bulletSpeed = 50f; // Velocidade do tiro

    [Header("Referências")]
    public Animator blasterAnimator;
    public AudioSource somDeTiro; 

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (somDeTiro != null) somDeTiro.Play();

        if (blasterAnimator != null)
        {
            blasterAnimator.ResetTrigger("Fire"); 
            blasterAnimator.SetTrigger("Fire"); 
        }

        // NOVO: Cria a bala física e empurra ela para a frente
        if (bulletPrefab != null && firePoint != null)
        {
            // Cria a bala na posição e rotação do cano da arma
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // Pega o Rigidbody da bala criada e aplica velocidade
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
        }
    }
}