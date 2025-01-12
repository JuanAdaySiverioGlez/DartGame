using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectionDiana : MonoBehaviour
{
    public GameObject targetCenter; // Centro de la diana
    public GameObject targetLimitCenter;

    public Vector3 dartboardCenter; // Centro de la diana
    public float bullseyeRadius = 0.05f; // Radio del Bullseye
    public float outerBullseyeRadius = 0.1f; // Radio del Outer Bullseye
    public float tripleRingInnerRadius = 0.2f; // Radio interno del Triple
    public float tripleRingOuterRadius = 0.25f; // Radio externo del Triple
    public float doubleRingInnerRadius = 0.4f; // Radio interno del Doble
    public float doubleRingOuterRadius = 0.45f; // Radio externo del Doble
    public float dartboardRadius = 0.5f; // Radio total de la diana

    // Puntuación de cada zona de la diana en orden de derecha a izquierda
    private int[] sectorScores = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 }; 

    public List<Vector3> darts; // Posiciones de los dardos lanzados

    private void Awake()
    {
        dartboardCenter = targetCenter.transform.position;
    }

    void Start()
    {

        CalculateScores();

    }

    private void OnTriggerEnter(Collider other)
    {
        // En caso de ser un dardo
        if (true)
        {
            // Le quitamos la gravedad y lo dejamos con velocidad 0 para que quede "clavado" en la diana

            // Calculamos la posicion correspondiente
            // Sumamos los puntos a añadir al puntuaje final

        }
    }

    void CalculateScores()
    {
        foreach (Vector3 dart in darts)
        {
            int score = GetScore(dart);
            Debug.Log($"El dardo en {dart} obtuvo una puntuación de {score}.");
        }
    }

    int GetScore(Vector3 dart)
    {
        // Calcular distancia del dardo al centro
        float distance = Vector2.Distance(dart, dartboardCenter);

        // Centro
        if (distance <= bullseyeRadius) return 50;
        // Segundo Centro
        if (distance <= outerBullseyeRadius) return 25;

        // Determinar en que zona cae el dardo
        float angle = Mathf.Atan2(dart.y - dartboardCenter.y, dart.x - dartboardCenter.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        int sectorIndex = Mathf.FloorToInt(angle / 18); // Dividir el círculo en 20 sectores (18° por sector)
        int baseScore = sectorScores[sectorIndex];

        // Zona de triples
        if (distance > tripleRingInnerRadius && distance <= tripleRingOuterRadius) return baseScore * 3;

        // zona doble
        if (distance > doubleRingInnerRadius && distance <= doubleRingOuterRadius) return baseScore * 2;

        // zona normal
        if (distance <= dartboardRadius) return baseScore;

        // tiro fallao
        return 0;
    }

    // Dibuja la diana para facilitar el desarrollo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(dartboardCenter, bullseyeRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(dartboardCenter, outerBullseyeRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(dartboardCenter, tripleRingInnerRadius);
        Gizmos.DrawWireSphere(dartboardCenter, tripleRingOuterRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(dartboardCenter, doubleRingInnerRadius);
        Gizmos.DrawWireSphere(dartboardCenter, doubleRingOuterRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(dartboardCenter, dartboardRadius);
    }
}