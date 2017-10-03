using cakeslice;
using UnityEngine;

public class SliceOutlineUpdater : MonoBehaviour
{
    private TagController _tagController;
    private Outline _outline;

    // Use this for initialization
    void Start()
    {
        _tagController = Manager.GameManager.GetComponent<TagController>();
        _outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        _outline.color = _tagController.TagWeigh.Contains(gameObject) ? 2 : 1;  // Change outline-color of slice when on the scale to Color-1 else Color-0
    }
}
