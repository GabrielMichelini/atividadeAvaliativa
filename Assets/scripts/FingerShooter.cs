using UnityEngine;
using System.Collections; // Necessário para a rotina de tempo

public class FingerShooter : MonoBehaviour
{
    public float damage = 25f; 
    public float range = 100f; 
    public Camera fpsCam;      

    [Header("Efeitos Visuais")]
    public Transform firePoint;     // Arraste o objeto FirePoint para cá
    public LineRenderer laserLine;  // Arraste o Line Renderer para cá
    public float laserDuration = 0.05f; // Tempo que o laser brilha na tela

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. Liga o visual do laser por uma fração de segundo
        StartCoroutine(ShotEffect());

        // 2. O começo do laser sempre será na ponta do dedo
        laserLine.SetPosition(0, firePoint.position);

        RaycastHit hit;
        // O "olho" da mira continua sendo o centro da tela para você acertar onde olha
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
        {
            // 3. Se acertar algo, o final do laser vai até o ponto de impacto
            laserLine.SetPosition(1, hit.point);

            TargetRobot target = hit.transform.GetComponent<TargetRobot>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        else
        {
            // Se atirar pro céu, o laser vai reto até o infinito (range)
            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * range));
        }
    }

    // Função que liga e desliga o laser bem rápido
    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}