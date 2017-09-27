using System.Collections.Generic;
using cakeslice;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> SlicingObjects = new List<GameObject>();
    public GameState State;

    public int PlayerScore;
    public int OpponentScore;
    public int PointsForWin;

    private static GameType _gameMode = GameType.Bot;
    private static float _lastTotalWeight;
    private readonly Bot _botOpponent = new Bot();

    public enum GameState
    {
        Playing,
        StartGame,
        EndGame,
        WonGame,
        LossGame,
        WonRound,
        LossRound,
        FinishedMove,
        TimeOver,
        NoMassLeft,
        WaitForOpponent,
    }

    public enum GameType
    {
        Bot,
        Bluetooth,
        Wifi
    }

    // Testing only
    private void Start()
    {
        StartGame(5, GameType.Bot);
    }

    public void StartGame(int iPointsForWin, GameType iGameMode)
    {
        PointsForWin = iPointsForWin;
        _gameMode = iGameMode;
        State = GameState.StartGame;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (State)
        {
            case GameState.Playing:
                break;
            case GameState.StartGame:
                InstantiateSlicingObjects();
                State = GameState.Playing;
                break;
            case GameState.EndGame:
                break;
            case GameState.WonGame:
                break;
            case GameState.LossGame:
                break;
            case GameState.WonRound:
                PlayerScore++;
                State = GameState.Playing;
                if (PlayerScore >= PointsForWin) State = GameState.WonGame;
                break;
            case GameState.LossRound:
                OpponentScore++;
                State = GameState.Playing;
                if (OpponentScore >= PointsForWin) State = GameState.LossGame;
                break;
            case GameState.FinishedMove:
                State = GameState.WaitForOpponent;
                break;
            case GameState.TimeOver:
                State = GameState.FinishedMove;
                break;
            case GameState.NoMassLeft:
                State = GameState.LossGame;
                break;
            case GameState.WaitForOpponent:
                if (_gameMode == GameType.Bot)
                {
                    State = _lastTotalWeight > _botOpponent.GetNextMove() ? GameState.WonRound : GameState.LossRound;
                }
                break;
        }
    }

    public void FinishMove()
    {
        if (_gameMode == GameType.Bot)
        {
            foreach (GameObject slice in GetComponent<TagController>().TagWeigh)
            {
                slice.AddComponent<FadeAndDestroy>();
            }
        }
        _lastTotalWeight = GetWeightOnScale();
        State = GameState.FinishedMove;
    }

    private void InstantiateSlicingObjects()
    {
        List<GameObject> tmpSlicingObjects = new List<GameObject>();
        foreach (GameObject slicingObject in SlicingObjects)
        {
            tmpSlicingObjects.Add(Instantiate(slicingObject));
        }
        SlicingObjects = tmpSlicingObjects;
    }

    public float GetWeightOnScale()
    {
        // Get weight of all slices -> Sum them up -> Display the value on the screen
        float totalWeigh = 0;
        foreach (GameObject go in GetComponent<TagController>().TagWeigh)
        {
            totalWeigh += go.GetComponent<Rigidbody2D>().mass;
        }
        return totalWeigh;
    }
}
