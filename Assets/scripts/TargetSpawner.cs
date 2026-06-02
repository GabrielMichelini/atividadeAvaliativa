using UnityEngine;
using System.Collections.Generic;

// Essa estrutura cria as caixinhas detalhadas que você viu na foto
[System.Serializable]
public class SpawnConfig
{
    public Transform Position;
    public GameObject TargetPrefab;
    public int Quantity;
    public Vector3 Scale = Vector3.one; // Começa com tamanho 1x1x1
    public Vector3 Rotation;
    public bool MoveHorizontal;
    public bool MoveVertical;
    public float MoveSpeed;
    public float MoveRange;
    public float Health;
    public int PointsValue;

    // Lista invisível para o sistema saber quantos robôs dessa caixinha estão vivos
    [HideInInspector] public List<GameObject> alvosVivos = new List<GameObject>();
}

public class TargetSpawner : MonoBehaviour
{
    // Isso cria a lista idêntica à da foto no Inspector
    public SpawnConfig[] SpawnPoints;
    
    private bool spawnerLigado = false;

    // O Hangar vai chamar isso para dar a largada
    public void IniciarSpawns()
    {
        spawnerLigado = true;
    }

    // O Hangar vai chamar isso quando o tempo acabar para limpar tudo
    public void PararELimparSpawns()
    {
        spawnerLigado = false;
        foreach (var config in SpawnPoints)
        {
            foreach (var alvo in config.alvosVivos)
            {
                if (alvo != null) Destroy(alvo);
            }
            config.alvosVivos.Clear();
        }
    }

    void Update()
    {
        if (!spawnerLigado) return;

        foreach (var config in SpawnPoints)
        {
            // Limpa da lista os alvos que você destruiu
            config.alvosVivos.RemoveAll(alvo => alvo == null);

            // Se você matou um alvo, o sistema cria outro para manter a Quantidade (Quantity) exigida
            while (config.alvosVivos.Count < config.Quantity)
            {
                CriarAlvo(config);
            }
        }
    }

    void CriarAlvo(SpawnConfig config)
    {
        if (config.TargetPrefab == null || config.Position == null) return;

        // 1. Cria o alvo com a rotação customizada
        GameObject alvo = Instantiate(config.TargetPrefab, config.Position.position, Quaternion.Euler(config.Rotation));
        
        // 2. Aplica o tamanho (Scale)
        alvo.transform.localScale = config.Scale;

        // 3. Aplica a Vida e os Pontos
        TargetRobot scriptVida = alvo.GetComponent<TargetRobot>();
        if (scriptVida != null)
        {
            scriptVida.health = config.Health;
            scriptVida.pointsValue = config.PointsValue;
        }

        // 4. Aplica o Movimento customizado
        RobotMovement scriptMovimento = alvo.GetComponent<RobotMovement>();
        if (scriptMovimento != null)
        {
            scriptMovimento.moveHorizontal = config.MoveHorizontal;
            scriptMovimento.moveVertical = config.MoveVertical;
            scriptMovimento.speed = config.MoveSpeed;
            scriptMovimento.distance = config.MoveRange;
        }

        // Salva na lista
        config.alvosVivos.Add(alvo);
    }
}