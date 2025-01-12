using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDardo : MonoBehaviour
{

    public float launchForce = 500f;
    public Vector3 launchDirection; // Está al reves

    private Rigidbody rb;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en el dardo");
            return;
        }

        launchDirection = -transform.forward;

        LaunchDart();
    }

    void LaunchDart()
    {
        rb.AddForce(launchDirection.normalized * launchForce);
    }

    void Update()
    {
        
    }
}
