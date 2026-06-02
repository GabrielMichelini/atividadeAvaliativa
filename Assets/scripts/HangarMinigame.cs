using UnityEngine;
using TMPro; 
using System.Collections;
using System.Collections.Generic;

public class HangarMinigame : MonoBehaviour
{
    public static HangarMinigame instance; 

    [Header("Referências")]
    public FPSController playerMove;
    public CustomShooter playerShoot;
    public TextMeshProUGUI textoContagem; 
    public TargetSpawner spawnerDeAlvos; // NOVO: Conecta com o cérebro dos Spawns
    
    [Header("Itens")]
    public GameObject itemTempoPrefab;
    public GameObject itemMunicaoPrefab;
    public float tempoSurgimentoItens = 8f; 
    public Transform[] pontosDeSpawnDeItens; // Pontos exclusivos para os itens nascerem

    [Header("Regras")]
    public float tempoDeJogo = 30f;    
    public int municaoDoDesafio = 15;  

    private bool minigameIniciado = false;
    private bool jogoRolando = false;
    private List<GameObject> itensAtivos = new List<GameObject>(); 

    void Awake() { instance = this; }

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

        // Dá a ordem para o Spawner começar a criar os robôs conforme as suas configurações
        if (spawnerDeAlvos != null) spawnerDeAlvos.IniciarSpawns();
        
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

            if (tempoDeJogo <= 0)
            {
                EncerrarJogo();
            }
        }
    }

    public void AdicionarTempo(float extra) { tempoDeJogo += extra; }

    public void AdicionarMunicao(int extra) 
    { 
        playerShoot.currentAmmo += extra; 
        UIManager.instance.AtualizarAmmoTimer(playerShoot.currentAmmo, tempoDeJogo);
    }

    IEnumerator GeradorDeItens()
    {
        while (jogoRolando)
        {
            yield return new WaitForSeconds(tempoSurgimentoItens);
            
            if (jogoRolando && pontosDeSpawnDeItens.Length > 0)
            {
                Transform pontoEscolhido = pontosDeSpawnDeItens[Random.Range(0, pontosDeSpawnDeItens.Length)];
                GameObject itemSorteado = (Random.value > 0.5f) ? itemTempoPrefab : itemMunicaoPrefab;
                Vector3 posicaoAlta = pontoEscolhido.position + new Vector3(0, 1f, 0);
                
                GameObject novoItem = Instantiate(itemSorteado, posicaoAlta, Quaternion.identity);
                itensAtivos.Add(novoItem);
                Destroy(novoItem, 6f); 
            }
        }
    }

    void EncerrarJogo()
    {
        jogoRolando = false;
        playerShoot.canShoot = false; 
        UIManager.instance.MostrarGameOver();

        // Manda o Spawner destruir todos os robôs
        if (spawnerDeAlvos != null) spawnerDeAlvos.PararELimparSpawns();

        foreach (GameObject item in itensAtivos) if (item != null) Destroy(item);
        itensAtivos.Clear();
    }
}