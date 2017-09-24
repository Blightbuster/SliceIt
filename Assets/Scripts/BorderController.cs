using UnityEngine;

public class BorderController : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;

    // Use this for initialization
    private void Start()
    {
        // Set left and right border at screenborder
        Left.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0.0f));
        Right.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 0.0f));
    }
}