using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifeTime = 3f; 

    void Start() { Destroy(gameObject, lifeTime); }

    void OnCollisionEnter(Collision collision)
    {
        TargetRobot robot = collision.gameObject.GetComponent<TargetRobot>();
        if (robot != null) robot.TakeDamage(damage);

        PickupItem item = collision.gameObject.GetComponent<PickupItem>();
        if (item != null) item.Coletar();
        
        Destroy(gameObject);
    }
}