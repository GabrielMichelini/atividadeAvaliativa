using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifeTime = 3f; // Tempo para a bala sumir se não acertar nada

    void Start()
    {
        // Se a bala se perder no mapa, ela se destrói sozinha após 3 segundos
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica se o objeto em que a bala bateu tem o script do Robô
        TargetRobot robot = collision.gameObject.GetComponent<TargetRobot>();
        
        if (robot != null)
        {
            robot.TakeDamage(damage);
        }

        // Destrói a bala imediatamente após bater em qualquer coisa (robô, parede ou chão)
        Destroy(gameObject);
    }
}