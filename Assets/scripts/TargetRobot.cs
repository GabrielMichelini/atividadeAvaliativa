using UnityEngine;

public class TargetRobot : MonoBehaviour
{
    [Header("Atributos")]
    public float health = 100f;

    [Header("Efeitos")]
    public GameObject explosaoPrefab; // O espaço para o efeito que criamos

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Acertou o robô! Vida restante: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Cria a explosão na exata posição (transform.position) onde o robô está
        if (explosaoPrefab != null)
        {
            GameObject efeito = Instantiate(explosaoPrefab, transform.position, transform.rotation);
            
            // Destrói as partículas da memória depois de 2 segundos para não pesar o jogo
            Destroy(efeito, 2f); 
        }

        Debug.Log("Robô Destruído!");
        
        // 2. Faz o robô sumir
        Destroy(gameObject); 
    }
}