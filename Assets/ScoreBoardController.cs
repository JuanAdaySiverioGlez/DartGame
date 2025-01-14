using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreBoardController : MonoBehaviour
{
    public TextMeshPro[] totalPointsByPlayer; // Puntos ordenados de los jugadores  
    public TextMeshPro[] roundPointsByPlayer; // Puntos ordenados de los jugadores
    public TextMeshPro[] actualTurn; // Puntos ordenados de los jugadores

    public TargetDetectionDiana diana;

    private void Start()
    {
        GameController.Instance.NextPlayer += NextPlayer;
        GameController.Instance.NextRound += NewRound;
        GameController.Instance.ResetGame += ResetGame;
        diana.DartImpacted += UpdateScoreBoard;
    }

    void ResetGame(int newPlayers)
    {
        NewRound();
        for (int i = 0; i < newPlayers; i++) {
            totalPointsByPlayer[i].text = "0";
            roundPointsByPlayer[i].text = "";
        }
    }

    void UpdateScoreBoard(int newPoints)
    {
        int player = GameController.Instance.actualTurn;
        int actualTotalPoints, actualRoundPoints;
        int.TryParse(totalPointsByPlayer[player].text, out actualTotalPoints);
        int.TryParse(roundPointsByPlayer[player].text, out actualRoundPoints);
        totalPointsByPlayer[player].text = (actualTotalPoints + newPoints).ToString();
        roundPointsByPlayer[player].text = (actualRoundPoints + newPoints).ToString();
    }

    public void NextPlayer(int nextPlayer)
    {
        for (int i = 0; i < actualTurn.Length; i++)
        {
            actualTurn[i].text = ""; // Todos disabled
        }
        actualTurn[nextPlayer].text = "X"; // iniciamos con el 1
    }

    void NewRound()
    {
        for (int i = 0; i < roundPointsByPlayer.Length; i++)
        {
            roundPointsByPlayer[i].text = ""; // Eliminamos los puntos de la ronda
        }

        for (int i = 0; i < actualTurn.Length; i++)
        {
            actualTurn[i].text = ""; // Todos disabled
        }
        actualTurn[0].text = "X"; // iniciamos con el 1
    }
}
