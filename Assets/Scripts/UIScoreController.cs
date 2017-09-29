using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private bool _playerScore;
    [SerializeField] private GameObject _parantGameObject;
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private int _spacing;
    [SerializeField] private Color _enabledColor;
    [SerializeField] private Color _disabledColor;

    private List<GameObject> PointList = new List<GameObject>();

    public void Setup()
    {
        float xPos = _parantGameObject.transform.position.x;
        float yPos = _parantGameObject.transform.position.y;

        for (int i = 0; i < Manager.GameManager.PointsForWin; i++)
        {
            Vector3 pos = new Vector3(xPos + i * _spacing, yPos, 0);
            PointList.Add(Instantiate(_pointPrefab, pos, Quaternion.identity, _parantGameObject.transform));
            PointList.Last().GetComponent<Image>().color = _disabledColor;
        }
    }

    public void UpdatePoints()
    {
        if (_playerScore)
        {
            for (int i = 0; i < Manager.GameManager.Player.Points; i++)
            {
                PointList[i].GetComponent<Image>().color = _enabledColor;
            }
        }
        else
        {
            for (int i = 0; i < Manager.GameManager.Opponent.Points; i++)
            {
                PointList[i].GetComponent<Image>().color = _enabledColor;
            }
        }
    }
}
