using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActualizarTexto : MonoBehaviour
{
    public CartelJugadores cartel;
    private int playersPlaying = 1;
    private Text text; 

    void Start()
    {
        cartel.CartelEvent += UpdateText;
    }

    void UpdateText(int players)
    {
        playersPlaying = players;
        text = GetComponent<Text>();
        text.text = "Jugadores: " + playersPlaying;
    }
}
