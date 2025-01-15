using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar UI como Slider

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
    private bool isIncreasing = true; // Controla si la fuerza est� aumentando o disminuyendo

    public Slider forceSlider; // Referencia al slider del HUD para la barra de carga

    void Start()
    {
        // Generar el primer dardo al iniciar el juego
        SpawnNewDart();

        // Inicializar el slider con el valor m�nimo
        if (forceSlider != null)
        {
            forceSlider.minValue = 0f;
            forceSlider.maxValue = maxForce;
            forceSlider.value = 0f; // Empezamos con la barra vac�a
        }
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("BotonX")) // Boton X del mando
        {
            // Poner la barra en el HUD subiendo y bajando !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            isCharging = true;
            currentForce = 0f;
            isIncreasing = true; // Comienza a incrementar la fuerza
        }

        // Incrementar o disminuir la fuerza mientras se mantiene presionado el bot�n
        if (isCharging && (Input.GetKey(KeyCode.Space) || Input.GetButton("BotonX") || Input.GetKey(KeyCode.JoystickButton0))) // Boton X del mando
        {
            if (isIncreasing)
            {
                currentForce += chargeRate * Time.deltaTime;
                if (currentForce >= maxForce)
                {
                    // Si alcanza el m�ximo, comienza a disminuir la fuerza
                    isIncreasing = false;
                }
            }
            else
            {
                currentForce -= chargeRate * Time.deltaTime;
                if (currentForce <= 0f)
                {
                    // Si llega a 0, empieza a aumentar de nuevo
                    isIncreasing = true;
                }
            }

            // Asegurar que la fuerza se mantenga entre 0 y maxForce
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce);

            // Actualizar el slider con el valor de la fuerza
            if (forceSlider != null)
            {
                forceSlider.value = currentForce;
            }
        }

        // Lanzar el dardo al soltar el bot�n
        if (isCharging && Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("BotonX"))
        {
            isCharging = false;
            ThrowDart(currentForce);
            currentForce = 0f;

            // Restablecer el slider despu�s de lanzar el dardo
            if (forceSlider != null)
            {
                forceSlider.value = 0f;
            }
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
                // A�adimos que hemos lanzado un dardo m�s
                GameController.Instance.LaunchDart();
            }
            currentDart = null; // Eliminar referencia al dardo actual

            Invoke(nameof(SpawnNewDart), 0.5f); // Generar un nuevo dardo despu�s de lanzar
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