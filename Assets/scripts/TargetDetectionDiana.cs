using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetDetectionDiana : MonoBehaviour
{
    public GameObject targetCenter; // Centro de la diana
    public GameObject targetLimitCenter;

    public GameObject DardosEnganchadosList;

    public Vector2 dartboardCenter; // Centro de la diana
    public float bullseyeRadius = 0.05f; // Radio del Bullseye
    public float outerBullseyeRadius = 0.1f; // Radio del Outer Bullseye
    public float tripleRingInnerRadius = 0.2f; // Radio interno del Triple
    public float tripleRingOuterRadius = 0.25f; // Radio externo del Triple
    public float doubleRingInnerRadius = 0.4f; // Radio interno del Doble
    public float doubleRingOuterRadius = 0.45f; // Radio externo del Doble
    public float dartboardRadius = 0.5f; // Radio total de la diana

    // Puntuaci�n de cada zona de la diana en orden de derecha a izquierda
    private int[] sectorScores = { 11, 14, 9, 12, 5, 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8 };

    public List<Vector3> darts; // Posiciones de los dardos lanzados

    public TextMeshPro[] showActualLaunch;

    // ================ Sistema eventos para controlar scoreBoard ================
    public delegate void DartImpacted_EVENT(int points);
    public event DartImpacted_EVENT DartImpacted;
    // ===========================================================================

    private void Awake()
    {
        dartboardCenter = Vector2.zero;
    }

    void Start()
    {
        CalculateScores();
    }

    private void Update()
    {
        if (delayBetweenLaunchs < MAX_Limit_BetweenLaunchs)
        {
            delayBetweenLaunchs += Time.deltaTime;
        }
    }

    private float delayBetweenLaunchs = 0f;
    private float MAX_Limit_BetweenLaunchs = 0.5f;
    private int numberOfDarts = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        // En caso de ser un dardo
        if (other.CompareTag("DART") && delayBetweenLaunchs >= 0.5)
        {
            delayBetweenLaunchs = 0.0f; // Poner limite de tiempo

            // Guarda la posici�n global del dardo antes de cambiar su padre
            Vector3 globalPosition = other.transform.position;

            // Cambia el padre del dardo al objeto de la diana
            other.transform.SetParent(transform, true); // Mantiene la posici�n global

            // Restaura la posici�n global del dardo para que permanezca en el punto de impacto
            other.transform.position = new Vector3(globalPosition.x + 0.2f, globalPosition.y, globalPosition.z);

            // Le quitamos la gravedad y lo dejamos con velocidad 0 para que quede "clavado" en la diana
            Rigidbody dartRigidBody = other.GetComponent<Rigidbody>();
            if (dartRigidBody != null)
            {
                other.attachedRigidbody.isKinematic  = true;
                other.attachedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

            // Calculamos la posicion correspondiente
            // Sumamos los puntos a a�adir al puntuaje final

            // Obtener el punto m�s cercano en la superficie del objeto de la diana
            Vector3 impactPoint = new Vector2(other.transform.localPosition.x, other.transform.localPosition.y); //other.ClosestPoint(transform.position);
            // Fijar la posici�n del dardo al punto de impacto
            //other.transform.position = impactPoint;
            int newScore = GetScore(impactPoint);

            // En caso de encestar, a�adimos puntos
            GameController.Instance.AddPointsToPlayer(newScore);
            DartImpacted(newScore);

            UpdateScorePoint(newScore); // Mostramos el puntuaje en los tablones cercanos

            Debug.Log($"El dardo en {impactPoint} obtuvo una puntuaci�n de {newScore}.");
            
            // Sumamos 1 a el numero de dardos impactados
            numberOfDarts++;
            if (numberOfDarts == 3)
            {
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("DART"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                numberOfDarts = 0; // Reiniciamos el contador
            }
        }
    }

    void UpdateScorePoint(int points)
    {
        for (int i = 0; i < showActualLaunch.Length; i++) {
            showActualLaunch[i].text = points.ToString();
        }
    }

    void CalculateScores()
    {
        foreach (Vector3 dart in darts)
        {
            int score = GetScore(dart);
            Debug.Log($"El dardo en {dart} obtuvo una puntuaci�n de {score}.");
        }
    }

    int GetScore(Vector2 dart)
    {
        // Calcular distancia del dardo al centro de la diana
        float distance = Vector2.Distance(dart, dartboardCenter);

        // Centro
        if (distance <= bullseyeRadius) return 50;

        // Segundo Centro
        if (distance <= outerBullseyeRadius) return 25;

        // Determinar en qu� sector cae el dardo
        float angle = Mathf.Atan2(dart.y - dartboardCenter.y, dart.x - dartboardCenter.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        // Ajustar el �ngulo para que coincida con el sector 20 empezando a los 9�
        float adjustedAngle = (angle + 9) % 360;

        // Calcular el �ndice del sector basado en el �ngulo ajustado
        int sectorIndex = Mathf.FloorToInt(adjustedAngle / 18); // 18� por sector
        int baseScore = sectorScores[sectorIndex];
        Debug.Log(sectorIndex);

        // Zonas de puntuaci�n adicionales
        if (distance > tripleRingInnerRadius && distance <= tripleRingOuterRadius) return baseScore * 3;
        if (distance > doubleRingInnerRadius && distance <= doubleRingOuterRadius) return baseScore * 2;
        if (distance <= dartboardRadius) return baseScore;

        // Fuera de la diana
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