using cakeslice;
using UnityEngine;

namespace Game
{
    public class SliceOutlineUpdater : MonoBehaviour
    {
        private Outline _outline;

        // Use this for initialization
        private void Start()
        {
            _outline = GetComponent<Outline>();
        }

        // Update is called once per frame
        private void Update()
        {
            _outline.color = Controller.TagController.TagWeigh.Contains(gameObject) ? 2 : 1;  // Change outline-color of slice when on the scale to Color-1 else Color-0
        }
    }
}
