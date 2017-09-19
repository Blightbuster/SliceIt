using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    public List<Sprite> NumberSprites;
    public List<GameObject> DiplayDigits;

    public float TargetNumber
    {
        get { return _targetNumber; }
        set
        {
            if (Math.Abs(_targetNumber - value) > 0.5f)
            {
                _numberDifference = Mathf.Abs(value - _currentNumber);
                _stepValue = (float)Time.fixedDeltaTime * _numberDifference / TransitionTime;
            }
            _targetNumber = value;
        }
    }

    public float TransitionTime = 1.0f;

    private float _targetNumber;
    private float _currentNumber;
    private float _numberDifference;
    private float _stepValue;

    void FixedUpdate()
    {
        if (Math.Abs(_currentNumber - _targetNumber) > 0.5f)
        {
            if (_currentNumber < _targetNumber) SetDisplayValue(_currentNumber + _stepValue);
            else if (_currentNumber > _targetNumber) SetDisplayValue(_currentNumber - _stepValue);

            if (_currentNumber < 0) _currentNumber = 0;
        }
    }

    private void SetDisplayValue(float n)
    {
        _currentNumber = n;
        string number = (string)IntToString((int)n);    // Convert given number to a string with (if nescessary) additional "0" infront

        if (number.Length != 5) return;    // Can the display even show the number?

        number = Reverse(number);   // Reverse number because the loop is going throug the digits backwards

        for (int digintCount = 0; digintCount < 5; digintCount++)
        {
            int numberIndex = (int)char.GetNumericValue(number[digintCount]);  // Get the equivalent spriteIndex of the given number
            DiplayDigits[digintCount].GetComponent<SpriteRenderer>().sprite = NumberSprites[numberIndex];   // Set digit-sprite to the desired number
        }
    }

    private static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    private string IntToString(int number)
    {
        string finishedNumber = number.ToString();
        while (finishedNumber.Length < 5)
        {
            finishedNumber = "0" + finishedNumber;
        }
        return finishedNumber;
    }
}
