using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifeTime = 3f; 

    void Start() { Destroy(gameObject, lifeTime); }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Tenta achar um Robô para dar dano
        TargetRobot robot = collision.gameObject.GetComponent<TargetRobot>();
        if (robot != null) robot.TakeDamage(damage);

        // 2. NOVO: Tenta achar um Item para coletar
        PickupItem item = collision.gameObject.GetComponent<PickupItem>();
        if (item != null) item.Coletar();
        
        // Destrói a bala de qualquer jeito ao bater em algo
        Destroy(gameObject);
    }
}