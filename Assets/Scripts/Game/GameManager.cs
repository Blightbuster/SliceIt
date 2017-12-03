using System;
using System.Collections.Generic;
using System.Linq;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Canvas;
        public GameObject PlayingField;
        public List<GameObject> SlicingObjects = new List<GameObject>();
        public GameState State;
        public static GameManager Instance = null;

        public int PointsForWin;
        public float TotalMassLeft = 1000;
        public GameType GameMode;
        public Score Player = new Score();
        public Score Opponent = new Score();

        private static float _lastPlacedMass;
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
            Online,
        }

        public class Score
        {
            public int Points;
            public List<float> Moves = new List<float>();
        }

        private void Start()
        {
            if (Instance == null) Instance = this;
            GameMode = (GameType)Enum.Parse(typeof(GameType), Scenes.GetString("GameMode"));
            StartGame(5, GameMode);
            GameObject.Find("OpponentName").GetComponent<Text>().text = "You vs. " + Scenes.GetString("OpponentName");
        }

        public void StartGame(int iPointsForWin, GameType iGameMode)
        {
            Canvas = GameObject.Find("Canvas");
            PlayingField = GameObject.Find("PlayingField");
            PointsForWin = iPointsForWin;
            GameMode = iGameMode;
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
                    Scenes.Load("Menu");
                    break;
                case GameState.WonGame:
                    State = GameState.ShowRoundResults;
                    break;
                case GameState.LossGame:
                    GameObject.Find("Border").SetActive(false);
                    GameObject.Find("Background").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.73f);
                    ExplodeSlices();
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
                    if (TotalMassLeft <= 0) State = GameState.NoMassLeft;
                    break;
                case GameState.FinishedMove:
                    if (GetMassOnScale() <= 1)
                    {
                        State = GameState.Playing;
                        break;
                    }
                    if (GameMode == GameType.Online)
                    {
                        MultiplayerManager.Instance.FinishMove(_lastPlacedMass);
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
                    if (GameMode == GameType.Bot)
                    {
                        float botMove = _botOpponent.GetNextMove();
                        State = _lastPlacedMass > botMove ? GameState.WonRound : GameState.LossRound;
                        TotalMassLeft -= _lastPlacedMass;
                        Player.Moves.Add(_lastPlacedMass);
                        Opponent.Moves.Add(botMove);
                    }
                    break;
                case GameState.ShowRoundResults:
                    Canvas.transform.Find("FinishMove").gameObject.SetActive(false);
                    Canvas.transform.Find("ExitGame").gameObject.SetActive(true);
                    PlayingField.SetActive(false);
                    Controller.RoundResultsController.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void FinishMove()
        {
            foreach (GameObject slice in Controller.TagController.TagWeigh)
            {
                slice.AddComponent<FadeAndDestroy>();
            }
            _lastPlacedMass = GetMassOnScale();
            State = GameState.FinishedMove;
        }

        public void SetState(string iState)
        {
            State = (GameState)System.Enum.Parse(typeof(GameState), iState);
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

        public bool OpponentMove(float mass)
        {
            if (State == GameState.WaitForOpponent)
            {
                State = _lastPlacedMass > mass ? GameState.WonRound : GameState.LossRound;
                TotalMassLeft -= _lastPlacedMass;
                Player.Moves.Add(_lastPlacedMass);
                Opponent.Moves.Add(mass);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExplodeSlices()
        {
            foreach (SpriteSlicer2DSliceInfo sliceInfo in ObjectSlicer.Instance.SlicedSpriteInfo)
            {
                sliceInfo.ChildObjects.RemoveAll(item => item == null);
                foreach (GameObject slice in sliceInfo.ChildObjects)
                {
                    slice.GetComponent<Rigidbody2D>().mass = 0; // Set mass of each slicing object to 0 in order to apply less force
                    SpriteSlicer2D.ExplodeSprite(slice, 10, 1); // Explode the slices because its fun :D
                }
            }
        }

        private void UpdateScoreDisplay()
        {
            Controller.PlayerUIScoreController.UpdatePoints();
            Controller.OpponentUIScoreController.UpdatePoints();
        }

        private void InstantiateSlicingObjects()
        {
            List<GameObject> tmpSlicingObjects = SlicingObjects.Select(Instantiate).ToList();
            SlicingObjects = tmpSlicingObjects;
        }
    }
}
