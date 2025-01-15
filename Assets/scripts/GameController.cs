using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Propiedad est�tica para el Singleton.
    public static GameController Instance { get; private set; }

    // Modo de juego (0: T�pico, 1: Otro modo, etc.).
    public int GameMode = 0;

    // Jugadores y sus puntos.
    public int playersPlaying = 1;
    public List<int> playersPoints;

    // Variables del turno.
    public int actualTurn = 0; // Jugador actual (�ndice).
    public int dardosLanzados = 0;

    // Estados del juego.
    public enum GameState { ThrowingDarts, NextTurn }
    public GameState currentState = GameState.ThrowingDarts;

    // ================ Sistema eventos para controlar scoreBoard ================
    public delegate void NextPlayer_EVENT(int player);
    public event NextPlayer_EVENT NextPlayer;

    public delegate void NewRound_EVENT();
    public event NewRound_EVENT NextRound;

    public delegate void ResetGame_EVENT(int newPlayers);
    public event ResetGame_EVENT ResetGame;
    // ===========================================================================
    public CartelJugadores[] cartel;
    [SerializeField] private int pointsToWin = 301;
    public int GetPointsToWin() { return pointsToWin; }
    public void UpdatePlayerNumber(int newPlayers) { playersPlaying = newPlayers; }
    // ===========================================================================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < cartel.Length; i++)
        {
            cartel[i].CartelEvent += UpdatePlayerNumber;
        }
        InitializeNewGame(playersPlaying);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeNewGame(playersPlaying); // Inicializar el juego cuando se carga una nueva escena
    }

    // Inicializar un nuevo juego con un n�mero dado de jugadores.
    public void InitializeNewGame(int newPlayers)
    {
        playersPlaying = newPlayers;
        playersPoints = new List<int>();

        // Inicializar puntos a 0 para cada jugador.
        for (int i = 0; i < playersPlaying; i++)
        {
            playersPoints.Add(0);
        }

        // Reiniciar variables del turno.
        actualTurn = 0;
        dardosLanzados = 0;
        currentState = GameState.ThrowingDarts;

        ResetGame(newPlayers);
    }

    // M�todo para avanzar al siguiente estado.
    public void LaunchDart()
    {
        switch (currentState)
        {
            case GameState.ThrowingDarts:
                dardosLanzados++;
                if (dardosLanzados >= 2)
                {
                    currentState = GameState.NextTurn;
                    dardosLanzados = 0;
                }
                break;

            case GameState.NextTurn:
                actualTurn = (actualTurn + 1) % playersPlaying; // Avanzar al siguiente jugador.
                if (actualTurn == 0) // Es nueva ronda
                {
                    NextRound();
                } else
                {
                    NextPlayer(actualTurn); // Evento para indicar a todos que ahora toca el siguiente jugador
                }
                currentState = GameState.ThrowingDarts;
                break;
        }
    }

    public void AddPointsToPlayer(int points)
    {
        if (actualTurn < playersPoints.Count)
        {
            playersPoints[actualTurn] += points;
            if (playersPoints[actualTurn] == pointsToWin)
            {
                Debug.Log("Player " + actualTurn + " wins!");
                SceneManager.LoadScene("MenuInicial");
            } else if (playersPoints[actualTurn] > pointsToWin)
            {
                Debug.Log("Player " + actualTurn + " needs to close cleanly.");
                playersPoints[actualTurn] -= points;
            }
        }
    }

}