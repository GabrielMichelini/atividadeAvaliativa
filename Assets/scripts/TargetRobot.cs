using UnityEngine;

public class TargetRobot : MonoBehaviour
{
    public float health = 100f;
    public int pointsValue = 10; // Nova variável que será alterada pelo Spawner
    public GameObject explosaoPrefab;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        // Envia a pontuação customizada em vez de enviar sempre 10
        if (UIManager.instance != null)
        {
            UIManager.instance.AtualizarScore(pointsValue);
        }

        if (explosaoPrefab != null)
        {
            GameObject efeito = Instantiate(explosaoPrefab, transform.position, transform.rotation);
            Destroy(efeito, 2f); 
        }
        
        Destroy(gameObject); 
    }
}