using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObjects : MonoBehaviour
{
    // Referencia al PlayerController
    private PlayerController playerController;

    public Transform PlaceToGo;
    public float speed = 5f; // La velocidad a la que se mueve el objeto

    private void Start()
    {
        playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
    }

    private void MaxPointsObtained()
    {
        StartCoroutine(MovingToPosition());
    }

    private IEnumerator MovingToPosition()
    {
        // Mientras el objeto no haya llegado al destino
        while (Vector3.Distance(transform.position, PlaceToGo.position) > 0.1f) // 0.1f es el margen de error
        {
            // Mueve el objeto hacia la posición objetivo con una velocidad constante
            transform.position = Vector3.MoveTowards(transform.position, PlaceToGo.position, speed * Time.deltaTime);
            // Espera un frame antes de continuar
            yield return null;
        }

        // Aseguramos que el objeto llegue exactamente a la posición final
        transform.position = PlaceToGo.position;
    }
}
