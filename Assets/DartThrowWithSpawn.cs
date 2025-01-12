using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartThrowWithSpawn : MonoBehaviour {
    public Transform cameraTransform; // Referencia a la cámara del jugador
    public GameObject dartPrefab; // Prefab del dardo
    public Transform spawnOffset; // Offset delante de la cámara para crear el dardo
    public float maxForce = 20f; // Fuerza máxima del lanzamiento
    public float chargeRate = 10f; // Velocidad de carga de la fuerza

    public Vector3 dartScale = new Vector3(0.5f, 0.5f, -1f); // Tamaño del dardo al instanciarlo
    public Vector3 dartRotation = new Vector3(0f, 180f, 0f); // Rotación adicional del dardo

    private GameObject currentDart; // Dardo actual que sigue la cámara
    private float currentForce = 0f; // Fuerza acumulada
    private bool isCharging = false; // Indica si se está cargando la fuerza

    void Start()
    {
        // Generar el primer dardo al iniciar el juego
        SpawnNewDart();
    }

    void Update()
    {
        // Si hay un dardo activo, sigue la posición y orientación de la cámara
        if (currentDart != null)
        {
            currentDart.transform.position = spawnOffset.position;
            currentDart.transform.rotation = cameraTransform.rotation * Quaternion.Euler(0f, 180f, 0f);

        }

        // Cargar fuerza al presionar el botón
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentForce = 0f;
        }

        // Incrementar la fuerza mientras se mantiene presionado el botón
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            currentForce += chargeRate * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce);
        }

        // Lanzar el dardo al soltar el botón
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
            // Hacer que el dardo sea independiente de la cámara
            Rigidbody rb = currentDart.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Activar física
                rb.AddForce(cameraTransform.forward * force, ForceMode.Impulse);
            }

            currentDart = null; // Eliminar referencia al dardo actual
            Invoke(nameof(SpawnNewDart), 0.2f); // Generar un nuevo dardo después de lanzar
        }
    }

    void SpawnNewDart()
    {
        // Crear un nuevo dardo en el offset delante de la cámara
        currentDart = Instantiate(dartPrefab, spawnOffset.position, cameraTransform.rotation);

        // Asegurarse de que el Rigidbody esté en modo kinematic hasta que se lance
        Rigidbody rb = currentDart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Desactivar física hasta que se lance
        }
    }
}