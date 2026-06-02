using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Telas")]
    public GameObject painelHUD; 
    public GameObject gameOverTexto; 
    public GameObject painelPausa; 

    [Header("Textos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoTimerText;
    public TextMeshProUGUI valorSensibilidadeTexto; 

    [Header("Configurações")]
    public Slider sliderSensibilidade;
    public FPSController playerMove; 

    private int score = 0;
    private bool jogoPausado = false;

    void Awake() 
    { 
        instance = this; 
        painelHUD.SetActive(false);
        gameOverTexto.SetActive(false);
        painelPausa.SetActive(false); 
    }

    void Start()
    {
        if (sliderSensibilidade != null && playerMove != null)
        {
            // Tenta puxar a sensibilidade salva na memória. 
            // Se for a primeira vez jogando, ele usa a sensibilidade padrão do personagem.
            float sensibilidadeSalva = PlayerPrefs.GetFloat("MinhaSensibilidade", playerMove.mouseSensitivity);
            
            // Aplica a sensibilidade salva no personagem e no Slider
            playerMove.mouseSensitivity = sensibilidadeSalva;
            sliderSensibilidade.value = sensibilidadeSalva;
            
            if (valorSensibilidadeTexto != null) 
            {
                valorSensibilidadeTexto.text = sensibilidadeSalva.ToString("F1");
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        jogoPausado = !jogoPausado;
        painelPausa.SetActive(jogoPausado);

        if (jogoPausado)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void MudarSensibilidade(float novoValor)
    {
        if (playerMove != null)
        {
            playerMove.mouseSensitivity = novoValor;
        }
        
        if (valorSensibilidadeTexto != null)
        {
            valorSensibilidadeTexto.text = novoValor.ToString("F1");
        }

        // Toda vez que você mexe na barra, ele salva o novo valor na memória!
        PlayerPrefs.SetFloat("MinhaSensibilidade", novoValor);
        PlayerPrefs.Save();
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        int tempoArredondado = Mathf.Max(0, Mathf.RoundToInt(time));
        ammoTimerText.text = "Munição: " + ammo + " | Tempo: " + tempoArredondado + "s";
    }

    public void MostrarGameOver()
    {
        gameOverTexto.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}