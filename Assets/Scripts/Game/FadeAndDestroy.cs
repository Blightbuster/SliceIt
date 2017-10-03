using UnityEngine;

// Helper component to fade a gameobject out and then 
// destroy it once invisible
namespace Game
{
    [RequireComponent(typeof(SlicedSprite))]
    public class FadeAndDestroy : MonoBehaviour
    {
        private Color _initialColor;
        private Material _material;
        private Rigidbody2D _rigidBody;

        private SlicedSprite _slicedSprite;
        private float _timer;
        public float FadeDelay;
        public float FadeTime = 1.0f;
        public float StationaryVelocity = 0.2f;
        public bool WaitUntilStationary;

        // Called on script construction
        private void Awake()
        {
            _slicedSprite = GetComponent<SlicedSprite>();
            _rigidBody = _slicedSprite.GetComponent<Rigidbody2D>();
            _material = _slicedSprite.GetComponent<Renderer>().material;
            _initialColor = _material.color;
            tag = "Destroy";
        }

        // Update this instance
        private void Update()
        {
            if (!WaitUntilStationary || _rigidBody.velocity.sqrMagnitude < StationaryVelocity * StationaryVelocity)
            {
                _timer += Time.deltaTime;

                if (FadeTime > 0)
                {
                    var newColor = _initialColor;
                    newColor.a = 1.0f - Mathf.Clamp01((_timer - FadeDelay) / FadeTime);
                    _slicedSprite.GetComponent<Renderer>().material.color = newColor;
                }

                if (_timer - FadeDelay >= FadeTime)
                    Destroy(gameObject);
            }
        }
    }
}