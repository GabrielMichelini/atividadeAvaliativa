using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    public float speed = 3f;      // Velocidade do movimento
    public float distance = 4f;   // Distância que ele vai para cada lado
    
    private Vector3 startPosition;

    void Start()
    {
        // Salva a posição onde o robô começou no mapa
        startPosition = transform.position;
    }

    void Update()
    {
        // Cria uma onda que vai de -1 a 1 baseado no tempo e multiplica pela distância
        float movement = Mathf.Sin(Time.time * speed) * distance;
        
        // Aplica o movimento somando na posição inicial (ajusta o eixo X)
        transform.position = startPosition + new Vector3(movement, 0f, 0f);
    }
}