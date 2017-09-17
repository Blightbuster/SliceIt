using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded;
    private int _currentTouching;

    void OnCollisionEnter2D(Collision2D collision)
    {
        _currentTouching++;
        CheckStatus();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        _currentTouching--;
        CheckStatus();
    }

    private void CheckStatus()
    {
        IsGrounded = _currentTouching >= 1;
    }
}
