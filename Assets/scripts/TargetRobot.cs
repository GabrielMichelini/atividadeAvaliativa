using UnityEngine;

public class TargetRobot : MonoBehaviour
{
    public float health = 100f; // Vida total do robô

    // Função que recebe o dano do tiro
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Acertou! Vida do robô: " + health);

        // Se a vida zerar, ele morre
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Robô Destruído!");
        Destroy(gameObject); // Apaga o robô da cena
    }
}