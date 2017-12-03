using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TagController : MonoBehaviour
    {
        public List<GameObject> TagWeigh;
        public List<GameObject> TagSlice;

        public GameObject Scale;

        private Vector2 _scalLowerLeftCorner;

        // Use this for initialization
        private void Start()
        {
            Bounds bounds = Scale.GetComponent<SpriteRenderer>().bounds; // Get the boundingbox of the parents sprite
            _scalLowerLeftCorner = bounds.min; // bounds.min is the lower-left corner of the sprite
        }

        // LateUpdate is execute after all update cycles are finished
        // Here you have to use LateUpdate because some slices may move in update functions
        private void LateUpdate()
        {
            TagWeigh.Clear();

            foreach (SpriteSlicer2DSliceInfo sliceInfo in ObjectSlicer.Instance.SlicedSpriteInfo)
            {
                sliceInfo.ChildObjects.RemoveAll(item => item == null);
                foreach (GameObject go in sliceInfo.ChildObjects)
                {
                    if (go.activeSelf && go.GetComponent<MeshRenderer>().bounds.center.x > _scalLowerLeftCorner.x)
                    {
                        if (go.GetComponent<GroundChecker>().IsGrounded && !go.CompareTag("Destroy")) TagWeigh.Add(go);
                    }
                    if (go.activeSelf && !go.CompareTag("Destroy"))
                    {
                        TagSlice.Add(go);
                    }
                }
            }
        }
    }
}
