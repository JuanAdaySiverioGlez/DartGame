using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelJugadores : MonoBehaviour
{
    [SerializeField] private int numberOfPlayers = 1;	
    public delegate void Cartel_EVENT(int players);
    public event Cartel_EVENT CartelEvent; 

    private bool lookingAtThis;
    void Update()
    {
        if (lookingAtThis && Input.GetButtonDown("BotonX"))
        {
            CartelEvent?.Invoke(numberOfPlayers); // Usar el operador de invocación segura
        }
    }

    void OnPointerEnter()
    {
        lookingAtThis = true;
    }

    void OnPointerExit()
    {
        lookingAtThis = false;
    }
}
