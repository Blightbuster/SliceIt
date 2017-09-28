using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> SlicingObjects = new List<GameObject>();
    public GameState State;

    public Score Player = new Score();
    public Score Opponent = new Score();
    public int PointsForWin;
    public float TotalMassLeft = 1000;

    private static GameType _gameMode = GameType.Bot;
    private static float _lastTotalMass;
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
        NextRound,
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

    public class Score
    {
        public int Points;
        public List<float> Moves = new List<float>();
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
                Controller.PlayerUIScoreController.Setup();
                Controller.OpponentUIScoreController.Setup();
                State = GameState.Playing;
                break;
            case GameState.EndGame:
                break;
            case GameState.WonGame:
                break;
            case GameState.LossGame:
                break;
            case GameState.WonRound:
                Player.Points++;
                UpdateScoreDisplay();
                State = GameState.NextRound;
                if (Player.Points >= PointsForWin) State = GameState.WonGame;
                break;
            case GameState.LossRound:
                Opponent.Points++;
                UpdateScoreDisplay();
                State = GameState.NextRound;
                if (Opponent.Points >= PointsForWin) State = GameState.LossGame;
                break;
            case GameState.NextRound:
                State = GameState.Playing;
                if (TotalMassLeft <= 0) State = GameState.LossGame;
                break;
            case GameState.FinishedMove:
                if (GetMassOnScale() == 0)
                {
                    State = GameState.Playing;
                    break;   
                }
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
                    float botMove = _botOpponent.GetNextMove();
                    State = _lastTotalMass > botMove ? GameState.WonRound : GameState.LossRound;
                    TotalMassLeft -= _lastTotalMass;
                    Player.Moves.Add(_lastTotalMass);
                    Opponent.Moves.Add(botMove);
                }
                break;
        }
    }

    public void FinishMove()
    {
        if (_gameMode == GameType.Bot)
        {
            foreach (GameObject slice in Controller.TagController.TagWeigh)
            {
                slice.AddComponent<FadeAndDestroy>();
            }
        }
        _lastTotalMass = GetMassOnScale();
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

    public float GetMassOnScale()
    {
        // Get weight of all slices -> Sum them up -> Display the value on the screen
        float totalWeigh = 0;
        foreach (GameObject go in Controller.TagController.TagWeigh)
        {
            totalWeigh += go.GetComponent<Rigidbody2D>().mass;
        }
        return totalWeigh;
    }

    private void UpdateScoreDisplay()
    {
        Controller.PlayerUIScoreController.UpdatePoints();
        Controller.OpponentUIScoreController.UpdatePoints();
    }
}
