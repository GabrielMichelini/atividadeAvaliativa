using UnityEngine;
using TMPro; 
using System.Collections;
using System.Collections.Generic;

public class HangarMinigame : MonoBehaviour
{
    // A Instância permite que o script do Item encontre o Hangar instantaneamente
    public static HangarMinigame instance; 

    [Header("Referências do Jogador")]
    public FPSController playerMove;
    public CustomShooter playerShoot;
    public TextMeshProUGUI textoContagem; 
    
    [Header("Configurações do Inimigo")]
    public GameObject roboPrefab;       
    public Transform[] pontosDeSpawn;   

    [Header("Configurações dos Itens")]
    public GameObject itemTempoPrefab;
    public GameObject itemMunicaoPrefab;
    public float tempoSurgimentoItens = 8f; // Um item novo a cada 8 segundos

    [Header("Regras do Desafio")]
    public float tempoDeJogo = 30f;    
    public int municaoDoDesafio = 15;  

    private bool minigameIniciado = false;
    private bool jogoRolando = false;
    
    private List<GameObject> robosAtivos = new List<GameObject>();
    private List<GameObject> itensAtivos = new List<GameObject>(); // Controla os itens no mapa

    void Awake()
    {
        instance = this; // Liga o acesso rápido
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !minigameIniciado)
        {
            minigameIniciado = true;
            StartCoroutine(IniciarContagem());
        }
    }

    IEnumerator IniciarContagem()
    {
        playerMove.canMove = false;
        playerShoot.canShoot = false;
        
        textoContagem.gameObject.SetActive(true);
        textoContagem.text = "3";
        yield return new WaitForSeconds(1f);
        textoContagem.text = "2";
        yield return new WaitForSeconds(1f);
        textoContagem.text = "1";
        yield return new WaitForSeconds(1f);

        UIManager.instance.IniciarHUD();
        playerShoot.currentAmmo = municaoDoDesafio;
        playerShoot.gastaMunicao = true; 

        textoContagem.text = "VAI!";
        playerShoot.canShoot = true; 
        jogoRolando = true; 

        VerificarEPreencherInimigos();
        
        // Inicia a rotina de jogar itens no mapa
        StartCoroutine(GeradorDeItens());

        yield return new WaitForSeconds(1f);
        textoContagem.gameObject.SetActive(false);
    }

    void Update()
    {
        if (jogoRolando)
        {
            tempoDeJogo -= Time.deltaTime;
            UIManager.instance.AtualizarAmmoTimer(playerShoot.currentAmmo, tempoDeJogo);

            robosAtivos.RemoveAll(robo => robo == null);

            if (robosAtivos.Count < 3)
            {
                VerificarEPreencherInimigos();
            }

            if (tempoDeJogo <= 0)
            {
                EncerrarJogo();
            }
        }
    }

    // --- NOVAS FUNÇÕES PARA OS ITENS ---

    // O Item chama essa função quando você atira nele
    public void AdicionarTempo(float extra) 
    { 
        tempoDeJogo += extra; 
    }

    // O Item chama essa função quando você atira nele
    public void AdicionarMunicao(int extra) 
    { 
        playerShoot.currentAmmo += extra; 
        // Avisa a UI na mesma hora que a munição subiu!
        UIManager.instance.AtualizarAmmoTimer(playerShoot.currentAmmo, tempoDeJogo);
    }

    IEnumerator GeradorDeItens()
    {
        while (jogoRolando)
        {
            yield return new WaitForSeconds(tempoSurgimentoItens);
            
            if (jogoRolando && pontosDeSpawn.Length > 0)
            {
                // Escolhe um ponto aleatório
                Transform pontoEscolhido = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
                
                // 50% de chance de ser Tempo, 50% de ser Munição
                GameObject itemSorteado = (Random.value > 0.5f) ? itemTempoPrefab : itemMunicaoPrefab;
                
                // Cria o item e joga ele um pouquinho para cima (Y + 1) para não nascer colado no chão
                Vector3 posicaoAlta = pontoEscolhido.position + new Vector3(0, 1f, 0);
                GameObject novoItem = Instantiate(itemSorteado, posicaoAlta, Quaternion.identity);
                
                itensAtivos.Add(novoItem);
                
                // Opcional: Faz o item sumir sozinho se o jogador ignorar ele por 6 segundos
                Destroy(novoItem, 6f); 
            }
        }
    }

    // -----------------------------------

    void VerificarEPreencherInimigos()
    {
        if (!jogoRolando) return;

        while (robosAtivos.Count < 3 && pontosDeSpawn.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, pontosDeSpawn.Length);
            Transform pontoEscolhido = pontosDeSpawn[indiceAleatorio];

            GameObject novoRobo = Instantiate(roboPrefab, pontoEscolhido.position, roboPrefab.transform.rotation);
            robosAtivos.Add(novoRobo);
        }
    }

    void EncerrarJogo()
    {
        jogoRolando = false;
        playerShoot.canShoot = false; 
        UIManager.instance.MostrarGameOver();

        foreach (GameObject robo in robosAtivos) if (robo != null) Destroy(robo);
        foreach (GameObject item in itensAtivos) if (item != null) Destroy(item);
        
        robosAtivos.Clear();
        itensAtivos.Clear();
    }
}