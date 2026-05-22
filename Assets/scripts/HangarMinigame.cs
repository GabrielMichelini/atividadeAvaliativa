using UnityEngine;
using TMPro; 
using System.Collections;

public class HangarMinigame : MonoBehaviour
{
    [Header("Referências")]
    public FPSController playerMove;
    public CustomShooter playerShoot;
    public TextMeshProUGUI textoContagem; 
    
    [Header("Regras do Desafio")]
    public float tempoDeJogo = 30f;    // 30 segundos de minigame
    public int municaoDoDesafio = 15;  // Quantas balas o jogador ganha

    private bool minigameIniciado = false;
    private bool jogoRolando = false;

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

        // Prepara a interface e a arma
        UIManager.instance.IniciarHUD();
        playerShoot.currentAmmo = municaoDoDesafio;
        playerShoot.gastaMunicao = true; // Agora começa a gastar bala!

        textoContagem.text = "VAI!";
        playerShoot.canShoot = true; 
        jogoRolando = true; // Inicia o Timer valendo!

        yield return new WaitForSeconds(1f);
        textoContagem.gameObject.SetActive(false);
    }

    void Update()
    {
        if (jogoRolando)
        {
            // O tempo começa a cair
            tempoDeJogo -= Time.deltaTime;
            
            // Avisa a interface para atualizar os números na tela
            UIManager.instance.AtualizarAmmoTimer(playerShoot.currentAmmo, tempoDeJogo);

            // Se o tempo acabar, Fim de Jogo!
            if (tempoDeJogo <= 0)
            {
                EncerrarJogo();
            }
        }
    }

    void EncerrarJogo()
    {
        jogoRolando = false;
        playerShoot.canShoot = false; // Trava a arma
        UIManager.instance.MostrarGameOver();
    }
}