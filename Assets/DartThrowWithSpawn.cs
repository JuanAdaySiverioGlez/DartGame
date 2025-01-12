using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartThrowWithSpawn : MonoBehaviour {
    public Transform cameraTransform; // Referencia a la c�mara del jugador
    public GameObject dartPrefab; // Prefab del dardo
    public Transform spawnOffset; // Offset delante de la c�mara para crear el dardo
    public float maxForce = 20f; // Fuerza m�xima del lanzamiento
    public float chargeRate = 10f; // Velocidad de carga de la fuerza

    public Vector3 dartScale = new Vector3(0.5f, 0.5f, -1f); // Tama�o del dardo al instanciarlo
    public Vector3 dartRotation = new Vector3(0f, 180f, 0f); // Rotaci�n adicional del dardo

    private GameObject currentDart; // Dardo actual que sigue la c�mara
    private float currentForce = 0f; // Fuerza acumulada
    private bool isCharging = false; // Indica si se est� cargando la fuerza

    void Start()
    {
        // Generar el primer dardo al iniciar el juego
        SpawnNewDart();
    }

    void Update()
    {
        // Si hay un dardo activo, sigue la posici�n y orientaci�n de la c�mara
        if (currentDart != null)
        {
            currentDart.transform.position = spawnOffset.position;
            currentDart.transform.rotation = cameraTransform.rotation * Quaternion.Euler(0f, 180f, 0f);

        }

        // Cargar fuerza al presionar el bot�n
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentForce = 0f;
        }

        // Incrementar la fuerza mientras se mantiene presionado el bot�n
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            currentForce += chargeRate * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce);
        }

        // Lanzar el dardo al soltar el bot�n
        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            ThrowDart(currentForce);
            currentForce = 0f;
        }
    }

    void ThrowDart(float force)
    {
        if (currentDart != null)
        {
            // Hacer que el dardo sea independiente de la c�mara
            Rigidbody rb = currentDart.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Activar f�sica
                rb.AddForce(cameraTransform.forward * force, ForceMode.Impulse);
            }

            currentDart = null; // Eliminar referencia al dardo actual
            Invoke(nameof(SpawnNewDart), 0.2f); // Generar un nuevo dardo despu�s de lanzar
        }
    }

    void SpawnNewDart()
    {
        // Crear un nuevo dardo en el offset delante de la c�mara
        currentDart = Instantiate(dartPrefab, spawnOffset.position, cameraTransform.rotation);

        // Asegurarse de que el Rigidbody est� en modo kinematic hasta que se lance
        Rigidbody rb = currentDart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Desactivar f�sica hasta que se lance
        }
    }
}