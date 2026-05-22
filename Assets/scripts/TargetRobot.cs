using UnityEngine;

public class TargetRobot : MonoBehaviour
{
    public float health = 100f;
    public GameObject explosaoPrefab;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        // Avisa o UIManager central para somar 10 pontos na conta!
        if (UIManager.instance != null)
        {
            UIManager.instance.AtualizarScore(10);
        }

        if (explosaoPrefab != null)
        {
            GameObject efeito = Instantiate(explosaoPrefab, transform.position, transform.rotation);
            Destroy(efeito, 2f); 
        }
        
        Destroy(gameObject); 
    }
}