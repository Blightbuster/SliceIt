﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIScoreController : MonoBehaviour
    {
        [SerializeField] private bool _playerScore;
        [SerializeField] private GameObject _parantGameObject;
        [SerializeField] private GameObject _pointPrefab;
        [SerializeField] private int _xSpacing;
        [SerializeField] private Color _enabledColor;
        [SerializeField] private Color _disabledColor;

        private List<GameObject> PointList = new List<GameObject>();

        public void Setup()
        {
            float xPos = _parantGameObject.transform.position.x;
            float yPos = _parantGameObject.transform.position.y;

            for (int i = 0; i < GameManager.Instance.PointsForWin; i++)
            {
                Vector3 pos = new Vector3(xPos + i * _xSpacing, yPos, 0);
                PointList.Add(Instantiate(_pointPrefab, pos, Quaternion.identity, _parantGameObject.transform));
                PointList.Last().GetComponent<Image>().color = _disabledColor;
            }
        }

        public void UpdatePoints()
        {
            if (_playerScore)
            {
                for (int i = 0; i < GameManager.Instance.Player.Points; i++)
                {
                    PointList[i].GetComponent<Image>().color = _enabledColor;
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.Opponent.Points; i++)
                {
                    PointList[i].GetComponent<Image>().color = _enabledColor;
                }
            }
        }
    }
}
