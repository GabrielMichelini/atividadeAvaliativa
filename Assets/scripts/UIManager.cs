using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Isso permite que qualquer outro script ache o UIManager sem precisarmos arrastar nada!
    public static UIManager instance;

    [Header("Telas")]
    public GameObject painelHUD; // Um objeto vazio dentro do Canvas que guarda os textos de jogo
    public GameObject gameOverTexto; // O texto de Fim de Jogo

    [Header("Textos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoTimerText;

    private int score = 0;

    void Awake() 
    { 
        instance = this; 
        
        // Esconde a interface quando o jogo começa
        painelHUD.SetActive(false);
        gameOverTexto.SetActive(false);
    }

    public void IniciarHUD()
    {
        painelHUD.SetActive(true);
        score = 0;
        AtualizarScore(0);
    }

    public void AtualizarScore(int pontos)
    {
        score += pontos;
        scoreText.text = "Pontos: " + score;
    }

    public void AtualizarAmmoTimer(int ammo, float time)
    {
        // Garante que o relógio não mostre números negativos
        int tempoArredondado = Mathf.Max(0, Mathf.RoundToInt(time));
        ammoTimerText.text = "Munição: " + ammo + " | Tempo: " + tempoArredondado + "s";
    }

    public void MostrarGameOver()
    {
        gameOverTexto.SetActive(true);
    }
}