using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    // Agora o sistema do Spawner pode escolher a direção que o robô vai se mover
    public bool moveHorizontal;
    public bool moveVertical;
    
    public float speed = 3f;      
    public float distance = 4f;   
    private Vector3 startPosition;

    void Start() { startPosition = transform.position; }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        Vector3 offset = Vector3.zero;

        // Se marcou caixinha Horizontal, anda no X
        if (moveHorizontal) offset.x = movement;
        
        // Se marcou caixinha Vertical, sobe e desce no Y
        if (moveVertical) offset.y = movement;

        transform.position = startPosition + offset;
    }
}