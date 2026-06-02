using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum TipoItem { Tempo, Municao }
    
    public TipoItem tipo; 
    public float tempoExtra = 5f;
    public int municaoExtra = 10;
    public GameObject particulaColeta; 

    public void Coletar()
    {
        if (tipo == TipoItem.Tempo)
        {
            HangarMinigame.instance.AdicionarTempo(tempoExtra);
        }
        else if (tipo == TipoItem.Municao)
        {
            HangarMinigame.instance.AdicionarMunicao(municaoExtra);
        }

        if (particulaColeta != null)
        {
            GameObject efeito = Instantiate(particulaColeta, transform.position, transform.rotation);
            Destroy(efeito, 1f);
        }

        Destroy(gameObject);
    }
}