using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    public List<Sprite> NumberSprites;
    public List<GameObject> DiplayDigits;

    void Update()
    {
        float totalWeigh = 0;

        foreach (GameObject go in GameObject.Find("GameManager").GetComponent<TagController>().TagWeigh) totalWeigh += go.GetComponent<Rigidbody2D>().mass;

        SetDisplayValue((int)totalWeigh);
    }

    public void SetDisplayValue(int n)
    {
        String number = (string)IntToString(n);    // Convert given number to a string with (if nescessary) additional "0" infront

        if (number.Length != 5) return;    // Can the display even show the number?

        number = Reverse(number);   // Reverse number because the loop is going throug the digits backwards

        for (int digintCount = 0; digintCount < 5; digintCount++)
        {
            int numberIndex = (int)char.GetNumericValue(number[digintCount]);  // Get the equivalent spriteIndex of the given number
            DiplayDigits[digintCount].GetComponent<SpriteRenderer>().sprite = NumberSprites[numberIndex];   // Set digit-sprite to the desired number
        }
    }

    public static string Reverse(string s)
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
