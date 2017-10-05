using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Canvas;
        public GameObject PlayingField;
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
            ShowRoundResults
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

        private void Start()
        {
            //StartGame(5, (GameType)Scenes.GetInt("Gamemode"));
            StartGame(5, 0);    // Debug only
        }

        public void StartGame(int iPointsForWin, GameType iGameMode)
        {
            Canvas = GameObject.Find("Canvas");
            PlayingField = GameObject.Find("PlayingField");
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
                    Scenes.Load("Main");
                    break;
                case GameState.WonGame:
                    State = GameState.ShowRoundResults;
                    break;
                case GameState.LossGame:
                    State = GameState.ShowRoundResults;
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
                case GameState.ShowRoundResults:
                    Canvas.transform.Find("FinishMove").gameObject.SetActive(false);
                    Canvas.transform.Find("ExitGame").gameObject.SetActive(true);
                    PlayingField.SetActive(false);
                    Controller.RoundResultsController.enabled = true;
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

        public void SetState(string tmpState)
        {
            State = (GameState) System.Enum.Parse(typeof(GameState), tmpState);
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

        private void InstantiateSlicingObjects()
        {
            List<GameObject> tmpSlicingObjects = new List<GameObject>();
            foreach (GameObject slicingObject in SlicingObjects)
            {
                tmpSlicingObjects.Add(Instantiate(slicingObject));
            }
            SlicingObjects = tmpSlicingObjects;
        }
    }
}
