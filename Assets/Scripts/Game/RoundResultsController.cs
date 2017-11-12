using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RoundResultsController : MonoBehaviour
    {
        [SerializeField] private GameObject _roundResultsGO;
        [SerializeField] private float _yOffset = -50;
        [SerializeField] private GameObject _textPrefab;
        [SerializeField] private Color _colorWon = Color.green;
        [SerializeField] private Color _colorLoss = Color.red;
        [SerializeField] private Vector3 _positionPlayerScore;
        [SerializeField] private Vector3 _positionOpponentScore;
        private List<GameObject> _scores = new List<GameObject>();

        private void OnEnable()
        {
            DisplayScores(_positionPlayerScore, Manager.GameManager.Player, Manager.GameManager.Opponent);
            DisplayScores(_positionOpponentScore, Manager.GameManager.Opponent, Manager.GameManager.Player);
        }

        private void DisplayScores(Vector3 pos, GameManager.Score playerScore, GameManager.Score opponentScore)
        {
            for (int i = 0; i < playerScore.Moves.Count; i++)
            {
                GameObject textGO = Instantiate(_textPrefab, _roundResultsGO.transform);
                textGO.transform.localPosition = pos + new Vector3(0, i * _yOffset, 0);
                Text textComponent = textGO.GetComponent<Text>();
                textComponent.color = playerScore.Moves[i] > opponentScore.Moves[i] ? _colorWon : _colorLoss;
                textComponent.text = string.Format("{0:0.00}", playerScore.Moves[i]);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _scores.Count; i++)
            {
                Destroy(_scores[i]);
            }
        }
    }
}
