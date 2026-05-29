using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum TipoItem { Tempo, Municao }
    
    [Header("Configuração do Item")]
    public TipoItem tipo; // No Inspector da Unity, você escolhe se é Tempo ou Munição
    public float tempoExtra = 5f;
    public int municaoExtra = 10;
    
    [Header("Efeitos")]
    public GameObject particulaColeta; // Opcional: Uma faísca para quando coletar

    public void Coletar()
    {
        // O HangarMinigame agora tem uma 'instance', facilitando o acesso direto a ele!
        if (tipo == TipoItem.Tempo)
        {
            HangarMinigame.instance.AdicionarTempo(tempoExtra);
            Debug.Log("+ Tempo Coletado!");
        }
        else if (tipo == TipoItem.Municao)
        {
            HangarMinigame.instance.AdicionarMunicao(municaoExtra);
            Debug.Log("+ Munição Coletada!");
        }

        // Cria a partícula se você tiver colocado alguma
        if (particulaColeta != null)
        {
            GameObject efeito = Instantiate(particulaColeta, transform.position, transform.rotation);
            Destroy(efeito, 1f);
        }

        // Destrói o item flutuante da tela
        Destroy(gameObject);
    }
}