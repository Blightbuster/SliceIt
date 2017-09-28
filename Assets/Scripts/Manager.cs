using UnityEngine;

public class Manager : MonoBehaviour
{
    public static GameManager GameManager;

    void Start()
    {
        GameManager = GetComponent<GameManager>();
    }
}
